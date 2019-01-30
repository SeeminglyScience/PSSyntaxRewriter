using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public abstract class SimpleSyntaxRewriter : RewritingVisitor
    {
        public abstract StatementAst VisitStatement(StatementAst statementAst);

        public abstract ExpressionAst VisitExpression(ExpressionAst expressionAst);

        public virtual TAst VisitOther<TAst>(TAst ast)
            where TAst : Ast
            => ast;

        public override ExpressionAst VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
            => VisitExpression(base.VisitArrayExpression(arrayExpressionAst));

        public override ExpressionAst VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
            => VisitExpression(base.VisitArrayLiteral(arrayLiteralAst));

        public override StatementAst VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
            => VisitStatement(base.VisitAssignmentStatement(assignmentStatementAst));

        public override AttributeBaseAst VisitAttribute(AttributeAst attributeAst)
            => VisitOther(base.VisitAttribute(attributeAst));

        public override ExpressionAst VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
            => VisitExpression(base.VisitAttributedExpression(attributedExpressionAst));

        public override ExpressionAst VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
            => VisitExpression(base.VisitBinaryExpression(binaryExpressionAst));

        public override StatementAst VisitBlockStatement(BlockStatementAst blockStatementAst)
            => VisitStatement(base.VisitBlockStatement(blockStatementAst));

        public override StatementAst VisitBreakStatement(BreakStatementAst breakStatementAst)
            => VisitStatement(base.VisitBreakStatement(breakStatementAst));

        public override StatementAst VisitCommand(CommandAst commandAst)
            => VisitStatement(base.VisitCommand(commandAst));

        public override StatementAst VisitCommandExpression(CommandExpressionAst commandExpressionAst)
            => VisitStatement(base.VisitCommandExpression(commandExpressionAst));

        public override CommandElementAst VisitCommandParameter(CommandParameterAst commandParameterAst)
            => VisitOther(base.VisitCommandParameter(commandParameterAst));

        public override ExpressionAst VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
            => VisitExpression(base.VisitConstantExpression(constantExpressionAst));

        public override StatementAst VisitContinueStatement(ContinueStatementAst continueStatementAst)
            => VisitStatement(base.VisitContinueStatement(continueStatementAst));

        public override ExpressionAst VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
            => VisitExpression(base.VisitConvertExpression(convertExpressionAst));

        public override StatementAst VisitDataStatement(DataStatementAst dataStatementAst)
            => VisitStatement(base.VisitDataStatement(dataStatementAst));

        public override StatementAst VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
            => VisitStatement(base.VisitDoUntilStatement(doUntilStatementAst));

        public override StatementAst VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
            => VisitStatement(base.VisitDoWhileStatement(doWhileStatementAst));

        public override ExpressionAst VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
            => VisitExpression(base.VisitErrorExpression(errorExpressionAst));

        public override StatementAst VisitErrorStatement(ErrorStatementAst errorStatementAst)
            => VisitStatement(base.VisitErrorStatement(errorStatementAst));

        public override StatementAst VisitExitStatement(ExitStatementAst exitStatementAst)
            => VisitStatement(base.VisitExitStatement(exitStatementAst));

        public override ExpressionAst VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
            => VisitExpression(base.VisitExpandableStringExpression(expandableStringExpressionAst));

        public override FileRedirectionAst VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
            => VisitOther(base.VisitFileRedirection(fileRedirectionAst));

        public override StatementAst VisitForEachStatement(ForEachStatementAst forEachStatementAst)
            => VisitStatement(base.VisitForEachStatement(forEachStatementAst));

        public override StatementAst VisitForStatement(ForStatementAst forStatementAst)
            => VisitStatement(base.VisitForStatement(forStatementAst));

        public override StatementAst VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
            => VisitStatement(base.VisitFunctionDefinition(functionDefinitionAst));

        public override ExpressionAst VisitHashtable(HashtableAst hashtableAst)
            => VisitExpression(base.VisitHashtable(hashtableAst));

        public override StatementAst VisitIfStatement(IfStatementAst ifStmtAst)
            => VisitStatement(base.VisitIfStatement(ifStmtAst));

        public override ExpressionAst VisitIndexExpression(IndexExpressionAst indexExpressionAst)
            => VisitExpression(base.VisitIndexExpression(indexExpressionAst));

        public override ExpressionAst VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
            => VisitExpression(base.VisitInvokeMemberExpression(invokeMemberExpressionAst));

        public override ExpressionAst VisitMemberExpression(MemberExpressionAst memberExpressionAst)
            => VisitExpression(base.VisitMemberExpression(memberExpressionAst));

        public override MergingRedirectionAst VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
            => VisitOther(base.VisitMergingRedirection(mergingRedirectionAst));

        public override NamedAttributeArgumentAst VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
            => VisitOther(base.VisitNamedAttributeArgument(namedAttributeArgumentAst));

        public override NamedBlockAst VisitNamedBlock(NamedBlockAst namedBlockAst)
            => VisitOther(base.VisitNamedBlock(namedBlockAst));

        public override ParamBlockAst VisitParamBlock(ParamBlockAst paramBlockAst)
            => VisitOther(base.VisitParamBlock(paramBlockAst));

        public override ParameterAst VisitParameter(ParameterAst parameterAst)
            => VisitOther(base.VisitParameter(parameterAst));

        public override ExpressionAst VisitParenExpression(ParenExpressionAst parenExpressionAst)
            => VisitExpression(base.VisitParenExpression(parenExpressionAst));

        public override StatementAst VisitPipeline(PipelineAst pipelineAst)
            => VisitStatement(base.VisitPipeline(pipelineAst));

        public override StatementAst VisitReturnStatement(ReturnStatementAst returnStatementAst)
            => VisitStatement(base.VisitReturnStatement(returnStatementAst));

        public override ScriptBlockAst VisitScriptBlock(ScriptBlockAst scriptBlockAst)
            => VisitOther(base.VisitScriptBlock(scriptBlockAst));

        public override ExpressionAst VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
            => VisitExpression(base.VisitScriptBlockExpression(scriptBlockExpressionAst));

        public override StatementBlockAst VisitStatementBlock(StatementBlockAst statementBlockAst)
            => VisitOther(base.VisitStatementBlock(statementBlockAst));

        public override ExpressionAst VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
            => VisitExpression(base.VisitStringConstantExpression(stringConstantExpressionAst));

        public override ExpressionAst VisitSubExpression(SubExpressionAst subExpressionAst)
            => VisitExpression(base.VisitSubExpression(subExpressionAst));

        public override StatementAst VisitSwitchStatement(SwitchStatementAst switchStatementAst)
            => VisitStatement(base.VisitSwitchStatement(switchStatementAst));

        public override StatementAst VisitThrowStatement(ThrowStatementAst throwStatementAst)
            => VisitStatement(base.VisitThrowStatement(throwStatementAst));

        public override StatementAst VisitTrap(TrapStatementAst trapStatementAst)
            => VisitStatement(base.VisitTrap(trapStatementAst));

        public override StatementAst VisitTryStatement(TryStatementAst tryStatementAst)
            => VisitStatement(base.VisitTryStatement(tryStatementAst));

        public override AttributeBaseAst VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
            => VisitOther(base.VisitTypeConstraint(typeConstraintAst));

        public override ExpressionAst VisitTypeExpression(TypeExpressionAst typeExpressionAst)
            => VisitExpression(base.VisitTypeExpression(typeExpressionAst));

        public override ExpressionAst VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
            => VisitExpression(base.VisitUnaryExpression(unaryExpressionAst));

        public override ExpressionAst VisitUsingExpression(UsingExpressionAst usingExpressionAst)
            => VisitExpression(base.VisitUsingExpression(usingExpressionAst));

        public override ExpressionAst VisitVariableExpression(VariableExpressionAst variableExpressionAst)
            => VisitExpression(base.VisitVariableExpression(variableExpressionAst));

        public override StatementAst VisitWhileStatement(WhileStatementAst whileStatementAst)
            => VisitStatement(base.VisitWhileStatement(whileStatementAst));

        public override BaseCtorInvokeMemberExpressionAst VisitBaseCtorInvokeMemberExpression(
            BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
            => VisitOther(base.VisitBaseCtorInvokeMemberExpression(baseCtorInvokeMemberExpressionAst));

        public override CatchClauseAst VisitCatchClause(CatchClauseAst catchClauseAst)
            => VisitOther(base.VisitCatchClause(catchClauseAst));

        public override StatementAst VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
            => VisitStatement(base.VisitConfigurationDefinition(configurationDefinitionAst));

        public override StatementAst VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst)
            => VisitStatement(base.VisitDynamicKeywordStatement(dynamicKeywordAst));

        public override MemberAst VisitFunctionMember(FunctionMemberAst functionMemberAst)
            => VisitOther(base.VisitFunctionMember(functionMemberAst));

        public override MemberAst VisitPropertyMember(PropertyMemberAst propertyMemberAst)
            => VisitOther(base.VisitPropertyMember(propertyMemberAst));

        public override StatementAst VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
            => VisitStatement(base.VisitTypeDefinition(typeDefinitionAst));

        public override UsingStatementAst VisitUsingStatement(UsingStatementAst usingStatement)
            => VisitOther(base.VisitUsingStatement(usingStatement));
    }
}
