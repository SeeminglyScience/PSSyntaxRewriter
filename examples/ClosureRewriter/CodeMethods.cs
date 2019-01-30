using System.Collections;
using System.Management.Automation;

namespace ClosureRewriter
{
    public static class CodeMethods
    {
        public static IEnumerable NewWhere(PSObject instance, ScriptBlock scriptBlock)
        {
            IEnumerable enumerable = LanguagePrimitives.GetEnumerable(instance);
            if (enumerable == null)
            {
                enumerable = new[] { instance };
            }

            return new WhereScriptBlockEnumerable(scriptBlock, enumerable);
        }

        public static IEnumerable NewForEach(PSObject instance, ScriptBlock scriptBlock)
        {
            IEnumerable enumerable = LanguagePrimitives.GetEnumerable(instance);
            if (enumerable == null)
            {
                enumerable = new[] { instance };
            }

            return new ForEachScriptBlockEnumerable(scriptBlock, enumerable);
        }
    }
}
