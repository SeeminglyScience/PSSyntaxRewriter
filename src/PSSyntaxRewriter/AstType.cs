using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public static class AstType
    {
        public static readonly AstType<ErrorStatementAst> ErrorStatement = new AstType<ErrorStatementAst>();
        public static readonly AstType<ErrorExpressionAst> ErrorExpression = new AstType<ErrorExpressionAst>();
        public static readonly AstType<ScriptBlockAst> ScriptBlock = new AstType<ScriptBlockAst>();
        public static readonly AstType<ParamBlockAst> ParamBlock = new AstType<ParamBlockAst>();
        public static readonly AstType<NamedBlockAst> NamedBlock = new AstType<NamedBlockAst>();
        public static readonly AstType<NamedAttributeArgumentAst> NamedAttributeArgument = new AstType<NamedAttributeArgumentAst>();
        public static readonly AstType<AttributeBaseAst> AttributeBase = new AstType<AttributeBaseAst>();
        public static readonly AstType<AttributeAst> Attribute = new AstType<AttributeAst>();
        public static readonly AstType<TypeConstraintAst> TypeConstraint = new AstType<TypeConstraintAst>();
        public static readonly AstType<ParameterAst> Parameter = new AstType<ParameterAst>();
        public static readonly AstType<StatementBlockAst> StatementBlock = new AstType<StatementBlockAst>();
        public static readonly AstType<StatementAst> Statement = new AstType<StatementAst>();
        public static readonly AstType<TypeDefinitionAst> TypeDefinition = new AstType<TypeDefinitionAst>();
        public static readonly AstType<UsingStatementAst> UsingStatement = new AstType<UsingStatementAst>();
        public static readonly AstType<MemberAst> Member = new AstType<MemberAst>();
        public static readonly AstType<PropertyMemberAst> PropertyMember = new AstType<PropertyMemberAst>();
        public static readonly AstType<FunctionMemberAst> FunctionMember = new AstType<FunctionMemberAst>();
        public static readonly AstType<FunctionDefinitionAst> FunctionDefinition = new AstType<FunctionDefinitionAst>();
        public static readonly AstType<IfStatementAst> IfStatement = new AstType<IfStatementAst>();
        public static readonly AstType<DataStatementAst> DataStatement = new AstType<DataStatementAst>();
        public static readonly AstType<LabeledStatementAst> LabeledStatement = new AstType<LabeledStatementAst>();
        public static readonly AstType<LoopStatementAst> LoopStatement = new AstType<LoopStatementAst>();
        public static readonly AstType<ForEachStatementAst> ForEachStatement = new AstType<ForEachStatementAst>();
        public static readonly AstType<ForStatementAst> ForStatement = new AstType<ForStatementAst>();
        public static readonly AstType<DoWhileStatementAst> DoWhileStatement = new AstType<DoWhileStatementAst>();
        public static readonly AstType<DoUntilStatementAst> DoUntilStatement = new AstType<DoUntilStatementAst>();
        public static readonly AstType<WhileStatementAst> WhileStatement = new AstType<WhileStatementAst>();
        public static readonly AstType<SwitchStatementAst> SwitchStatement = new AstType<SwitchStatementAst>();
        public static readonly AstType<CatchClauseAst> CatchClause = new AstType<CatchClauseAst>();
        public static readonly AstType<TryStatementAst> TryStatement = new AstType<TryStatementAst>();
        public static readonly AstType<TrapStatementAst> TrapStatement = new AstType<TrapStatementAst>();
        public static readonly AstType<BreakStatementAst> BreakStatement = new AstType<BreakStatementAst>();
        public static readonly AstType<ContinueStatementAst> ContinueStatement = new AstType<ContinueStatementAst>();
        public static readonly AstType<ReturnStatementAst> ReturnStatement = new AstType<ReturnStatementAst>();
        public static readonly AstType<ExitStatementAst> ExitStatement = new AstType<ExitStatementAst>();
        public static readonly AstType<ThrowStatementAst> ThrowStatement = new AstType<ThrowStatementAst>();
        public static readonly AstType<PipelineBaseAst> PipelineBase = new AstType<PipelineBaseAst>();
        public static readonly AstType<PipelineAst> Pipeline = new AstType<PipelineAst>();
        public static readonly AstType<CommandElementAst> CommandElement = new AstType<CommandElementAst>();
        public static readonly AstType<CommandParameterAst> CommandParameter = new AstType<CommandParameterAst>();
        public static readonly AstType<CommandBaseAst> CommandBase = new AstType<CommandBaseAst>();
        public static readonly AstType<CommandAst> Command = new AstType<CommandAst>();
        public static readonly AstType<CommandExpressionAst> CommandExpression = new AstType<CommandExpressionAst>();
        public static readonly AstType<RedirectionAst> Redirection = new AstType<RedirectionAst>();
        public static readonly AstType<MergingRedirectionAst> MergingRedirection = new AstType<MergingRedirectionAst>();
        public static readonly AstType<FileRedirectionAst> FileRedirection = new AstType<FileRedirectionAst>();
        public static readonly AstType<AssignmentStatementAst> AssignmentStatement = new AstType<AssignmentStatementAst>();
        public static readonly AstType<ConfigurationDefinitionAst> ConfigurationDefinition = new AstType<ConfigurationDefinitionAst>();
        public static readonly AstType<DynamicKeywordStatementAst> DynamicKeywordStatement = new AstType<DynamicKeywordStatementAst>();
        public static readonly AstType<ExpressionAst> Expression = new AstType<ExpressionAst>();
        public static readonly AstType<BinaryExpressionAst> BinaryExpression = new AstType<BinaryExpressionAst>();
        public static readonly AstType<UnaryExpressionAst> UnaryExpression = new AstType<UnaryExpressionAst>();
        public static readonly AstType<BlockStatementAst> BlockStatement = new AstType<BlockStatementAst>();
        public static readonly AstType<AttributedExpressionAst> AttributedExpression = new AstType<AttributedExpressionAst>();
        public static readonly AstType<ConvertExpressionAst> ConvertExpression = new AstType<ConvertExpressionAst>();
        public static readonly AstType<MemberExpressionAst> MemberExpression = new AstType<MemberExpressionAst>();
        public static readonly AstType<InvokeMemberExpressionAst> InvokeMemberExpression = new AstType<InvokeMemberExpressionAst>();
        public static readonly AstType<BaseCtorInvokeMemberExpressionAst> BaseCtorInvokeMemberExpression = new AstType<BaseCtorInvokeMemberExpressionAst>();
        public static readonly AstType<TypeExpressionAst> TypeExpression = new AstType<TypeExpressionAst>();
        public static readonly AstType<VariableExpressionAst> VariableExpression = new AstType<VariableExpressionAst>();
        public static readonly AstType<ConstantExpressionAst> ConstantExpression = new AstType<ConstantExpressionAst>();
        public static readonly AstType<StringConstantExpressionAst> StringConstantExpression = new AstType<StringConstantExpressionAst>();
        public static readonly AstType<ExpandableStringExpressionAst> ExpandableStringExpression = new AstType<ExpandableStringExpressionAst>();
        public static readonly AstType<ScriptBlockExpressionAst> ScriptBlockExpression = new AstType<ScriptBlockExpressionAst>();
        public static readonly AstType<ArrayLiteralAst> ArrayLiteral = new AstType<ArrayLiteralAst>();
        public static readonly AstType<HashtableAst> Hashtable = new AstType<HashtableAst>();
        public static readonly AstType<ArrayExpressionAst> ArrayExpression = new AstType<ArrayExpressionAst>();
        public static readonly AstType<ParenExpressionAst> ParenExpression = new AstType<ParenExpressionAst>();
        public static readonly AstType<SubExpressionAst> SubExpression = new AstType<SubExpressionAst>();
        public static readonly AstType<UsingExpressionAst> UsingExpression = new AstType<UsingExpressionAst>();
        public static readonly AstType<IndexExpressionAst> IndexExpression = new AstType<IndexExpressionAst>();
    }

    public class AstType<TAst> where TAst : Ast
    {
        internal AstType()
        {
        }
    }
}
