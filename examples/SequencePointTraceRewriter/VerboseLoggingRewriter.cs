using System;
using System.Management.Automation.Language;
using PSSyntaxRewriter;

using static PSSyntaxRewriter.Syntax;

namespace SequencePointTraceRewriter
{
    public class VerboseLoggingRewriter : SimpleSyntaxRewriter
    {
        private const string StackVariable = "global:__stack_count__";

        private const string TempVariable = "__temp__";

        private const string WriteHost = "Write-Host";

        private readonly ColorPicker _colors = new ColorPicker(
            ConsoleColor.White,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Gray,
            ConsoleColor.Red,
            ConsoleColor.Magenta);

        public override StatementAst VisitStatement(StatementAst statementAst)
        {
            if (!(CurrentExpectedKind == SyntaxKind.Any ||
                CurrentExpectedKind == SyntaxKind.Statement ||
                CurrentExpectedKind == SyntaxKind.Command ||
                CurrentExpectedKind == SyntaxKind.Pipeline))
            {
                return statementAst;
            }

            CommandExpressionAst result = CreateLoggingExpression(statementAst);
            return CurrentExpectedKind == SyntaxKind.Pipeline
                ? (StatementAst)Pipeline(result)
                : result;
        }

        public override ExpressionAst VisitExpression(ExpressionAst expressionAst)
        {
            if (CurrentExpectedKind == SyntaxKind.Any || CurrentExpectedKind == SyntaxKind.Expression)
            {
                return CreateLoggingExpression(expressionAst);
            }

            return expressionAst;
        }

        public override StatementAst VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
        {
            // Don't rewrite the LHS of an assignment.
            var newAssignment = assignmentStatementAst.Update(
                right: assignmentStatementAst.Right.Rewrite(this));

            return Assignment(Variables.Null, CreateLoggingExpression(newAssignment));
        }

        private CommandExpressionAst CreateLoggingExpression(StatementAst source)
        {
            return CommandExpression(CreateLoggingExpressionImpl(CreateEvaluation(source), source.Extent, source.GetType()));
        }

        private ExpressionAst CreateLoggingExpression(ExpressionAst source)
        {
            return CreateLoggingExpressionImpl(CreateEvaluation(source), source.Extent, source.GetType());
        }

        private ExpressionAst CreateLoggingExpressionImpl(
            StatementAst evaluation,
            IScriptExtent sourceExtent,
            Type astType)
        {
            var color = _colors.Next();
            return Index(
                Array(
                    MaybeCreateStackVar(),
                    IncrementStack(),
                    CreateProcessingMessage(sourceExtent, astType, color),
                    evaluation,
                    CreateProcessedMessage(sourceExtent.EndScriptPosition, color, astType),
                    DecrementStack(),
                    CreateReturnValue()),
                Constant(0));
        }

        private StatementAst CreateProcessingMessage(IScriptExtent source, Type astType, ConsoleColor dashColor)
        {
            ExpressionAst traceInvocation = InvokeMember(
                isStatic: true,
                Type(typeof(SequencePointTracer)),
                nameof(SequencePointTracer.TraceExpression),
                Variable("Host"),
                Member(Type(typeof(ConsoleColor)), dashColor.ToString(), isStatic: true),
                Type(astType),
                Variable(StackVariable),
                Constant(source.StartLineNumber),
                Constant(source.StartColumnNumber),
                String(source.Text),
                Variable("true"));

            return Pipeline(CommandExpression(traceInvocation));
        }

        // private StatementAst CreateProcessingMessage(IScriptExtent source, ConsoleColor dashColor)
        // {

        //     ExpressionAst startingMsg = String($"> {source.Text}");
        //     CommandAst command = Command(
        //         WriteHost,
        //         Add(CreateDashString(), startingMsg),
        //         CommandParameter("ForegroundColor"),
        //         String(dashColor.ToString()));

        //     return Pipeline(command);
        // }

        private ExpressionAst CreateDashString()
        {
            return Binary(String("-"), TokenKind.Multiply, Variable(StackVariable));
        }

        private StatementAst CreateEvaluation(StatementAst source)
        {
            return Assignment(
                Variable(TempVariable),
                source is CommandBaseAst command ? Pipeline(command) : source);
        }

        private StatementAst CreateEvaluation(ExpressionAst source)
        {
            return Assignment(Variable(TempVariable), CommandExpression(source));
        }

        private StatementAst CreateReturnValue()
        {
            return Pipeline(CommandExpression(ArrayLiteral(Variable(TempVariable))));
        }

        private StatementAst MaybeCreateStackVar()
        {
            var condition = Binary(Variable("null"), TokenKind.Ieq, Variable(StackVariable));

            var initializeVariable = Assignment(
                Variable(StackVariable),
                CommandExpression(Constant(0)));

            return If(condition, initializeVariable);
        }

        private StatementAst IncrementStack()
        {
            return Pipeline(
                CommandExpression(
                    Increment(Variable(StackVariable))));
        }

        private StatementAst CreateProcessedMessage(IScriptPosition position, ConsoleColor dashColor, Type astType)
        {
            ExpressionAst traceInvocation = InvokeMember(
                isStatic: true,
                Type(typeof(SequencePointTracer)),
                nameof(SequencePointTracer.TraceExpression),
                Variable("Host"),
                Member(Type(typeof(ConsoleColor)), dashColor.ToString(), isStatic: true),
                Type(astType),
                Variable(StackVariable),
                Constant(position.LineNumber),
                Constant(position.ColumnNumber),
                Index(
                    Array(
                        Pipeline(
                            CommandExpression(
                                ArrayLiteral(Variable(TempVariable))))),
                    Constant(0)),
                Variable("false"));

            return Pipeline(CommandExpression(traceInvocation));
            // var prefix = Add(CreateDashString(), String("< {0}"));
            // var message = Format(prefix, Variable(TempVariable));
            // return Pipeline(
            //     Command(
            //         WriteHost,
            //         message,
            //         CommandParameter("ForegroundColor"),
            //         String(dashColor.ToString())));
        }

        private StatementAst DecrementStack()
        {
            return Pipeline(
                CommandExpression(
                    Decrement(Variable(StackVariable))));
        }
    }
}
