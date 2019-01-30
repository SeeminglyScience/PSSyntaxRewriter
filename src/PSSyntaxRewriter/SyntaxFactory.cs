using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public class SyntaxFactory
    {
        public static readonly SyntaxFactory Default;

        private readonly Stack<Ast> _context = new Stack<Ast>();

        private IScriptExtent _currentExtent;

        static SyntaxFactory()
        {
            Default = new SyntaxFactory();
            Default.PushContext(Empty.Ast());
        }

        internal void PushContext(Ast newContext)
        {
            if (newContext == null)
            {
                throw new ArgumentNullException(nameof(newContext));
            }

            if (_context.Count == 0)
            {
                _context.Push(newContext);
                _currentExtent = newContext.Extent;
                return;
            }

            var currentExtent = _currentExtent;
            var newExtent = newContext.Extent;
            bool isSameAsPrevious =
                currentExtent.StartLineNumber == newExtent.StartLineNumber &&
                currentExtent.StartColumnNumber == newExtent.StartColumnNumber &&
                currentExtent.EndLineNumber == newExtent.EndLineNumber &&
                currentExtent.EndColumnNumber == newExtent.EndColumnNumber;

            if (isSameAsPrevious)
            {
                return;
            }

            _context.Push(newContext);
            _currentExtent = newContext.Extent;
        }

        internal Ast PopContext()
        {
            if (_context.Count < 2)
            {
                return null;
            }

            Ast popped = _context.Pop();
            _currentExtent = _context.Peek().Extent;
            return popped;
        }

        public MemberExpressionAst Member(
            ExpressionAst expression,
            string memberName,
            bool isStatic = false)
        {
            return Member(expression, String(memberName), isStatic);
        }

        public MemberExpressionAst Member(
            ExpressionAst expression,
            CommandElementAst member,
            bool isStatic = false)
        {
            return new MemberExpressionAst(
                _currentExtent,
                expression,
                member,
                isStatic);
        }

        public TypeExpressionAst Type(Type type)
        {
            return new TypeExpressionAst(
                _currentExtent,
                new ReflectionTypeName(type));
        }

        public TypeExpressionAst Type(string typeName)
        {
            return new TypeExpressionAst(
                _currentExtent,
                new TypeName(_currentExtent, typeName));
        }

        public ArrayExpressionAst Array(params StatementAst[] statements)
        {
            return Array((IEnumerable<StatementAst>)statements);
        }

        public ArrayExpressionAst Array(IEnumerable<StatementAst> statements)
        {
            return new ArrayExpressionAst(_currentExtent, Block(statements));
        }

        public IndexExpressionAst Index(ExpressionAst target, ExpressionAst index)
        {
            return new IndexExpressionAst(
                _currentExtent,
                target,
                index);
        }

        public StatementBlockAst Block(IEnumerable<StatementAst> statements)
        {
            return new StatementBlockAst(
                _currentExtent,
                statements,
                Enumerable.Empty<TrapStatementAst>());
        }

        public StatementBlockAst Block(params StatementAst[] statements)
        {
            return new StatementBlockAst(
                _currentExtent,
                statements,
                Enumerable.Empty<TrapStatementAst>());
        }

        public IfStatementAst If(
            PipelineBaseAst condition,
            StatementBlockAst ifTrueBody,
            StatementBlockAst ifFalseBody)
        {
            return new IfStatementAst(
                _currentExtent,
                new[] { Tuple.Create(condition, ifTrueBody) },
                ifFalseBody);
        }

        public IfStatementAst If(ExpressionAst condition, params StatementAst[] statements)
        {
            var clauses = new[]
            {
                Tuple.Create(
                    (PipelineBaseAst)Pipeline(CommandExpression(condition)),
                    Block(statements))
            };
            return new IfStatementAst(
                _currentExtent,
                clauses,
                null);
        }

        public UnaryExpressionAst Unary(ExpressionAst child, TokenKind operatorKind)
        {
            return new UnaryExpressionAst(_currentExtent, operatorKind, child);
        }

        public IfStatementAst If(PipelineBaseAst condition, params StatementAst[] statements)
        {
            return new IfStatementAst(
                _currentExtent,
                new[] { Tuple.Create(condition, Block(statements)) },
                null);
        }

        public ParenExpressionAst Paren(PipelineBaseAst pipeline)
        {
            return new ParenExpressionAst(
                _currentExtent,
                pipeline);
        }

        public AssignmentStatementAst Assignment(
            ExpressionAst left,
            StatementAst right,
            TokenKind operatorKind = TokenKind.Equals)
        {
            return new AssignmentStatementAst(
                _currentExtent,
                left,
                operatorKind,
                right,
                Empty.Extent(_currentExtent));
        }

        public PipelineAst Pipeline(params CommandBaseAst[] commands)
        {
            return new PipelineAst(_currentExtent, commands);
        }

        public VariableExpressionAst Variable(string variableName, bool isSplatted = false)
        {
            return new VariableExpressionAst(
                _currentExtent,
                variableName,
                isSplatted);
        }

        public ConstantExpressionAst Constant(object value)
        {
            return new ConstantExpressionAst(_currentExtent, value);
        }

        public CommandAst Command(string commandName, params CommandElementAst[] elements)
        {
            return Command(
                commandName,
                TokenKind.Ampersand,
                elements);
        }

        public CommandAst Command(string commandName, TokenKind invocationOperator, params CommandElementAst[] elements)
        {
            // var extent = GetExtentFromConstant(commandName, source.Extent);
            var newElements = new CommandElementAst[elements.Length + 1];
            for (int i = 0; i < elements.Length; i++)
            {
                newElements[i + 1] = elements[i];
            }

            // newElements[0] = source.As(StringConstantExpression, commandName);
            newElements[0] = String(commandName);
            return new CommandAst(
                _currentExtent,
                newElements,
                invocationOperator,
                Enumerable.Empty<RedirectionAst>());
        }

        public CommandParameterAst CommandParameter(string parameterName, ExpressionAst argument = null)
        {
            return new CommandParameterAst(
                _currentExtent,
                parameterName,
                argument,
                Empty.Extent(_currentExtent));
        }

        public StringConstantExpressionAst String(
            string value,
            StringConstantType stringConstantType = StringConstantType.BareWord)
        {
            return new StringConstantExpressionAst(
                _currentExtent,
                value,
                stringConstantType);
        }

        public BinaryExpressionAst Binary(
            ExpressionAst left,
            TokenKind @operator,
            ExpressionAst right,
            IScriptExtent errorPosition = null)
        {
            return new BinaryExpressionAst(
                _currentExtent,
                left,
                @operator,
                right,
                errorPosition ?? Empty.Extent(_currentExtent));
        }

        public BinaryExpressionAst Add(ExpressionAst left, ExpressionAst right)
        {
            return Binary(left, TokenKind.Plus, right);
        }

        public BinaryExpressionAst Format(ExpressionAst format, ExpressionAst arg0)
        {
            return Binary(format, TokenKind.Format, arg0);
        }

        public BinaryExpressionAst Format(ExpressionAst format, params ExpressionAst[] args)
        {
            return Binary(format, TokenKind.Format, ArrayLiteral(args));
        }

        public ArrayLiteralAst ArrayLiteral(params ExpressionAst[] items)
        {
            return new ArrayLiteralAst(_currentExtent, items);
        }


        public UnaryExpressionAst Decrement(ExpressionAst child)
        {
            return Unary(child, TokenKind.MinusMinus);
        }
        public UnaryExpressionAst Increment(ExpressionAst child)
        {
            return Unary(child, TokenKind.PlusPlus);
        }

        public InvokeMemberExpressionAst InvokeMember(
            ExpressionAst expression,
            string methodName,
            params ExpressionAst[] arguments)
        {
            return InvokeMember(isStatic: false, expression, String(methodName), arguments);
        }

        public InvokeMemberExpressionAst InvokeMember(
            ExpressionAst expression,
            CommandElementAst method,
            params ExpressionAst[] arguments)
        {
            return InvokeMember(isStatic: false, expression, method, arguments);
        }

        public InvokeMemberExpressionAst InvokeMember(
            bool isStatic,
            ExpressionAst expression,
            string methodName,
            params ExpressionAst[] arguments)
        {
            return InvokeMember(isStatic, expression, String(methodName), arguments);
        }

        public InvokeMemberExpressionAst InvokeMember(
            bool isStatic,
            ExpressionAst expression,
            CommandElementAst method,
            params ExpressionAst[] arguments)
        {
            return new InvokeMemberExpressionAst(
                _currentExtent,
                expression,
                method,
                arguments,
                isStatic);
        }

        public SubExpressionAst SubExpression(IEnumerable<StatementAst> statements)
        {
            return new SubExpressionAst(_currentExtent, Block(statements));
        }

        public SubExpressionAst SubExpression(params StatementAst[] statements)
        {
            return new SubExpressionAst(_currentExtent, Block(statements));
        }

        public CommandExpressionAst CommandExpression(ExpressionAst expression)
        {
            return new CommandExpressionAst(
                _currentExtent,
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
