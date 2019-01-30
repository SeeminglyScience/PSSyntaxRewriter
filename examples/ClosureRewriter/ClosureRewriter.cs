using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using Microsoft.PowerShell.Commands;
using PSSyntaxRewriter;

namespace ClosureRewriter
{
    public class ClosureRewriter : RewritingVisitor
    {
        private readonly SessionState _sessionState;

        private int _localsIndex;

        private int _inheritsIndex;

        private int _parametersIndex;

        private bool _hasThis;

        private readonly Dictionary<string, int> _locals =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, Tuple<int, PSVariable>> _inherits =
            new Dictionary<string, Tuple<int, PSVariable>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, int> _parameters =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> _alreadyProcessed =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private ClosureRewriter(SessionState sessionState)
        {
            _sessionState = sessionState;
        }

        internal static (ScriptBlock, Closure) CreateNewClosure(ScriptBlock scriptBlock)
        {
            SessionState sessionState = scriptBlock.Module == null
                ? GetTopLevelStateFromTLS()
                : scriptBlock.Module.SessionState;

            var writer = new ClosureRewriter(sessionState);
            var newAst = (ScriptBlockAst)scriptBlock.Ast.Rewrite(writer);
            var newSb = newAst.GetScriptBlock();
            var inherits = new PSVariable[writer._inheritsIndex];
            foreach (Tuple<int, PSVariable> inherit in writer._inherits.Values)
            {
                inherits[inherit.Item1] = inherit.Item2;
            }

            var autos = new object[3];
            if (writer._hasThis)
            {
                autos[2] = sessionState.PSVariable.Get("this")?.Value;
            }

            var closure = new Closure(
                inherits,
                new object[writer._localsIndex],
                new object[writer._parametersIndex],
                Array.Empty<CommandInfo>(),
                newSb,
                autos);

            return (newSb, closure);
        }

        public static MulticastDelegate CreateNewClosure(ScriptBlock scriptBlock, Type delegateType)
        {
            if (delegateType == null)
            {
                delegateType = typeof(Func<object>);
            }

            SessionState sessionState = scriptBlock.Module == null
                ? GetTopLevelStateFromTLS()
                : scriptBlock.Module.SessionState;

            var writer = new ClosureRewriter(sessionState);
            var newAst = (ScriptBlockAst)scriptBlock.Ast.Rewrite(writer);
            var newSb = newAst.GetScriptBlock();
            // if (scriptBlock.Module != null)
            // {
            //     newSb = scriptBlock.Module.NewBoundScriptBlock(newSb);
            // }

            var inherits = new PSVariable[writer._inheritsIndex];
            foreach (Tuple<int, PSVariable> inherit in writer._inherits.Values)
            {
                inherits[inherit.Item1] = inherit.Item2;
            }

            var closure = new Closure(
                inherits,
                new object[writer._localsIndex],
                new object[writer._parametersIndex],
                Array.Empty<CommandInfo>(),
                newSb);

            return CreateDelegate(newSb, closure, delegateType);
        }

        private static MulticastDelegate CreateDelegate(ScriptBlock scriptBlock, Closure sourceClosure, Type delegateType)
        {
            var sb = Expression.Constant(scriptBlock, typeof(ScriptBlock));
            var sourceClosureExpr = Expression.Constant(sourceClosure, typeof(Closure));

            var closure = Expression.Variable(typeof(Closure));
            var invokeResult = Expression.Variable(typeof(object));
            var variables = new List<ParameterExpression> { closure, invokeResult };

            MethodInfo invokeMethod = delegateType.GetMethod(nameof(Action.Invoke));
            ParameterInfo[] parameters = invokeMethod.GetParameters();
            ParameterExpression[] parameterExprs;
            Expression[] convertedParameters;
            Type[] delegateTypes;
            Type returnType = invokeMethod.ReturnType;
            if (parameters.Length > 0)
            {
                convertedParameters = new Expression[parameters.Length];
                parameterExprs = new ParameterExpression[parameters.Length];
                delegateTypes = new Type[parameters.Length + 1];
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = Expression.Parameter(parameters[i].ParameterType, parameters[i].Name);
                    parameterExprs[i] = parameter;
                    convertedParameters[i] = Expression.Convert(parameter, typeof(object));
                        // Expression.Convert(

                            // typeof(object));

                    delegateTypes[i] = parameters[i].ParameterType;
                }

                delegateTypes[parameters.Length] = invokeMethod.ReturnType;
            }
            else
            {
                parameterExprs = Array.Empty<ParameterExpression>();
                convertedParameters = Array.Empty<Expression>();
                delegateTypes = new[] { invokeMethod.ReturnType };
            }

            var body = new List<Expression>
            {
                Expression.Assign(
                    closure,
                    Expression.Call(
                        sourceClosureExpr,
                        typeof(Closure).GetMethod(nameof(Closure.Clone), BindingFlags.Instance | BindingFlags.NonPublic),
                        Expression.NewArrayInit(typeof(object), convertedParameters))),

                Expression.Assign(
                    invokeResult,
                    Expression.Call(
                        Expression.Field(closure, typeof(Closure).GetField(nameof(Closure.ScriptBlock))),
                        typeof(ScriptBlock).GetMethod(nameof(ScriptBlock.InvokeReturnAsIs)),
                        Expression.NewArrayInit(typeof(object), Expression.Convert(closure, typeof(object)))))
            };

            if (returnType == typeof(void))
            {
                body.Add(Expression.Empty());
            }
            else
            {
                var convertedResult = Expression.Variable(returnType);
                variables.Add(convertedResult);
                body.Add(
                    Expression.Condition(
                        Expression.Call(
                            typeof(LanguagePrimitives),
                            nameof(LanguagePrimitives.TryConvertTo),
                            new[] { returnType },
                            invokeResult,
                            convertedResult),
                        convertedResult,
                        Expression.Default(returnType)));
            }

            return (MulticastDelegate)Expression.Lambda(
                Expression.GetDelegateType(delegateTypes),
                Expression.Block(
                    returnType,
                    variables,
                    body),
                parameterExprs)
                .Compile();
        }

        public override ExpressionAst VisitVariableExpression(VariableExpressionAst variableExpressionAst)
        {
            return CreateClosureAccess(variableExpressionAst);
        }

        public override ParamBlockAst VisitParamBlock(ParamBlockAst paramBlockAst)
        {
            foreach (ParameterAst parameter in paramBlockAst.Parameters)
            {
                _parameters.Add(parameter.Name.VariablePath.UserPath, _parametersIndex);
                _parametersIndex++;
            }

            return null;
        }

        private ExpressionAst CreateClosureAccess(VariableExpressionAst ast)
        {
            return CreateClosureAccess(ast.Extent, ast.VariablePath.UserPath);
        }

        private ExpressionAst CreateClosureAccess(IScriptExtent extent, string name)
        {
            MaybeProcessMember(name, out int index, out string groupName);
            var closureExpr = new IndexExpressionAst(
                extent,
                new VariableExpressionAst(extent, "args", false),
                new ConstantExpressionAst(extent, 0));

             var groupExpr = new MemberExpressionAst(
                extent,
                closureExpr,
                new StringConstantExpressionAst(extent, groupName, StringConstantType.BareWord),
                @static: false);

            var elementAccess = new IndexExpressionAst(
                extent,
                groupExpr,
                new ConstantExpressionAst(extent, index));

            if (groupName != nameof(Closure.Inherits))
            {
                return elementAccess;
            }

            return new MemberExpressionAst(
                extent,
                elementAccess,
                new StringConstantExpressionAst(extent, nameof(PSVariable.Value), StringConstantType.BareWord),
                @static: false);
        }

        private void MaybeProcessMember(string name, out int index, out string groupName)
        {
            if ((name.Length == 1 && name[0] == '_') || name.Equals("PSItem", StringComparison.OrdinalIgnoreCase))
            {
                index = 0;
                groupName = nameof(Closure.Autos);
                return;
            }

            if (name.Equals("args", StringComparison.OrdinalIgnoreCase))
            {
                index = 1;
                groupName = nameof(Closure.Autos);
                return;
            }

            if (name.Equals("this", StringComparison.OrdinalIgnoreCase))
            {
                index = 2;
                groupName = nameof(Closure.Autos);
                _hasThis = true;
                return;
            }

            if (_parameters.TryGetValue(name, out index))
            {
                groupName = nameof(Closure.Parameters);
                return;
            }

            if (_inherits.TryGetValue(name, out Tuple<int, PSVariable> existingInherit))
            {
                (index, _) = existingInherit;
                groupName = nameof(Closure.Inherits);
                return;
            }

            if (_locals.TryGetValue(name, out index))
            {
                groupName = nameof(Closure.Locals);
                return;
            }

            bool doesExist = _sessionState.InvokeProvider.Item.Exists(
                $"Microsoft.PowerShell.Core\\{VariableProvider.ProviderName}::local:{name}");

            PSVariable variable;
            if (doesExist)
            {
                variable = _sessionState.PSVariable.Get(name);
                _inherits.Add(name, Tuple.Create(_inheritsIndex, variable));
                index = _inheritsIndex;
                _inheritsIndex++;
                groupName = nameof(Closure.Inherits);
                return;
            }

            _locals.Add(name, _localsIndex);
            index = _localsIndex;
            _localsIndex++;
            groupName = nameof(Closure.Locals);
        }

        private static SessionState GetTopLevelStateFromTLS()
        {
            var runspace = Runspace.DefaultRunspace;
            if (runspace == null)
            {
                throw new PSInvalidOperationException("There is no default runspace for this thread.");
            }

            var executionContext = typeof(Runspace)
                .GetProperty("GetExecutionContext", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(runspace);

            var sessionStateInternal = executionContext.GetType()
                .GetProperty("TopLevelSessionState", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(executionContext);

            return (SessionState)sessionStateInternal.GetType()
                .GetProperty("PublicSessionState", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(sessionStateInternal);
        }
    }
}
