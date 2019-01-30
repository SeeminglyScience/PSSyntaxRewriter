using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public static class Syntax
    {
        [ThreadStatic]
        internal static SyntaxFactory s_current;

        private static SyntaxFactory Current => s_current ?? SyntaxFactory.Default;

        public static MemberExpressionAst Member(ExpressionAst expression, string memberName, bool isStatic = false)
            => Current.Member(expression, memberName, isStatic);

        public static MemberExpressionAst Member(ExpressionAst expression, CommandElementAst member, bool isStatic = false)
            => Current.Member(expression, member, isStatic);

        public static TypeExpressionAst Type(Type type)
            => Current.Type(type);

        public static TypeExpressionAst Type(string typeName)
            => Current.Type(typeName);

        public static ArrayExpressionAst Array(params StatementAst[] statements)
            => Current.Array(statements);

        public static ArrayExpressionAst Array(IEnumerable<StatementAst> statements)
            => Current.Array(statements);

        public static IndexExpressionAst Index(ExpressionAst target, ExpressionAst index)
            => Current.Index(target, index);

        public static StatementBlockAst Block(IEnumerable<StatementAst> statements)
            => Current.Block(statements);

        public static StatementBlockAst Block(params StatementAst[] statements)
            => Current.Block(statements);

        public static IfStatementAst If(
            PipelineBaseAst condition,
            StatementBlockAst ifTrueBody,
            StatementBlockAst ifFalseBody)
            => Current.If(condition, ifTrueBody, ifFalseBody);

        public static IfStatementAst If(ExpressionAst condition, params StatementAst[] statements)
            => Current.If(condition, statements);

        public static UnaryExpressionAst Unary(ExpressionAst child, TokenKind operatorKind)
            => Current.Unary(child, operatorKind);

        public static IfStatementAst If(PipelineBaseAst condition, params StatementAst[] statements)
            => Current.If(condition, statements);

        public static ParenExpressionAst Paren(PipelineBaseAst pipeline) => Current.Paren(pipeline);

        public static AssignmentStatementAst Assignment(
            ExpressionAst left,
            StatementAst right,
            TokenKind operatorKind = TokenKind.Equals)
            => Current.Assignment(left, right, operatorKind);

        public static PipelineAst Pipeline(params CommandBaseAst[] commands) => Current.Pipeline(commands);

        public static VariableExpressionAst Variable(string variableName, bool isSplatted = false)
            => Current.Variable(variableName, isSplatted);

        public static ConstantExpressionAst Constant(object value) => Current.Constant(value);

        public static CommandAst Command(string commandName, params CommandElementAst[] elements)
            => Current.Command(commandName, elements);

        public static CommandAst Command(
            string commandName,
            TokenKind invocationOperator,
            params CommandElementAst[] elements)
            => Current.Command(commandName, invocationOperator, elements);

        public static CommandParameterAst CommandParameter(
            string parameterName,
            ExpressionAst argument = null)
            => Current.CommandParameter(parameterName, argument);

        public static StringConstantExpressionAst String(
            string value,
            StringConstantType stringConstantType = StringConstantType.BareWord)
            => Current.String(value, stringConstantType);

        public static BinaryExpressionAst Binary(
            ExpressionAst left,
            TokenKind @operator,
            ExpressionAst right,
            IScriptExtent errorPosition = null)
            => Current.Binary(left, @operator, right, errorPosition);

        public static BinaryExpressionAst Add(ExpressionAst left, ExpressionAst right)
            => Current.Add(left, right);

        public static BinaryExpressionAst Format(ExpressionAst format, ExpressionAst arg0)
            => Current.Format(format, arg0);

        public static BinaryExpressionAst Format(ExpressionAst format, params ExpressionAst[] args)
            => Current.Format(format, args);

        public static ArrayLiteralAst ArrayLiteral(params ExpressionAst[] items)
            => Current.ArrayLiteral(items);

        public static UnaryExpressionAst Decrement(ExpressionAst child)
            => Current.Decrement(child);

        public static UnaryExpressionAst Increment(ExpressionAst child)
            => Current.Increment(child);

        public static InvokeMemberExpressionAst InvokeMember(
            ExpressionAst expression,
            string methodName,
            params ExpressionAst[] arguments)
            => Current.InvokeMember(expression, methodName, arguments);
        public static InvokeMemberExpressionAst InvokeMember(
            ExpressionAst expression,
            CommandElementAst method,
            params ExpressionAst[] arguments)
            => Current.InvokeMember(expression, method, arguments);

        public static InvokeMemberExpressionAst InvokeMember(
            bool isStatic,
            ExpressionAst expression,
            CommandElementAst method,
            params ExpressionAst[] arguments)
            => Current.InvokeMember(isStatic, expression, method, arguments);

        public static InvokeMemberExpressionAst InvokeMember(
            bool isStatic,
            ExpressionAst expression,
            string methodName,
            params ExpressionAst[] arguments)
            => Current.InvokeMember(isStatic, expression, methodName, arguments);

        public static SubExpressionAst SubExpression(IEnumerable<StatementAst> statements)
            => Current.SubExpression(statements);

        public static SubExpressionAst SubExpression(params StatementAst[] statements)
            => Current.SubExpression(statements);

        public static CommandExpressionAst CommandExpression(ExpressionAst expression)
            => Current.CommandExpression(expression);

        public static class Variables
        {
            public static VariableExpressionAst True => Variable("true");

            public static VariableExpressionAst False => Variable("false");

            public static VariableExpressionAst Null => Variable("null");

            public static VariableExpressionAst Host => Variable("Host");

            public static VariableExpressionAst ExecutionContext => Variable("ExecutionContext");
        }
    }
}
