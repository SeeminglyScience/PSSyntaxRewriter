using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;

using static PSSyntaxRewriter.AstType;

namespace PSSyntaxRewriter
{
    public static class AstTransformExtensions
    {
        public static StatementBlockAst As(
            this Ast source,
            AstType<StatementBlockAst> targetType,
            params StatementAst[] statements)
        {
            return new StatementBlockAst(
                source.Extent,
                statements,
                Enumerable.Empty<TrapStatementAst>());
        }

        public static IfStatementAst As(
            this Ast source,
            AstType<IfStatementAst> targetType,
            PipelineBaseAst condition,
            StatementBlockAst ifTrueBody,
            StatementBlockAst ifFalseBody)
        {
            return new IfStatementAst(
                source.Extent,
                new[] { Tuple.Create(condition, ifTrueBody) },
                ifFalseBody);
        }

        public static IfStatementAst As(
            this Ast source,
            AstType<IfStatementAst> targetType,
            PipelineBaseAst condition,
            params StatementAst[] statements)
        {
            return new IfStatementAst(
                source.Extent,
                new[] { Tuple.Create(condition, source.As(StatementBlock, statements)) },
                null);
        }

        public static ParenExpressionAst As(
            this Ast source,
            AstType<ParenExpressionAst> targetType,
            PipelineBaseAst pipeline)
        {
            return new ParenExpressionAst(
                source.Extent,
                pipeline);
        }

        public static AssignmentStatementAst As(
            this Ast source,
            AstType<AssignmentStatementAst> targetType,
            ExpressionAst left,
            StatementAst right,
            TokenKind operatorKind = TokenKind.Equals)
        {
            return new AssignmentStatementAst(
                source.Extent,
                left,
                operatorKind,
                right,
                Empty.Extent(source.Extent));
        }

        public static PipelineAst As(
            this Ast source,
            AstType<PipelineAst> targetType,
            params CommandBaseAst[] commands)
        {
            return new PipelineAst(
                source.Extent,
                commands);
        }

        public static VariableExpressionAst As(
            this Ast source,
            AstType<VariableExpressionAst> targetType,
            string variableName,
            bool isSplatted = false)
        {
            return new VariableExpressionAst(
                source.Extent,
                variableName,
                isSplatted);
        }

        public static CommandAst As(
            this Ast source,
            AstType<CommandAst> targetType,
            string commandName,
            params CommandElementAst[] elements)
        {
            return As(
                source,
                targetType,
                commandName,
                TokenKind.Ampersand,
                elements);
        }

        public static CommandAst As(
            this Ast source,
            AstType<CommandAst> targetType,
            string commandName,
            TokenKind invocationOperator,
            params CommandElementAst[] elements)
        {
            var extent = GetExtentFromConstant(commandName, source.Extent);
            var newElements = new CommandElementAst[elements.Length + 1];
            for (int i = 0; i < elements.Length; i++)
            {
                newElements[i + 1] = elements[i];
            }

            newElements[0] = source.As(StringConstantExpression, commandName);
            return new CommandAst(
                extent,
                newElements,
                invocationOperator,
                Enumerable.Empty<RedirectionAst>());
        }

        public static CommandParameterAst As(
            this Ast source,
            AstType<CommandParameterAst> targetType,
            string parameterName,
            ExpressionAst argument = null)
        {
            return new CommandParameterAst(
                source.Extent,
                parameterName,
                argument,
                Empty.Extent(source.Extent));
        }

        public static StringConstantExpressionAst As(
            this Ast source,
            AstType<StringConstantExpressionAst> targetType,
            string value,
            StringConstantType stringConstantType = StringConstantType.BareWord)
        {

            return new StringConstantExpressionAst(
                GetExtentFromConstant(value, source.Extent),
                value,
                stringConstantType);
        }

        public static BinaryExpressionAst As(
            this Ast source,
            AstType<BinaryExpressionAst> targetType,
            ExpressionAst left,
            TokenKind @operator,
            ExpressionAst right,
            IScriptExtent errorPosition = null)
        {
            return new BinaryExpressionAst(
                source.Extent,
                left,
                @operator,
                right,
                errorPosition ?? Empty.Extent(source.Extent));
        }

        public static InvokeMemberExpressionAst As(
            this Ast source,
            AstType<CommandExpressionAst> targetType,
            ExpressionAst expression,
            CommandElementAst method,
            IEnumerable<ExpressionAst> arguments,
            bool @static = false)
        {
            return new InvokeMemberExpressionAst(
                source.Extent,
                expression,
                method,
                arguments,
                @static);
        }

        public static SubExpressionAst As(
            this Ast source,
            AstType<SubExpressionAst> targetType,
            IEnumerable<StatementAst> statements)
        {
            return new SubExpressionAst(
                source.Extent,
                new StatementBlockAst(
                    source.Extent,
                    statements,
                    Enumerable.Empty<TrapStatementAst>()));
        }

        public static SubExpressionAst As(
            this Ast source,
            AstType<SubExpressionAst> targetType,
            params StatementAst[] statements)
        {
            return new SubExpressionAst(
                source.Extent,
                new StatementBlockAst(
                    source.Extent,
                    statements,
                    Enumerable.Empty<TrapStatementAst>()));
        }

        public static CommandExpressionAst As(
            this Ast source,
            AstType<CommandExpressionAst> targetType,
            ExpressionAst expression)
        {
            return new CommandExpressionAst(
                source.Extent,
                expression,
                Enumerable.Empty<RedirectionAst>());
        }

        private static IScriptExtent GetExtentFromConstant(string value, IScriptExtent source)
        {
            string startLine = null;
            int newLineCount = 1;
            int positionInColumn = 1;
            int lastNewLineIndex = -1;
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '\n':
                        if (startLine == null)
                        {
                            startLine = value.Substring(0, i + 1);
                        }

                        lastNewLineIndex = i;
                        newLineCount++;
                        positionInColumn = 1;
                        break;
                    case '\r':
                        break;
                    default:
                        positionInColumn++;
                        break;
                }
            }

            if (startLine == null)
            {
                startLine = value;
            }

            IScriptPosition oldStart = source.StartScriptPosition;
            string fullScript = oldStart.GetFullScript();

            string endLine =
                lastNewLineIndex > -1
                    ? value.Substring(lastNewLineIndex + 1, value.Length - lastNewLineIndex)
                    : value;

            var start = new ScriptPosition(
                oldStart.File,
                1,
                1,
                startLine,
                fullScript);

            var end = new ScriptPosition(
                oldStart.File,
                newLineCount,
                positionInColumn,
                endLine,
                fullScript);

            return new ScriptExtent(start, end);
        }
    }
}
