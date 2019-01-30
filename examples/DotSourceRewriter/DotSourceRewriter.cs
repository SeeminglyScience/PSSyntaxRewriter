using System;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using PSSyntaxRewriter;

namespace DotSourceRewriter
{
    public class DotSourceRewriter : RewritingVisitor
    {
        private static readonly char[] s_invalidPathChars = Path.GetInvalidPathChars();
        public override StatementAst VisitCommand(CommandAst commandAst)
        {
            string commandName;
            if (commandAst.InvocationOperator != TokenKind.Dot ||
                commandAst.CommandElements.Count != 1)
            {
                return base.VisitCommand(commandAst);
            }

            if (!string.IsNullOrEmpty(commandName = commandAst.GetCommandName()))
            {
                if (IsScriptFile(commandName))
                {
                    var wasSuccessful = TryCreateScriptBlockExpression(
                        commandName,
                        commandAst.Extent,
                        out StatementAst result);

                    return wasSuccessful
                        ? result
                        : base.VisitCommand(commandAst);
                }
            }

            return base.VisitCommand(commandAst);
        }

        private bool TryCreateScriptBlockExpression(string fileName, IScriptExtent extent, out StatementAst result)
        {
            ScriptBlockAst ast;
            try
            {
                ast = Parser.ParseFile(fileName, out Token[] tokens, out ParseError[] errors);
            }
            catch
            {
                result = null;
                return false;
            }

            var sbExpression = new ScriptBlockExpressionAst(
                extent,
                ast.Rewrite(this));

            result =
                new CommandExpressionAst(
                    extent,
                    new SubExpressionAst(
                        extent,
                        new StatementBlockAst(
                            extent,
                            ast.EndBlock.Statements.RewriteAll(this),
                            Enumerable.Empty<TrapStatementAst>())),
                    Enumerable.Empty<RedirectionAst>());
            return true;
        }

        private bool IsScriptFile(string name)
        {
            if (name.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return false;
            }

            string fileName =
                name.IndexOf(Path.DirectorySeparatorChar) == -1
                    ? name
                    : Path.GetFileName(name);

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                return false;
            }

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }

            return extension.Equals(".ps1", StringComparison.OrdinalIgnoreCase);
        }
    }
}
