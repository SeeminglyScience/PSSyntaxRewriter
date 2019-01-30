using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;

namespace PSSyntaxRewriter
{
    public static class AstUpdateExtensions
    {
        private static IEnumerable<TAst> CloneAll<TAst>(this IEnumerable<TAst> source)
            where TAst : Ast
        {
            if (source == null)
            {
                return Enumerable.Empty<TAst>();
            }

            if (source is TAst[] asArray)
            {
                for (int i = 0; i < asArray.Length; i++)
                {
                    asArray[i] = (TAst)asArray[i].Copy();
                }

                return asArray;
            }

            return source.EnumerateAndCloneAll();
        }

        private static IEnumerable<TAst> EnumerateAndCloneAll<TAst>(this IEnumerable<TAst> source)
            where TAst : Ast
        {
            foreach (TAst ast in source)
            {
                yield return (TAst)ast.Copy();
            }
        }

        internal static TAst Clone<TAst>(this TAst source)
            where TAst : Ast
        {
            if (source == null)
            {
                return null;
            }

            return (TAst)source.Copy();
        }

        public static ArrayExpressionAst Update(this ArrayExpressionAst ast, IEnumerable<StatementAst> statements)
        {
            return new ArrayExpressionAst(
                ast.Extent,
                new StatementBlockAst(
                    ast.SubExpression.Extent,
                    statements.CloneAll(),
                    ast.SubExpression.Traps.CloneAll()));
        }

        public static ArrayLiteralAst Update(this ArrayLiteralAst ast, IEnumerable<ExpressionAst> expressions)
        {
            return new ArrayLiteralAst(ast.Extent, expressions.CloneAll().ToArray());
        }

        public static AssignmentStatementAst Update(
            this AssignmentStatementAst ast,
            ExpressionAst left = null,
            TokenKind? @operator = null,
            StatementAst right = null)
        {
            return new AssignmentStatementAst(
                ast.Extent,
                left?.Clone() ?? ast.Left.Clone(),
                @operator ?? ast.Operator,
                right?.Clone() ?? ast.Right.Clone(),
                ast.ErrorPosition);
        }

        public static AttributeAst Update(
            this AttributeAst ast,
            ITypeName typeName = null,
            IEnumerable<ExpressionAst> positionalArguments = null,
            IEnumerable<NamedAttributeArgumentAst> namedArguments = null)
        {
            return new AttributeAst(
                ast.Extent,
                typeName ?? ast.TypeName,
                positionalArguments?.CloneAll() ?? ast.PositionalArguments.CloneAll(),
                namedArguments?.CloneAll() ?? ast.NamedArguments.CloneAll());
        }

        public static AttributedExpressionAst Update(
            this AttributedExpressionAst ast,
            AttributeBaseAst attribute = null,
            ExpressionAst child = null)
        {
            return new AttributedExpressionAst(
                ast.Extent,
                attribute?.Clone() ?? ast.Attribute.Clone(),
                child?.Clone() ?? ast.Child.Clone());
        }

        public static BinaryExpressionAst Update(
            this BinaryExpressionAst ast,
            ExpressionAst left = null,
            TokenKind? @operator = null,
            ExpressionAst right = null)
        {
            return new BinaryExpressionAst(
                ast.Extent,
                left?.Clone() ?? ast.Left.Clone(),
                @operator ?? ast.Operator,
                right?.Clone() ?? ast.Right.Clone(),
                ast.ErrorPosition);
        }

        public static BlockStatementAst Update(
            this BlockStatementAst ast,
            Token kind = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new BlockStatementAst(
                ast.Extent,
                kind ?? ast.Kind,
                new StatementBlockAst(
                    ast.Body.Extent,
                    statements?.CloneAll() ?? ast.Body.Statements.CloneAll(),
                    ast.Body.Traps?.CloneAll() ?? Enumerable.Empty<TrapStatementAst>()));
        }

        public static BreakStatementAst Update(this BreakStatementAst ast, ExpressionAst label = null)
        {
            return new BreakStatementAst(ast.Extent, label?.Clone() ?? ast.Label?.Clone());
        }

        public static CatchClauseAst Update(
            this CatchClauseAst ast,
            IEnumerable<TypeConstraintAst> catchTypes = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new CatchClauseAst(
                ast.Extent,
                catchTypes?.CloneAll() ?? ast.CatchTypes?.CloneAll(),
                ast.Body?.Update(statements));
        }

        public static StatementBlockAst Update(
            this StatementBlockAst ast,
            IEnumerable<StatementAst> statements = null,
            IEnumerable<TrapStatementAst> traps = null)
        {
            return new StatementBlockAst(
                ast.Extent,
                statements?.CloneAll() ?? ast.Statements.CloneAll(),
                traps?.CloneAll() ?? ast.Traps.CloneAll());
        }

        public static CommandAst Update(
            this CommandAst ast,
            IEnumerable<CommandElementAst> commandElements = null,
            TokenKind? invocationOperator = null,
            IEnumerable<RedirectionAst> redirections = null)
        {
            return new CommandAst(
                ast.Extent,
                commandElements?.CloneAll() ?? ast.CommandElements.CloneAll(),
                invocationOperator ?? ast.InvocationOperator,
                redirections?.CloneAll() ?? ast.Redirections.CloneAll());
        }

        public static CommandExpressionAst Update(
            this CommandExpressionAst ast,
            ExpressionAst expression = null,
            IEnumerable<RedirectionAst> redirections = null)
        {
            return new CommandExpressionAst(
                ast.Extent,
                expression?.Clone() ?? ast.Expression,
                redirections?.CloneAll() ?? ast.Redirections.CloneAll());
        }

        public static CommandParameterAst Update(
            this CommandParameterAst ast,
            string parameterName = null,
            ExpressionAst argument = null)
        {
            return new CommandParameterAst(
                ast.Extent,
                parameterName ?? ast.ParameterName,
                argument?.Clone() ?? ast.Argument?.Clone(),
                ast.ErrorPosition);
        }

        public static ConstantExpressionAst Update(this ConstantExpressionAst ast, object value = null)
        {
            return new ConstantExpressionAst(ast.Extent, value ?? ast.Value);
        }

        public static ContinueStatementAst Update(this ContinueStatementAst ast, ExpressionAst label = null)
        {
            return new ContinueStatementAst(ast.Extent, label?.Clone() ?? ast.Label?.Clone());
        }

        public static ConvertExpressionAst Update(
            this ConvertExpressionAst ast,
            TypeConstraintAst typeConstraint = null,
            ExpressionAst child = null)
        {
            return new ConvertExpressionAst(
                ast.Extent,
                typeConstraint?.Clone() ?? ast.Type.Clone(),
                child?.Clone() ?? ast.Child.Clone());
        }

        public static DataStatementAst Update(
            this DataStatementAst ast,
            string variableName = null,
            IEnumerable<ExpressionAst> commandsAllowed = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new DataStatementAst(
                ast.Extent,
                variableName ?? ast.Variable,
                commandsAllowed?.CloneAll() ?? ast.CommandsAllowed.CloneAll(),
                ast.Body.Update(statements));
        }

        public static DoUntilStatementAst Update(
            this DoUntilStatementAst ast,
            string label = null,
            PipelineBaseAst condition = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new DoUntilStatementAst(
                ast.Extent,
                label ?? ast.Label,
                condition?.Clone() ?? ast.Condition.Clone(),
                ast.Body.Update(statements));
        }

        public static DoWhileStatementAst Update(
            this DoWhileStatementAst ast,
            string label = null,
            PipelineBaseAst condition = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new DoWhileStatementAst(
                ast.Extent,
                label ?? ast.Label,
                condition?.Clone() ?? ast.Condition.Clone(),
                ast.Body.Update(statements));
        }

        public static ErrorExpressionAst Update(this ErrorExpressionAst ast, IEnumerable<Ast> nestedAsts = null)
        {
            return (ErrorExpressionAst)typeof(ErrorExpressionAst).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(IScriptExtent), typeof(IEnumerable<Ast>) },
                new[] { new ParameterModifier(2) })
                .Invoke(new object[] { ast.Extent, nestedAsts?.CloneAll() ?? ast.NestedAst.CloneAll() });
        }

        public static ErrorStatementAst Update(this ErrorStatementAst ast)
        {
            return ast.Clone();
        }

        public static ExitStatementAst Update(this ExitStatementAst ast, PipelineBaseAst pipeline = null)
        {
            return new ExitStatementAst(
                ast.Extent,
                pipeline?.Clone() ?? ast.Pipeline?.Clone());
        }

        public static ExpandableStringExpressionAst Update(
            this ExpandableStringExpressionAst ast,
            string value = null,
            StringConstantType? type = null)
        {
            return ast.Clone();
        }

        public static FileRedirectionAst Update(
            this FileRedirectionAst ast,
            RedirectionStream? stream = null,
            ExpressionAst file = null,
            bool? append = null)
        {
            return new FileRedirectionAst(
                ast.Extent,
                stream ?? ast.FromStream,
                file?.Clone() ?? ast.Location.Clone(),
                append ?? ast.Append);
        }

        public static ForEachStatementAst Update(
            this ForEachStatementAst ast,
            VariableExpressionAst variable = null,
            PipelineBaseAst expression = null,
            IEnumerable<StatementAst> statements = null,
            string label = null,
            ForEachFlags? flags = null)
        {
            return new ForEachStatementAst(
                ast.Extent,
                label ?? ast.Label,
                flags ?? ast.Flags,
                variable?.Clone() ?? ast.Variable.Clone(),
                expression?.Clone() ?? ast.Condition.Clone(),
                ast.Body.Update(statements));
        }

        public static ForStatementAst Update(
            this ForStatementAst ast,
            PipelineBaseAst initializer = null,
            PipelineBaseAst condition = null,
            PipelineBaseAst iterator = null,
            IEnumerable<StatementAst> statements = null,
            string label = null)
        {
            return new ForStatementAst(
                ast.Extent,
                label ?? ast.Label,
                initializer?.Clone() ?? ast.Initializer.Clone(),
                condition?.Clone() ?? ast.Condition.Clone(),
                iterator?.Clone() ?? ast.Iterator?.Clone(),
                ast.Body.Update(statements));
        }

        public static FunctionDefinitionAst Update(
            this FunctionDefinitionAst ast,
            string name = null,
            ScriptBlockAst body = null,
            IEnumerable<ParameterAst> parameters = null,
            bool? isFilter = null,
            bool? isWorkflow = null)
        {
            return new FunctionDefinitionAst(
                ast.Extent,
                isFilter ?? ast.IsFilter,
                isWorkflow ?? ast.IsWorkflow,
                name ?? ast.Name,
                parameters?.CloneAll() ?? ast.Parameters?.CloneAll(),
                body.Clone() ?? ast.Body.Clone());
        }

        private static Tuple<ExpressionAst, StatementAst> CloneKeyValuePair(Tuple<ExpressionAst, StatementAst> kvp)
        {
            return Tuple.Create(kvp.Item1.Clone(), kvp.Item2.Clone());
        }

        public static HashtableAst Update(
            this HashtableAst ast,
            IEnumerable<Tuple<ExpressionAst, StatementAst>> keyValuePairs = null)
        {
            return new HashtableAst(
                ast.Extent,
                keyValuePairs?.CloneAll() ?? ast.KeyValuePairs?.CloneAll());
        }

        public static Tuple<TAst, TAst2> CloneAstTuple<TAst, TAst2>(Tuple<TAst, TAst2> tuple)
            where TAst : Ast
            where TAst2 : Ast
        {
            return Tuple.Create(tuple.Item1.Clone(), tuple.Item2.Clone());
        }

        public static IEnumerable<Tuple<TAst, TAst2>> CloneAll<TAst, TAst2>(this IEnumerable<Tuple<TAst, TAst2>> source)
            where TAst : Ast
            where TAst2 : Ast
        {
            if (source is Tuple<TAst, TAst2>[] sourceArray)
            {
                var resultArray = new Tuple<TAst, TAst2>[sourceArray.Length];
                for (int i = 0; i < resultArray.Length; i++)
                {
                    resultArray[i] = CloneAstTuple(sourceArray[i]);
                }

                return resultArray;
            }

            return ReturnEnumerable();
            IEnumerable<Tuple<TAst, TAst2>> ReturnEnumerable()
            {
                foreach (Tuple<TAst, TAst2> tuple in source)
                {
                    yield return CloneAstTuple(tuple);
                }
            }
        }

        public static IfStatementAst Update(
            this IfStatementAst ast,
            IEnumerable<Tuple<PipelineBaseAst, StatementBlockAst>> clauses = null,
            IEnumerable<StatementAst> elseStatements = null)
        {
            return new IfStatementAst(
                ast.Extent,
                clauses?.Select(CloneAstTuple) ?? ast.Clauses?.Select(CloneAstTuple),
                ast.ElseClause?.Update(elseStatements));
        }

        public static IndexExpressionAst Update(
            this IndexExpressionAst ast,
            ExpressionAst target = null,
            ExpressionAst index = null)
        {
            return new IndexExpressionAst(
                ast.Extent,
                target?.Clone() ?? ast.Target.Clone(),
                index?.Clone() ?? ast.Index.Clone());
        }

        public static InvokeMemberExpressionAst Update(
            this InvokeMemberExpressionAst ast,
            ExpressionAst expression = null,
            CommandElementAst method = null,
            IEnumerable<ExpressionAst> arguments = null,
            bool? @static = null)
        {
            return new InvokeMemberExpressionAst(
                ast.Extent,
                expression?.Clone() ?? ast.Expression.Clone(),
                method?.Clone() ?? ast.Member.Clone(),
                arguments?.CloneAll() ?? ast.Arguments.CloneAll(),
                @static ?? ast.Static);
        }

        public static MemberExpressionAst Update(
            this MemberExpressionAst ast,
            ExpressionAst expression = null,
            CommandElementAst member = null,
            bool? @static = null)
        {
            return new MemberExpressionAst(
                ast.Extent,
                expression?.Clone() ?? ast.Expression.Clone(),
                member?.Clone() ?? ast.Member.Clone(),
                @static ?? ast.Static);
        }

        public static MergingRedirectionAst Update(
            this MergingRedirectionAst ast,
            RedirectionStream? from = null,
            RedirectionStream? to = null)
        {
            return new MergingRedirectionAst(
                ast.Extent,
                from ?? ast.FromStream,
                to ?? ast.ToStream);
        }

        public static NamedAttributeArgumentAst Update(
            NamedAttributeArgumentAst ast,
            string argumentName = null,
            ExpressionAst argument = null,
            bool? expressionOmitted = null)
        {
            return new NamedAttributeArgumentAst(
                ast.Extent,
                argumentName ?? ast.ArgumentName,
                argument?.Clone() ?? ast.Argument?.Clone(),
                expressionOmitted ?? ast.ExpressionOmitted);
        }

        public static NamedBlockAst Update(
            this NamedBlockAst ast,
            TokenKind? blockName = null,
            IEnumerable<StatementAst> statements = null,
            bool? unnamed = null)
        {
            return new NamedBlockAst(
                ast.Extent,
                blockName ?? ast.BlockKind,
                new StatementBlockAst(
                    ast.Extent,
                    statements?.CloneAll() ?? ast.Statements.CloneAll(),
                    ast.Traps?.CloneAll()),
                unnamed ?? ast.Unnamed);
        }

        public static ParamBlockAst Update(
            this ParamBlockAst ast,
            IEnumerable<ParameterAst> parameters = null,
            IEnumerable<AttributeAst> attributes = null)
        {
            return new ParamBlockAst(
                ast.Extent,
                attributes?.CloneAll() ?? ast.Attributes?.CloneAll(),
                parameters?.CloneAll() ?? ast.Parameters?.CloneAll());
        }

        public static ParameterAst Update(
            this ParameterAst ast,
            VariableExpressionAst name = null,
            IEnumerable<AttributeAst> attributes = null,
            ExpressionAst defaultValue = null)
        {
            return new ParameterAst(
                ast.Extent,
                name?.Clone() ?? ast.Name.Clone(),
                attributes?.CloneAll() ?? ast.Attributes?.CloneAll(),
                defaultValue?.Clone() ?? ast.DefaultValue?.Clone());
        }

        public static ParenExpressionAst Update(this ParenExpressionAst ast, PipelineBaseAst pipeline = null)
        {
            return new ParenExpressionAst(ast.Extent, pipeline?.Clone() ?? ast.Pipeline?.Clone());
        }

        public static PipelineAst Update(this PipelineAst ast, IEnumerable<CommandBaseAst> pipelineElements = null)
        {
            return new PipelineAst(
                ast.Extent,
                pipelineElements?.CloneAll() ?? ast.PipelineElements?.CloneAll());
        }

        public static PipelineAst Update(this PipelineAst ast, CommandBaseAst commandAst)
        {
            return new PipelineAst(
                ast.Extent,
                commandAst);
        }

        public static ReturnStatementAst Update(this ReturnStatementAst ast, PipelineBaseAst pipeline = null)
        {
            return new ReturnStatementAst(ast.Extent, pipeline?.Clone() ?? ast.Pipeline?.Clone());
        }

        public static ScriptBlockAst Update(
            this ScriptBlockAst ast,
            IEnumerable<UsingStatementAst> usingStatements = null,
            IEnumerable<AttributeAst> attributes = null,
            ParamBlockAst paramBlock = null,
            NamedBlockAst beginBlock = null,
            NamedBlockAst processBlock = null,
            NamedBlockAst endBlock = null,
            NamedBlockAst dynamicParamBlock = null)
        {
            return new ScriptBlockAst(
                ast.Extent,
                usingStatements?.CloneAll() ?? ast.UsingStatements?.CloneAll(),
                attributes?.CloneAll() ?? ast.Attributes?.CloneAll(),
                paramBlock?.Clone() ?? ast.ParamBlock?.Clone(),
                beginBlock?.Clone() ?? ast.BeginBlock?.Clone(),
                processBlock?.Clone() ?? ast.ProcessBlock?.Clone(),
                endBlock?.Clone() ?? ast.EndBlock?.Clone(),
                dynamicParamBlock?.Clone() ?? ast.DynamicParamBlock?.Clone());
        }

        public static ScriptBlockAst Update(
            this ScriptBlockAst ast,
            IEnumerable<StatementAst> statements,
            IEnumerable<UsingStatementAst> usingStatements = null,
            IEnumerable<AttributeAst> attributes = null,
            ParamBlockAst paramBlock = null,
            bool? isFilter = null,
            bool? isConfiguration = null)
        {
            return new ScriptBlockAst(
                ast.Extent,
                usingStatements?.CloneAll() ?? ast.UsingStatements?.CloneAll(),
                attributes?.CloneAll() ?? ast.Attributes?.CloneAll(),
                paramBlock?.Clone() ?? ast.ParamBlock?.Clone(),
                new StatementBlockAst(
                    ast.EndBlock.Extent,
                    statements?.CloneAll() ?? ast.EndBlock?.Statements?.CloneAll(),
                    ast.EndBlock?.Traps?.CloneAll()),
                isFilter: false,
                isConfiguration: false);
        }

        public static ScriptBlockExpressionAst Update(
            this ScriptBlockExpressionAst ast,
            ScriptBlockAst scriptBlock = null)
        {
            return new ScriptBlockExpressionAst(
                ast.Extent,
                scriptBlock?.Clone() ?? ast.ScriptBlock?.Clone());
        }

        public static StringConstantExpressionAst Update(
            this StringConstantExpressionAst ast,
            string value = null,
            StringConstantType? stringConstantType = null)
        {
            return new StringConstantExpressionAst(
                ast.Extent,
                value ?? ast.Value,
                stringConstantType ?? ast.StringConstantType);
        }

        public static SubExpressionAst Update(this SubExpressionAst ast, IEnumerable<StatementAst> statements = null)
        {
            return new SubExpressionAst(
                ast.Extent,
                ast.SubExpression?.Update(statements));
        }

        public static SwitchStatementAst Update(
            this SwitchStatementAst ast,
            string label = null,
            PipelineBaseAst condition = null,
            SwitchFlags? flags = null,
            IEnumerable<Tuple<ExpressionAst, StatementBlockAst>> clauses = null,
            StatementBlockAst @default = null)
        {
            return new SwitchStatementAst(
                ast.Extent,
                label ?? ast.Label,
                condition?.Clone() ?? ast.Condition?.Clone(),
                flags ?? ast.Flags,
                clauses?.CloneAll() ?? ast.Clauses?.CloneAll(),
                @default?.Clone() ?? ast.Default?.Clone());
        }

        public static ThrowStatementAst Update(this ThrowStatementAst ast, PipelineBaseAst pipeline = null)
        {
            return new ThrowStatementAst(ast.Extent, pipeline?.Clone() ?? ast.Pipeline?.Clone());
        }

        public static TrapStatementAst Update(
            this TrapStatementAst ast,
            TypeConstraintAst trapType = null,
            IEnumerable<StatementAst> statements = null)
        {
            return new TrapStatementAst(
                ast.Extent,
                trapType?.Clone() ?? ast.TrapType?.Clone(),
                ast.Body?.Update(statements));
        }

        public static TryStatementAst Update(
            this TryStatementAst ast,
            StatementBlockAst body = null,
            IEnumerable<CatchClauseAst> catchClauses = null,
            StatementBlockAst @finally = null)
        {
            return new TryStatementAst(
                ast.Extent,
                body?.Clone() ?? ast.Body?.Clone(),
                catchClauses?.CloneAll() ?? ast.CatchClauses?.CloneAll(),
                @finally?.Clone() ?? ast.Finally?.Clone());
        }

        public static TypeConstraintAst Update(this TypeConstraintAst ast, ITypeName typeName = null)
        {
            return new TypeConstraintAst(ast.Extent, typeName ?? ast.TypeName);
        }

        public static TypeConstraintAst Update(
            this TypeConstraintAst ast,
            string typeName)
        {
            return new TypeConstraintAst(
                ast.Extent,
                new TypeName(ast.TypeName.Extent, typeName));
        }

        public static TypeConstraintAst Update(
            this TypeConstraintAst ast,
            string typeName,
            int arrayRank)
        {
            return new TypeConstraintAst(
                ast.Extent,
                new ArrayTypeName(
                    ast.TypeName.Extent,
                    new TypeName(ast.TypeName.Extent, typeName),
                    arrayRank));
        }

        public static TypeConstraintAst Update(
            this TypeConstraintAst ast,
            string typeName,
            IEnumerable<ITypeName> genericTypeArguments)
        {
            return new TypeConstraintAst(
                ast.Extent,
                new GenericTypeName(
                    ast.TypeName.Extent,
                    new TypeName(ast.TypeName.Extent, typeName),
                    genericTypeArguments));
        }

        public static TypeExpressionAst Update(this TypeExpressionAst ast, ITypeName typeName = null)
        {
            return new TypeExpressionAst(ast.Extent, typeName ?? ast.TypeName);
        }

        public static TypeExpressionAst Update(
            this TypeExpressionAst ast,
            string typeName)
        {
            return new TypeExpressionAst(
                ast.Extent,
                new TypeName(ast.TypeName.Extent, typeName));
        }

        public static TypeExpressionAst Update(
            this TypeExpressionAst ast,
            string typeName,
            int arrayRank)
        {
            return new TypeExpressionAst(
                ast.Extent,
                new ArrayTypeName(
                    ast.TypeName.Extent,
                    new TypeName(ast.TypeName.Extent, typeName),
                    arrayRank));
        }

        public static TypeExpressionAst Update(
            this TypeExpressionAst ast,
            string typeName,
            IEnumerable<ITypeName> genericTypeArguments)
        {
            return new TypeExpressionAst(
                ast.Extent,
                new GenericTypeName(
                    ast.TypeName.Extent,
                    new TypeName(ast.TypeName.Extent, typeName),
                    genericTypeArguments));
        }

        public static UnaryExpressionAst Update(
            this UnaryExpressionAst ast,
            TokenKind? tokenKind = null,
            ExpressionAst child = null)
        {
            return new UnaryExpressionAst(
                ast.Extent,
                tokenKind ?? ast.TokenKind,
                child?.Clone() ?? ast.Child?.Clone());
        }

        public static UsingExpressionAst Update(this UsingExpressionAst ast, ExpressionAst expressionAst = null)
        {
            return new UsingExpressionAst(
                ast.Extent,
                expressionAst?.Clone() ?? ast.SubExpression?.Clone());
        }

        public static VariableExpressionAst Update(
            this VariableExpressionAst ast,
            VariablePath variablePath = null,
            bool? splatted = null)
        {
            return new VariableExpressionAst(
                ast.Extent,
                variablePath ?? ast.VariablePath,
                splatted ?? ast.Splatted);
        }

        public static VariableExpressionAst Update(
            this VariableExpressionAst ast,
            string variableName)
        {
            return new VariableExpressionAst(
                ast.Extent,
                variableName,
                ast.Splatted);
        }

        public static VariableExpressionAst Update(
            this VariableExpressionAst ast,
            bool splatted)
        {
            return new VariableExpressionAst(
                ast.Extent,
                ast.VariablePath,
                splatted);
        }

        public static VariableExpressionAst Update(
            this VariableExpressionAst ast,
            string variableName,
            bool splatted)
        {
            return new VariableExpressionAst(
                ast.Extent,
                variableName,
                splatted);
        }

        public static WhileStatementAst Update(
            this WhileStatementAst ast,
            PipelineBaseAst condition = null,
            StatementBlockAst body = null,
            string label = null)
        {
            return new WhileStatementAst(
                ast.Extent,
                label ?? ast.Label,
                condition?.Clone() ?? ast.Condition?.Clone(),
                body?.Clone() ?? ast.Body?.Clone());
        }
    }
}
