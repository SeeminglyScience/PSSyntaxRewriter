using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace ClosureRewriter
{
    internal static class Unstable
    {
        internal static readonly InvokeWithPipeProxy InvokeWithPipe = CreateInvokeWithPipeDelegate();

        internal static readonly Func<Collection<PSObject>, object> CreateNewPipe = CreateNewPipeDelegate();

        internal delegate void InvokeWithPipeProxy(
            ScriptBlock scriptBlock,
            bool useLocalScope,
            object dollarUnder,
            object input,
            object scriptThis,
            object outputPipe,
            InvocationInfo invocationInfo,
            bool propagateAllExceptionsToTop,
            List<PSVariable> variablesToDefine,
            Dictionary<string, ScriptBlock> functionsToDefine,
            object[] args);


        private static Func<Collection<PSObject>, object> CreateNewPipeDelegate()
        {
            ParameterExpression outputList = Parameter(typeof(Collection<PSObject>), nameof(outputList));
            var pipeType = typeof(PSObject).Assembly.GetType("System.Management.Automation.Internal.Pipe");
            return Lambda<Func<Collection<PSObject>, object>>(
                Convert(
                    New(
                        pipeType.GetConstructor(
                            BindingFlags.NonPublic | BindingFlags.Instance,
                            null,
                            new[] { typeof(Collection<PSObject>) },
                            new[] { new ParameterModifier(1) }),
                        outputList),
                    typeof(object)),
                new[] { outputList })
                .Compile();
        }

        private static InvokeWithPipeProxy CreateInvokeWithPipeDelegate()
        {
            ParameterExpression scriptBlock = Parameter(typeof(ScriptBlock), nameof(scriptBlock));
            ParameterExpression useLocalScope = Parameter(typeof(bool), nameof(useLocalScope));
            ParameterExpression dollarUnder = Parameter(typeof(object), nameof(dollarUnder));
            ParameterExpression input = Parameter(typeof(object), nameof(input));
            ParameterExpression scriptThis = Parameter(typeof(object), nameof(scriptThis));
            ParameterExpression outputPipe = Parameter(typeof(object), nameof(outputPipe));
            ParameterExpression invocationInfo = Parameter(typeof(InvocationInfo), nameof(invocationInfo));
            ParameterExpression propagateAllExceptionsToTop = Parameter(typeof(bool), nameof(propagateAllExceptionsToTop));
            ParameterExpression variablesToDefine = Parameter(typeof(List<PSVariable>), nameof(variablesToDefine));
            ParameterExpression functionsToDefine = Parameter(typeof(Dictionary<string, ScriptBlock>), nameof(functionsToDefine));
            ParameterExpression args = Parameter(typeof(object[]), nameof(args));

            var pipeType =
                typeof(PSObject)
                    .Assembly
                    .GetType("System.Management.Automation.Internal.Pipe");
            var errorBehaviorType =
                typeof(PSObject)
                    .Assembly
                    .GetType("System.Management.Automation.ScriptBlock+ErrorHandlingBehavior");

            var iwpMethod = typeof(ScriptBlock).GetMethod(
                "InvokeWithPipe",
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[]
                {
                    typeof(bool),
                    errorBehaviorType,
                    typeof(object),
                    typeof(object),
                    typeof(object),
                    pipeType,
                    typeof(InvocationInfo),
                    typeof(bool),
                    typeof(List<PSVariable>),
                    typeof(Dictionary<string, ScriptBlock>),
                    typeof(object[])
                },
                new[] { new ParameterModifier(10) });

            return Lambda<InvokeWithPipeProxy>(
                Call(
                    scriptBlock,
                    iwpMethod,
                    useLocalScope,
                    Expression.Field(null, errorBehaviorType.GetField("WriteToCurrentErrorPipe")),
                    dollarUnder,
                    input,
                    scriptThis,
                    Expression.Convert(outputPipe, pipeType),
                    invocationInfo,
                    propagateAllExceptionsToTop,
                    variablesToDefine,
                    functionsToDefine,
                    args),
                new[]
                {
                    scriptBlock,
                    useLocalScope,
                    dollarUnder,
                    input,
                    scriptThis,
                    outputPipe,
                    invocationInfo,
                    propagateAllExceptionsToTop,
                    variablesToDefine,
                    functionsToDefine,
                    args,
                })
                .Compile();
        }
    }
}
