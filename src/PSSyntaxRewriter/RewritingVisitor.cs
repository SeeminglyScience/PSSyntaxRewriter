using System;
using System.Linq;
using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public class RewritingVisitor : ICustomAstVisitor, ICustomAstVisitor2
    {
        public SyntaxKind CurrentExpectedKind { get; internal set; } = SyntaxKind.Any;

        internal SyntaxFactory Factory { get; } = new SyntaxFactory();

        public virtual ExpressionAst VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
        {
            return new ArrayExpressionAst(
                arrayExpressionAst.Extent,
                VisitStatementBlock(arrayExpressionAst.SubExpression));
        }

        public virtual ExpressionAst VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
        {
            return new ArrayLiteralAst(
                arrayLiteralAst.Extent,
                arrayLiteralAst.Elements.RewriteAll<ExpressionAst>(this, SyntaxKind.Expression));
        }

        public virtual StatementAst VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
        {
            return new AssignmentStatementAst(
                assignmentStatementAst.Extent,
                assignmentStatementAst.Left.Rewrite(this, SyntaxKind.Expression),
                assignmentStatementAst.Operator,
                assignmentStatementAst.Right.Rewrite(this, SyntaxKind.Statement),
                assignmentStatementAst.ErrorPosition);
        }

        public virtual AttributeBaseAst VisitAttribute(AttributeAst attributeAst)
        {
            return new AttributeAst(
                attributeAst.Extent,
                attributeAst.TypeName,
                attributeAst.PositionalArguments.RewriteAll<ExpressionAst>(this, SyntaxKind.Expression),
                attributeAst.NamedArguments.RewriteAll<NamedAttributeArgumentAst>(this, SyntaxKind.NamedAttributeArgument));
        }

        public virtual ExpressionAst VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
        {
            return new AttributedExpressionAst(
                attributedExpressionAst.Extent,
                attributedExpressionAst.Attribute.Rewrite(this, SyntaxKind.Attribute),
                attributedExpressionAst.Child.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual ExpressionAst VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
        {
            return new BinaryExpressionAst(
                binaryExpressionAst.Extent,
                binaryExpressionAst.Left.Rewrite(this, SyntaxKind.Expression),
                binaryExpressionAst.Operator,
                binaryExpressionAst.Right.Rewrite(this, SyntaxKind.Expression),
                binaryExpressionAst.ErrorPosition);
        }

        public virtual StatementAst VisitBlockStatement(BlockStatementAst blockStatementAst)
        {
            return new BlockStatementAst(
                blockStatementAst.Extent,
                blockStatementAst.Kind,
                blockStatementAst.Body.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitBreakStatement(BreakStatementAst breakStatementAst)
        {
            return new BreakStatementAst(
                breakStatementAst.Extent,
                breakStatementAst?.Label.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual CatchClauseAst VisitCatchClause(CatchClauseAst catchClauseAst)
        {
            return new CatchClauseAst(
                catchClauseAst.Extent,
                catchClauseAst.CatchTypes?.RewriteAll<TypeConstraintAst>(this, SyntaxKind.TypeConstraint),
                catchClauseAst.Body.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitCommand(CommandAst commandAst)
        {
            return new CommandAst(
                commandAst.Extent,
                commandAst.CommandElements.RewriteAll(this, SyntaxKind.CommandElement),
                commandAst.InvocationOperator,
                commandAst.Redirections?.RewriteAll<RedirectionAst>(this));
        }

        public virtual StatementAst VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            return new CommandExpressionAst(
                commandExpressionAst.Extent,
                commandExpressionAst.Expression.Rewrite(this),
                commandExpressionAst.Redirections?.RewriteAll<RedirectionAst>(this));
        }

        public virtual CommandElementAst VisitCommandParameter(CommandParameterAst commandParameterAst)
        {
            return new CommandParameterAst(
                commandParameterAst.Extent,
                commandParameterAst.ParameterName,
                commandParameterAst.Argument?.Rewrite(this),
                commandParameterAst.ErrorPosition);
        }

        public virtual ExpressionAst VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
        {
            return new ConstantExpressionAst(
                constantExpressionAst.Extent,
                constantExpressionAst.Value);
        }

        public virtual StatementAst VisitContinueStatement(ContinueStatementAst continueStatementAst)
        {
            return new ContinueStatementAst(
                continueStatementAst.Extent,
                continueStatementAst.Label?.Rewrite(this));
        }

        public virtual ExpressionAst VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
        {
            return new ConvertExpressionAst(
                convertExpressionAst.Extent,
                convertExpressionAst.Type.Rewrite(this),
                convertExpressionAst.Child.Rewrite(this));
        }

        public virtual StatementAst VisitDataStatement(DataStatementAst dataStatementAst)
        {
            return new DataStatementAst(
                dataStatementAst.Extent,
                dataStatementAst.Variable,
                dataStatementAst.CommandsAllowed?.RewriteAll<ExpressionAst>(this),
                dataStatementAst.Body.Rewrite(this));
        }

        public virtual StatementAst VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
        {
            return new DoUntilStatementAst(
                doUntilStatementAst.Extent,
                doUntilStatementAst.Label,
                doUntilStatementAst.Condition.Rewrite(this),
                doUntilStatementAst.Body.Rewrite(this));
        }

        public virtual StatementAst VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
        {
            return new DoWhileStatementAst(
                doWhileStatementAst.Extent,
                doWhileStatementAst.Label,
                doWhileStatementAst.Condition.Rewrite(this),
                doWhileStatementAst.Body.Rewrite(this));
        }

        public virtual ExpressionAst VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
        {
            return (ErrorExpressionAst)errorExpressionAst.Copy();
        }

        public virtual StatementAst VisitErrorStatement(ErrorStatementAst errorStatementAst)
        {
            return (ErrorStatementAst)errorStatementAst.Copy();
        }

        public virtual StatementAst VisitExitStatement(ExitStatementAst exitStatementAst)
        {
            return new ExitStatementAst(
                exitStatementAst.Extent,
                exitStatementAst.Pipeline?.Rewrite(this, SyntaxKind.Pipeline));
        }

        public virtual ExpressionAst VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
        {
            return new ExpandableStringExpressionAst(
                expandableStringExpressionAst.Extent,
                expandableStringExpressionAst.Value,
                expandableStringExpressionAst.StringConstantType);
        }

        public virtual FileRedirectionAst VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
        {
            return new FileRedirectionAst(
                fileRedirectionAst.Extent,
                fileRedirectionAst.FromStream,
                fileRedirectionAst.Location?.Rewrite(this),
                fileRedirectionAst.Append);
        }

        public virtual StatementAst VisitForEachStatement(ForEachStatementAst forEachStatementAst)
        {
            return new ForEachStatementAst(
                forEachStatementAst.Extent,
                forEachStatementAst.Label,
                forEachStatementAst.Flags,
                forEachStatementAst.Variable.Rewrite(this, SyntaxKind.Variable),
                forEachStatementAst.Condition.Rewrite(this, SyntaxKind.Pipeline),
                forEachStatementAst.Body.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitForStatement(ForStatementAst forStatementAst)
        {
            return new ForStatementAst(
                forStatementAst.Extent,
                forStatementAst.Label,
                forStatementAst.Initializer.Rewrite(this, SyntaxKind.Pipeline),
                forStatementAst.Condition.Rewrite(this, SyntaxKind.Pipeline),
                forStatementAst.Iterator.Rewrite(this, SyntaxKind.Pipeline),
                forStatementAst.Body.Rewrite(this, SyntaxKind.Pipeline));
        }

        public virtual StatementAst VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
        {
            return new FunctionDefinitionAst(
                functionDefinitionAst.Extent,
                functionDefinitionAst.IsFilter,
                functionDefinitionAst.IsWorkflow,
                functionDefinitionAst.Name,
                functionDefinitionAst.Parameters?.RewriteAll(this),
                functionDefinitionAst.Body?.Rewrite(this));
        }

        public virtual ExpressionAst VisitHashtable(HashtableAst hashtableAst)
        {
            return new HashtableAst(
                hashtableAst.Extent,
                hashtableAst.KeyValuePairs?.RewriteAllTuples(this));
        }

        public virtual StatementAst VisitIfStatement(IfStatementAst ifStmtAst)
        {
            return new IfStatementAst(
                ifStmtAst.Extent,
                ifStmtAst.Clauses?.RewriteAllTuples(this, SyntaxKind.Pipeline, SyntaxKind.StatementBlock),
                ifStmtAst.ElseClause?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual ExpressionAst VisitIndexExpression(IndexExpressionAst indexExpressionAst)
        {
            return new IndexExpressionAst(
                indexExpressionAst.Extent,
                indexExpressionAst.Target.Rewrite(this),
                indexExpressionAst.Index.Rewrite(this));
        }

        public virtual ExpressionAst VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
        {
            return new InvokeMemberExpressionAst(
                invokeMemberExpressionAst.Extent,
                invokeMemberExpressionAst.Expression.Rewrite(this),
                invokeMemberExpressionAst.Member.Rewrite(this),
                invokeMemberExpressionAst.Arguments?.RewriteAll(this),
                invokeMemberExpressionAst.Static);
        }

        public virtual ExpressionAst VisitMemberExpression(MemberExpressionAst memberExpressionAst)
        {
            return new MemberExpressionAst(
                memberExpressionAst.Extent,
                memberExpressionAst.Expression.Rewrite(this),
                memberExpressionAst.Member.Rewrite(this),
                memberExpressionAst.Static);
        }

        public virtual MergingRedirectionAst VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
        {
            return new MergingRedirectionAst(
                mergingRedirectionAst.Extent,
                mergingRedirectionAst.FromStream,
                mergingRedirectionAst.ToStream);
        }

        public virtual NamedAttributeArgumentAst VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
        {
            return new NamedAttributeArgumentAst(
                namedAttributeArgumentAst.Extent,
                namedAttributeArgumentAst.ArgumentName,
                namedAttributeArgumentAst.Argument?.Rewrite(this),
                namedAttributeArgumentAst.ExpressionOmitted);
        }

        public virtual NamedBlockAst VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            return new NamedBlockAst(
                namedBlockAst.Extent,
                namedBlockAst.BlockKind,
                new StatementBlockAst(
                    namedBlockAst.Extent,
                    namedBlockAst.Statements.RewriteAll(this),
                    namedBlockAst.Traps.RewriteAll(this)),
                namedBlockAst.Unnamed);
        }

        public virtual ParamBlockAst VisitParamBlock(ParamBlockAst paramBlockAst)
        {
            return new ParamBlockAst(
                paramBlockAst.Extent,
                paramBlockAst.Attributes.RewriteAll(this, SyntaxKind.Attribute),
                paramBlockAst.Parameters.RewriteAll(this));
        }

        public virtual ParameterAst VisitParameter(ParameterAst parameterAst)
        {
            return new ParameterAst(
                parameterAst.Extent,
                parameterAst.Name.Rewrite(this, SyntaxKind.Variable),
                parameterAst.Attributes.RewriteAll(this, SyntaxKind.Attribute),
                parameterAst.DefaultValue?.Rewrite(this));
        }

        public virtual ExpressionAst VisitParenExpression(ParenExpressionAst parenExpressionAst)
        {
            return new ParenExpressionAst(
                parenExpressionAst.Extent,
                parenExpressionAst.Pipeline?.Rewrite(this, SyntaxKind.Pipeline));
        }

        public virtual StatementAst VisitPipeline(PipelineAst pipelineAst)
        {
            return new PipelineAst(
                pipelineAst.Extent,
                pipelineAst.PipelineElements.RewriteAll(this, SyntaxKind.Command));
        }

        public virtual StatementAst VisitReturnStatement(ReturnStatementAst returnStatementAst)
        {
            return new ReturnStatementAst(
                returnStatementAst.Extent,
                returnStatementAst.Pipeline?.Rewrite(this, SyntaxKind.Pipeline));
        }

        public virtual ScriptBlockAst VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            var attributes = scriptBlockAst.Attributes.Count == 0
                ? null
                : scriptBlockAst.Attributes.RewriteAll(this, SyntaxKind.Attribute);

            return new ScriptBlockAst(
                scriptBlockAst.Extent,
                scriptBlockAst.UsingStatements?.RewriteAll(this, SyntaxKind.UsingStatement),
                attributes,
                scriptBlockAst.ParamBlock?.Rewrite(this, SyntaxKind.ParamBlock),
                scriptBlockAst.BeginBlock?.Rewrite(this, SyntaxKind.NamedBlock),
                scriptBlockAst.ProcessBlock?.Rewrite(this, SyntaxKind.NamedBlock),
                scriptBlockAst.EndBlock?.Rewrite(this, SyntaxKind.NamedBlock),
                scriptBlockAst.DynamicParamBlock?.Rewrite(this, SyntaxKind.NamedBlock));
        }

        public virtual ExpressionAst VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
        {
            return new ScriptBlockExpressionAst(
                scriptBlockExpressionAst.Extent,
                scriptBlockExpressionAst.ScriptBlock.Rewrite(this, SyntaxKind.ScriptBlock));
        }

        public virtual StatementBlockAst VisitStatementBlock(StatementBlockAst statementBlockAst)
        {
            return new StatementBlockAst(
                statementBlockAst.Extent,
                statementBlockAst.Statements?.RewriteAll(this, SyntaxKind.Statement),
                statementBlockAst.Traps?.RewriteAll(this, SyntaxKind.Trap));
        }

        public virtual ExpressionAst VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
        {
            return new StringConstantExpressionAst(
                stringConstantExpressionAst.Extent,
                stringConstantExpressionAst.Value,
                stringConstantExpressionAst.StringConstantType);
        }

        public virtual ExpressionAst VisitSubExpression(SubExpressionAst subExpressionAst)
        {
            return new SubExpressionAst(
                subExpressionAst.Extent,
                subExpressionAst.SubExpression?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitSwitchStatement(SwitchStatementAst switchStatementAst)
        {
            return new SwitchStatementAst(
                switchStatementAst.Extent,
                switchStatementAst.Label,
                switchStatementAst.Condition?.Rewrite(this, SyntaxKind.Pipeline),
                switchStatementAst.Flags,
                switchStatementAst.Clauses?.RewriteAllTuples(this, SyntaxKind.Expression, SyntaxKind.StatementBlock),
                switchStatementAst.Default?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitThrowStatement(ThrowStatementAst throwStatementAst)
        {
            return new ThrowStatementAst(
                throwStatementAst.Extent,
                throwStatementAst.Pipeline?.Rewrite(this, SyntaxKind.Pipeline));
        }

        public virtual StatementAst VisitTrap(TrapStatementAst trapStatementAst)
        {
            return new TrapStatementAst(
                trapStatementAst.Extent,
                trapStatementAst.TrapType?.Rewrite(this, SyntaxKind.TypeConstraint),
                trapStatementAst.Body?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual StatementAst VisitTryStatement(TryStatementAst tryStatementAst)
        {
            return new TryStatementAst(
                tryStatementAst.Extent,
                tryStatementAst.Body?.Rewrite(this, SyntaxKind.StatementBlock),
                tryStatementAst.CatchClauses?.RewriteAll(this, SyntaxKind.Catch),
                tryStatementAst.Finally?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual AttributeBaseAst VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
        {
            return new TypeConstraintAst(
                typeConstraintAst.Extent,
                typeConstraintAst.TypeName);
        }

        public virtual ExpressionAst VisitTypeExpression(TypeExpressionAst typeExpressionAst)
        {
            return new TypeExpressionAst(
                typeExpressionAst.Extent,
                typeExpressionAst.TypeName);
        }

        public virtual ExpressionAst VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
        {
            return new UnaryExpressionAst(
                unaryExpressionAst.Extent,
                unaryExpressionAst.TokenKind,
                unaryExpressionAst.Child.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual ExpressionAst VisitUsingExpression(UsingExpressionAst usingExpressionAst)
        {
            return new UsingExpressionAst(
                usingExpressionAst.Extent,
                usingExpressionAst.SubExpression.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual ExpressionAst VisitVariableExpression(VariableExpressionAst variableExpressionAst)
        {
            return new VariableExpressionAst(
                variableExpressionAst.Extent,
                variableExpressionAst.VariablePath,
                variableExpressionAst.Splatted);
        }

        public virtual StatementAst VisitWhileStatement(WhileStatementAst whileStatementAst)
        {
            return new WhileStatementAst(
                whileStatementAst.Extent,
                whileStatementAst.Label,
                whileStatementAst.Condition?.Rewrite(this, SyntaxKind.Pipeline),
                whileStatementAst.Body?.Rewrite(this, SyntaxKind.StatementBlock));
        }

        public virtual BaseCtorInvokeMemberExpressionAst VisitBaseCtorInvokeMemberExpression(
            BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
        {
            return new BaseCtorInvokeMemberExpressionAst(
                baseCtorInvokeMemberExpressionAst.Extent,
                baseCtorInvokeMemberExpressionAst.Expression.Extent,
                baseCtorInvokeMemberExpressionAst.Arguments?.RewriteAll(this, SyntaxKind.Expression));
        }

        public virtual StatementAst VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
        {
            return new ConfigurationDefinitionAst(
                configurationDefinitionAst.Extent,
                configurationDefinitionAst.Body.Rewrite(this, SyntaxKind.ScriptBlockExpression),
                configurationDefinitionAst.ConfigurationType,
                configurationDefinitionAst.InstanceName?.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual StatementAst VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst)
        {
            return new DynamicKeywordStatementAst(
                dynamicKeywordAst.Extent,
                dynamicKeywordAst.CommandElements?.RewriteAll(this, SyntaxKind.CommandElement));
        }

        public virtual MemberAst VisitFunctionMember(FunctionMemberAst functionMemberAst)
        {
            return new FunctionMemberAst(
                functionMemberAst.Extent,
                new FunctionDefinitionAst(
                    functionMemberAst.Extent,
                    false,
                    false,
                    functionMemberAst.Name,
                    functionMemberAst.Parameters?.RewriteAll(this, SyntaxKind.Parameter),
                    functionMemberAst.Body?.Rewrite(this, SyntaxKind.ScriptBlock)),
                functionMemberAst.ReturnType.Rewrite(this, SyntaxKind.TypeConstraint),
                functionMemberAst.Attributes?.RewriteAll(this),
                functionMemberAst.MethodAttributes);
        }

        public virtual MemberAst VisitPropertyMember(PropertyMemberAst propertyMemberAst)
        {
            return new PropertyMemberAst(
                propertyMemberAst.Extent,
                propertyMemberAst.Name,
                propertyMemberAst.PropertyType?.Rewrite(this, SyntaxKind.TypeConstraint),
                propertyMemberAst.Attributes?.RewriteAll(this, SyntaxKind.Attribute),
                propertyMemberAst.PropertyAttributes,
                propertyMemberAst.InitialValue?.Rewrite(this, SyntaxKind.Expression));
        }

        public virtual StatementAst VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
        {
            return new TypeDefinitionAst(
                typeDefinitionAst.Extent,
                typeDefinitionAst.Name,
                typeDefinitionAst.Attributes?.RewriteAll(this, SyntaxKind.Attribute),
                typeDefinitionAst.Members?.RewriteAll(this, SyntaxKind.Member),
                typeDefinitionAst.TypeAttributes,
                typeDefinitionAst.BaseTypes?.RewriteAll(this, SyntaxKind.TypeConstraint));
        }

        public virtual UsingStatementAst VisitUsingStatement(UsingStatementAst usingStatement)
        {
            return new UsingStatementAst(
                usingStatement.Extent,
                usingStatement.UsingStatementKind,
                usingStatement.Name?.Rewrite(this, SyntaxKind.StringConstant));
        }

        object ICustomAstVisitor.VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
            => ProcessRewriter(VisitArrayLiteral, arrayLiteralAst);

        object ICustomAstVisitor.VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
            => ProcessRewriter(VisitArrayExpression, arrayExpressionAst);

        object ICustomAstVisitor.VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
            => ProcessRewriter(VisitAssignmentStatement, assignmentStatementAst);

        object ICustomAstVisitor.VisitAttribute(AttributeAst attributeAst)
            => ProcessRewriter(VisitAttribute, attributeAst);

        object ICustomAstVisitor.VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
            => ProcessRewriter(VisitAttributedExpression, attributedExpressionAst);

        object ICustomAstVisitor.VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
            => ProcessRewriter(VisitBinaryExpression, binaryExpressionAst);

        object ICustomAstVisitor.VisitBlockStatement(BlockStatementAst blockStatementAst)
            => ProcessRewriter(VisitBlockStatement, blockStatementAst);

        object ICustomAstVisitor.VisitBreakStatement(BreakStatementAst breakStatementAst)
            => ProcessRewriter(VisitBreakStatement, breakStatementAst);

        object ICustomAstVisitor.VisitCatchClause(CatchClauseAst catchClauseAst)
            => ProcessRewriter(VisitCatchClause, catchClauseAst);

        object ICustomAstVisitor.VisitCommand(CommandAst commandAst)
            => ProcessRewriter(VisitCommand, commandAst);

        object ICustomAstVisitor.VisitCommandExpression(CommandExpressionAst commandExpressionAst)
            => ProcessRewriter(VisitCommandExpression, commandExpressionAst);

        object ICustomAstVisitor.VisitCommandParameter(CommandParameterAst commandParameterAst)
            => ProcessRewriter(VisitCommandParameter, commandParameterAst);

        object ICustomAstVisitor.VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
            => ProcessRewriter(VisitConstantExpression, constantExpressionAst);

        object ICustomAstVisitor.VisitContinueStatement(ContinueStatementAst continueStatementAst)
            => ProcessRewriter(VisitContinueStatement, continueStatementAst);

        object ICustomAstVisitor.VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
            => ProcessRewriter(VisitConvertExpression, convertExpressionAst);

        object ICustomAstVisitor.VisitDataStatement(DataStatementAst dataStatementAst)
            => ProcessRewriter(VisitDataStatement, dataStatementAst);

        object ICustomAstVisitor.VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
            => ProcessRewriter(VisitDoUntilStatement, doUntilStatementAst);

        object ICustomAstVisitor.VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
            => ProcessRewriter(VisitDoWhileStatement, doWhileStatementAst);

        object ICustomAstVisitor.VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
            => ProcessRewriter(VisitErrorExpression, errorExpressionAst);

        object ICustomAstVisitor.VisitErrorStatement(ErrorStatementAst errorStatementAst)
            => ProcessRewriter(VisitErrorStatement, errorStatementAst);

        object ICustomAstVisitor.VisitExitStatement(ExitStatementAst exitStatementAst)
            => ProcessRewriter(VisitExitStatement, exitStatementAst);

        object ICustomAstVisitor.VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
            => ProcessRewriter(VisitExpandableStringExpression, expandableStringExpressionAst);

        object ICustomAstVisitor.VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
            => ProcessRewriter(VisitFileRedirection, fileRedirectionAst);

        object ICustomAstVisitor.VisitForEachStatement(ForEachStatementAst forEachStatementAst)
            => ProcessRewriter(VisitForEachStatement, forEachStatementAst);

        object ICustomAstVisitor.VisitForStatement(ForStatementAst forStatementAst)
            => ProcessRewriter(VisitForStatement, forStatementAst);

        object ICustomAstVisitor.VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
            => ProcessRewriter(VisitFunctionDefinition, functionDefinitionAst);

        object ICustomAstVisitor.VisitHashtable(HashtableAst hashtableAst)
            => ProcessRewriter(VisitHashtable, hashtableAst);

        object ICustomAstVisitor.VisitIfStatement(IfStatementAst ifStmtAst)
            => ProcessRewriter(VisitIfStatement, ifStmtAst);

        object ICustomAstVisitor.VisitIndexExpression(IndexExpressionAst indexExpressionAst)
            => ProcessRewriter(VisitIndexExpression, indexExpressionAst);

        object ICustomAstVisitor.VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
            => ProcessRewriter(VisitInvokeMemberExpression, invokeMemberExpressionAst);

        object ICustomAstVisitor.VisitMemberExpression(MemberExpressionAst memberExpressionAst)
            => ProcessRewriter(VisitMemberExpression, memberExpressionAst);

        object ICustomAstVisitor.VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
            => ProcessRewriter(VisitMergingRedirection, mergingRedirectionAst);

        object ICustomAstVisitor.VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
            => ProcessRewriter(VisitNamedAttributeArgument, namedAttributeArgumentAst);

        object ICustomAstVisitor.VisitNamedBlock(NamedBlockAst namedBlockAst)
            => ProcessRewriter(VisitNamedBlock, namedBlockAst);

        object ICustomAstVisitor.VisitParamBlock(ParamBlockAst paramBlockAst)
            => ProcessRewriter(VisitParamBlock, paramBlockAst);

        object ICustomAstVisitor.VisitParameter(ParameterAst parameterAst)
            => ProcessRewriter(VisitParameter, parameterAst);

        object ICustomAstVisitor.VisitParenExpression(ParenExpressionAst parenExpressionAst)
            => ProcessRewriter(VisitParenExpression, parenExpressionAst);

        object ICustomAstVisitor.VisitPipeline(PipelineAst pipelineAst)
            => ProcessRewriter(VisitPipeline, pipelineAst);

        object ICustomAstVisitor.VisitReturnStatement(ReturnStatementAst returnStatementAst)
            => ProcessRewriter(VisitReturnStatement, returnStatementAst);

        object ICustomAstVisitor.VisitScriptBlock(ScriptBlockAst scriptBlockAst)
            => ProcessRewriter(VisitScriptBlock, scriptBlockAst);

        object ICustomAstVisitor.VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
            => ProcessRewriter(VisitScriptBlockExpression, scriptBlockExpressionAst);

        object ICustomAstVisitor.VisitStatementBlock(StatementBlockAst statementBlockAst)
            => ProcessRewriter(VisitStatementBlock, statementBlockAst);

        object ICustomAstVisitor.VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
            => ProcessRewriter(VisitStringConstantExpression, stringConstantExpressionAst);

        object ICustomAstVisitor.VisitSubExpression(SubExpressionAst subExpressionAst)
            => ProcessRewriter(VisitSubExpression, subExpressionAst);

        object ICustomAstVisitor.VisitSwitchStatement(SwitchStatementAst switchStatementAst)
            => ProcessRewriter(VisitSwitchStatement, switchStatementAst);

        object ICustomAstVisitor.VisitThrowStatement(ThrowStatementAst throwStatementAst)
            => ProcessRewriter(VisitThrowStatement, throwStatementAst);

        object ICustomAstVisitor.VisitTrap(TrapStatementAst trapStatementAst)
            => ProcessRewriter(VisitTrap, trapStatementAst);

        object ICustomAstVisitor.VisitTryStatement(TryStatementAst tryStatementAst)
            => ProcessRewriter(VisitTryStatement, tryStatementAst);

        object ICustomAstVisitor.VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
            => ProcessRewriter(VisitTypeConstraint, typeConstraintAst);

        object ICustomAstVisitor.VisitTypeExpression(TypeExpressionAst typeExpressionAst)
            => ProcessRewriter(VisitTypeExpression, typeExpressionAst);

        object ICustomAstVisitor.VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
            => ProcessRewriter(VisitUnaryExpression, unaryExpressionAst);

        object ICustomAstVisitor.VisitUsingExpression(UsingExpressionAst usingExpressionAst)
            => ProcessRewriter(VisitUsingExpression, usingExpressionAst);

        object ICustomAstVisitor.VisitVariableExpression(VariableExpressionAst variableExpressionAst)
            => ProcessRewriter(VisitVariableExpression, variableExpressionAst);

        object ICustomAstVisitor.VisitWhileStatement(WhileStatementAst whileStatementAst)
            => ProcessRewriter(VisitWhileStatement, whileStatementAst);

        object ICustomAstVisitor2.VisitBaseCtorInvokeMemberExpression(BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst)
            => ProcessRewriter(VisitBaseCtorInvokeMemberExpression, baseCtorInvokeMemberExpressionAst);

        object ICustomAstVisitor2.VisitConfigurationDefinition(ConfigurationDefinitionAst configurationDefinitionAst)
            => ProcessRewriter(VisitConfigurationDefinition, configurationDefinitionAst);

        object ICustomAstVisitor2.VisitDynamicKeywordStatement(DynamicKeywordStatementAst dynamicKeywordAst)
            => ProcessRewriter(VisitDynamicKeywordStatement, dynamicKeywordAst);

        object ICustomAstVisitor2.VisitFunctionMember(FunctionMemberAst functionMemberAst)
            => ProcessRewriter(VisitFunctionMember, functionMemberAst);

        object ICustomAstVisitor2.VisitPropertyMember(PropertyMemberAst propertyMemberAst)
            => ProcessRewriter(VisitPropertyMember, propertyMemberAst);

        object ICustomAstVisitor2.VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
            => ProcessRewriter(VisitTypeDefinition, typeDefinitionAst);

        object ICustomAstVisitor2.VisitUsingStatement(UsingStatementAst usingStatement)
            => ProcessRewriter(VisitUsingStatement, usingStatement);

        private TResultAst ProcessRewriter<TAst, TResultAst>(Func<TAst, TResultAst> visitor, TAst subject)
            where TAst : Ast
            where TResultAst : Ast
        {
            var oldTlsFactory = Syntax.s_current;
            Factory.PushContext(subject);
            try
            {
                Syntax.s_current = Factory;
                return visitor(subject);
            }
            finally
            {
                Syntax.s_current = oldTlsFactory;
                Factory.PopContext();
            }
        }
    }
}
