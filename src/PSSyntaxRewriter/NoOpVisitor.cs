using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public static class TypedReturnVisitorAstExtensions
    {
        public static TResult Accept<TResult>(this Ast ast, TypedReturnVisitor<TResult> visitor)
        {
            return (TResult)ast.Visit((ICustomAstVisitor)visitor);
        }
    }

    public class TypedReturnVisitor<TResult> : ICustomAstVisitor
    {
        public virtual TResult VisitArrayExpression(ArrayExpressionAst arrayExpressionAst) => default(TResult);
        public virtual TResult VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst) => default(TResult);
        public virtual TResult VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst) => default(TResult);
        public virtual TResult VisitAttribute(AttributeAst attributeAst) => default(TResult);
        public virtual TResult VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst) => default(TResult);
        public virtual TResult VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst) => default(TResult);
        public virtual TResult VisitBlockStatement(BlockStatementAst blockStatementAst) => default(TResult);
        public virtual TResult VisitBreakStatement(BreakStatementAst breakStatementAst) => default(TResult);
        public virtual TResult VisitCatchClause(CatchClauseAst catchClauseAst) => default(TResult);
        public virtual TResult VisitCommand(CommandAst commandAst) => default(TResult);
        public virtual TResult VisitCommandExpression(CommandExpressionAst commandExpressionAst) => default(TResult);
        public virtual TResult VisitCommandParameter(CommandParameterAst commandParameterAst) => default(TResult);
        public virtual TResult VisitConstantExpression(ConstantExpressionAst constantExpressionAst) => default(TResult);
        public virtual TResult VisitContinueStatement(ContinueStatementAst continueStatementAst) => default(TResult);
        public virtual TResult VisitConvertExpression(ConvertExpressionAst convertExpressionAst) => default(TResult);
        public virtual TResult VisitDataStatement(DataStatementAst dataStatementAst) => default(TResult);
        public virtual TResult VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst) => default(TResult);
        public virtual TResult VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst) => default(TResult);
        public virtual TResult VisitErrorExpression(ErrorExpressionAst errorExpressionAst) => default(TResult);
        public virtual TResult VisitErrorStatement(ErrorStatementAst errorStatementAst) => default(TResult);
        public virtual TResult VisitExitStatement(ExitStatementAst exitStatementAst) => default(TResult);
        public virtual TResult VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst) => default(TResult);
        public virtual TResult VisitFileRedirection(FileRedirectionAst fileRedirectionAst) => default(TResult);
        public virtual TResult VisitForEachStatement(ForEachStatementAst forEachStatementAst) => default(TResult);
        public virtual TResult VisitForStatement(ForStatementAst forStatementAst) => default(TResult);
        public virtual TResult VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst) => default(TResult);
        public virtual TResult VisitHashtable(HashtableAst hashtableAst) => default(TResult);
        public virtual TResult VisitIfStatement(IfStatementAst ifStmtAst) => default(TResult);
        public virtual TResult VisitIndexExpression(IndexExpressionAst indexExpressionAst) => default(TResult);
        public virtual TResult VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst) => default(TResult);
        public virtual TResult VisitMemberExpression(MemberExpressionAst memberExpressionAst) => default(TResult);
        public virtual TResult VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst) => default(TResult);
        public virtual TResult VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst) => default(TResult);
        public virtual TResult VisitNamedBlock(NamedBlockAst namedBlockAst) => default(TResult);
        public virtual TResult VisitParamBlock(ParamBlockAst paramBlockAst) => default(TResult);
        public virtual TResult VisitParameter(ParameterAst parameterAst) => default(TResult);
        public virtual TResult VisitParenExpression(ParenExpressionAst parenExpressionAst) => default(TResult);
        public virtual TResult VisitPipeline(PipelineAst pipelineAst) => default(TResult);
        public virtual TResult VisitReturnStatement(ReturnStatementAst returnStatementAst) => default(TResult);
        public virtual TResult VisitScriptBlock(ScriptBlockAst scriptBlockAst) => default(TResult);
        public virtual TResult VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst) => default(TResult);
        public virtual TResult VisitStatementBlock(StatementBlockAst statementBlockAst) => default(TResult);
        public virtual TResult VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst) => default(TResult);
        public virtual TResult VisitSubExpression(SubExpressionAst subExpressionAst) => default(TResult);
        public virtual TResult VisitSwitchStatement(SwitchStatementAst switchStatementAst) => default(TResult);
        public virtual TResult VisitThrowStatement(ThrowStatementAst throwStatementAst) => default(TResult);
        public virtual TResult VisitTrap(TrapStatementAst trapStatementAst) => default(TResult);
        public virtual TResult VisitTryStatement(TryStatementAst tryStatementAst) => default(TResult);
        public virtual TResult VisitTypeConstraint(TypeConstraintAst typeConstraintAst) => default(TResult);
        public virtual TResult VisitTypeExpression(TypeExpressionAst typeExpressionAst) => default(TResult);
        public virtual TResult VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst) => default(TResult);
        public virtual TResult VisitUsingExpression(UsingExpressionAst usingExpressionAst) => default(TResult);
        public virtual TResult VisitVariableExpression(VariableExpressionAst variableExpressionAst) => default(TResult);
        public virtual TResult VisitWhileStatement(WhileStatementAst whileStatementAst) => default(TResult);
        object ICustomAstVisitor.VisitArrayExpression(ArrayExpressionAst arrayExpressionAst) => VisitArrayExpression(arrayExpressionAst);
        object ICustomAstVisitor.VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst) => VisitArrayLiteral(arrayLiteralAst);
        object ICustomAstVisitor.VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst) => VisitAssignmentStatement(assignmentStatementAst);
        object ICustomAstVisitor.VisitAttribute(AttributeAst attributeAst) => VisitAttribute(attributeAst);
        object ICustomAstVisitor.VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst) => VisitAttributedExpression(attributedExpressionAst);
        object ICustomAstVisitor.VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst) => VisitBinaryExpression(binaryExpressionAst);
        object ICustomAstVisitor.VisitBlockStatement(BlockStatementAst blockStatementAst) => VisitBlockStatement(blockStatementAst);
        object ICustomAstVisitor.VisitBreakStatement(BreakStatementAst breakStatementAst) => VisitBreakStatement(breakStatementAst);
        object ICustomAstVisitor.VisitCatchClause(CatchClauseAst catchClauseAst) => VisitCatchClause(catchClauseAst);
        object ICustomAstVisitor.VisitCommand(CommandAst commandAst) => VisitCommand(commandAst);
        object ICustomAstVisitor.VisitCommandExpression(CommandExpressionAst commandExpressionAst) => VisitCommandExpression(commandExpressionAst);
        object ICustomAstVisitor.VisitCommandParameter(CommandParameterAst commandParameterAst) => VisitCommandParameter(commandParameterAst);
        object ICustomAstVisitor.VisitConstantExpression(ConstantExpressionAst constantExpressionAst) => VisitConstantExpression(constantExpressionAst);
        object ICustomAstVisitor.VisitContinueStatement(ContinueStatementAst continueStatementAst) => VisitContinueStatement(continueStatementAst);
        object ICustomAstVisitor.VisitConvertExpression(ConvertExpressionAst convertExpressionAst) => VisitConvertExpression(convertExpressionAst);
        object ICustomAstVisitor.VisitDataStatement(DataStatementAst dataStatementAst) => VisitDataStatement(dataStatementAst);
        object ICustomAstVisitor.VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst) => VisitDoUntilStatement(doUntilStatementAst);
        object ICustomAstVisitor.VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst) => VisitDoWhileStatement(doWhileStatementAst);
        object ICustomAstVisitor.VisitErrorExpression(ErrorExpressionAst errorExpressionAst) => VisitErrorExpression(errorExpressionAst);
        object ICustomAstVisitor.VisitErrorStatement(ErrorStatementAst errorStatementAst) => VisitErrorStatement(errorStatementAst);
        object ICustomAstVisitor.VisitExitStatement(ExitStatementAst exitStatementAst) => VisitExitStatement(exitStatementAst);
        object ICustomAstVisitor.VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst) => VisitExpandableStringExpression(expandableStringExpressionAst);
        object ICustomAstVisitor.VisitFileRedirection(FileRedirectionAst fileRedirectionAst) => VisitFileRedirection(fileRedirectionAst);
        object ICustomAstVisitor.VisitForEachStatement(ForEachStatementAst forEachStatementAst) => VisitForEachStatement(forEachStatementAst);
        object ICustomAstVisitor.VisitForStatement(ForStatementAst forStatementAst) => VisitForStatement(forStatementAst);
        object ICustomAstVisitor.VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst) => VisitFunctionDefinition(functionDefinitionAst);
        object ICustomAstVisitor.VisitHashtable(HashtableAst hashtableAst) => VisitHashtable(hashtableAst);
        object ICustomAstVisitor.VisitIfStatement(IfStatementAst ifStmtAst) => VisitIfStatement(ifStmtAst);
        object ICustomAstVisitor.VisitIndexExpression(IndexExpressionAst indexExpressionAst) => VisitIndexExpression(indexExpressionAst);
        object ICustomAstVisitor.VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst) => VisitInvokeMemberExpression(invokeMemberExpressionAst);
        object ICustomAstVisitor.VisitMemberExpression(MemberExpressionAst memberExpressionAst) => VisitMemberExpression(memberExpressionAst);
        object ICustomAstVisitor.VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst) => VisitMergingRedirection(mergingRedirectionAst);
        object ICustomAstVisitor.VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst) => VisitNamedAttributeArgument(namedAttributeArgumentAst);
        object ICustomAstVisitor.VisitNamedBlock(NamedBlockAst namedBlockAst) => VisitNamedBlock(namedBlockAst);
        object ICustomAstVisitor.VisitParamBlock(ParamBlockAst paramBlockAst) => VisitParamBlock(paramBlockAst);
        object ICustomAstVisitor.VisitParameter(ParameterAst parameterAst) => VisitParameter(parameterAst);
        object ICustomAstVisitor.VisitParenExpression(ParenExpressionAst parenExpressionAst) => VisitParenExpression(parenExpressionAst);
        object ICustomAstVisitor.VisitPipeline(PipelineAst pipelineAst) => VisitPipeline(pipelineAst);
        object ICustomAstVisitor.VisitReturnStatement(ReturnStatementAst returnStatementAst) => VisitReturnStatement(returnStatementAst);
        object ICustomAstVisitor.VisitScriptBlock(ScriptBlockAst scriptBlockAst) => VisitScriptBlock(scriptBlockAst);
        object ICustomAstVisitor.VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst) => VisitScriptBlockExpression(scriptBlockExpressionAst);
        object ICustomAstVisitor.VisitStatementBlock(StatementBlockAst statementBlockAst) => VisitStatementBlock(statementBlockAst);
        object ICustomAstVisitor.VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst) => VisitStringConstantExpression(stringConstantExpressionAst);
        object ICustomAstVisitor.VisitSubExpression(SubExpressionAst subExpressionAst) => VisitSubExpression(subExpressionAst);
        object ICustomAstVisitor.VisitSwitchStatement(SwitchStatementAst switchStatementAst) => VisitSwitchStatement(switchStatementAst);
        object ICustomAstVisitor.VisitThrowStatement(ThrowStatementAst throwStatementAst) => VisitThrowStatement(throwStatementAst);
        object ICustomAstVisitor.VisitTrap(TrapStatementAst trapStatementAst) => VisitTrap(trapStatementAst);
        object ICustomAstVisitor.VisitTryStatement(TryStatementAst tryStatementAst) => VisitTryStatement(tryStatementAst);
        object ICustomAstVisitor.VisitTypeConstraint(TypeConstraintAst typeConstraintAst) => VisitTypeConstraint(typeConstraintAst);
        object ICustomAstVisitor.VisitTypeExpression(TypeExpressionAst typeExpressionAst) => VisitTypeExpression(typeExpressionAst);
        object ICustomAstVisitor.VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst) => VisitUnaryExpression(unaryExpressionAst);
        object ICustomAstVisitor.VisitUsingExpression(UsingExpressionAst usingExpressionAst) => VisitUsingExpression(usingExpressionAst);
        object ICustomAstVisitor.VisitVariableExpression(VariableExpressionAst variableExpressionAst) => VisitVariableExpression(variableExpressionAst);
        object ICustomAstVisitor.VisitWhileStatement(WhileStatementAst whileStatementAst) => VisitWhileStatement(whileStatementAst);
    }
}
