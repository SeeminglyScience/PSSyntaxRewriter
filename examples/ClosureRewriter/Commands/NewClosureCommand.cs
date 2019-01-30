using System;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace ClosureRewriter.Commands
{
    [Cmdlet(VerbsCommon.New, "Closure")]
    [OutputType(typeof(ScriptBlock))]
    public class NewClosureCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNull]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter()]
        [ValidateNotNull]
        public Type DelegateType { get; set; }

        protected override void EndProcessing()
        {
            var closure = ClosureRewriter.CreateNewClosure(ScriptBlock, DelegateType);
            var sbAst = ScriptBlock.Ast as ScriptBlockAst;
            var e = sbAst.Extent;
            var invokeDelegateExpr = new InvokeMemberExpressionAst(
                e,
                new ConstantExpressionAst(e, closure),
                new StringConstantExpressionAst(e, nameof(Action.Invoke), StringConstantType.BareWord),
                Array.Empty<ExpressionAst>(),
                @static: false);

            var invokeDelegateStmt = new CommandExpressionAst(
                e,
                invokeDelegateExpr,
                Array.Empty<RedirectionAst>());

            var newSbAst = new ScriptBlockAst(
                e,
                sbAst.UsingStatements,
                Array.Empty<AttributeAst>(),
                null,
                null,
                null,
                new NamedBlockAst(
                    e,
                    TokenKind.End,
                    new StatementBlockAst(e, new[] { invokeDelegateStmt }, Array.Empty<TrapStatementAst>()),
                    unnamed: true),
                null);

            WriteObject(newSbAst.GetScriptBlock());
        }
    }
}
