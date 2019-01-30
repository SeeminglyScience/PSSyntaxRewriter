using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace ClosureRewriter
{
    internal class WhereScriptBlockEnumerable : ScriptBlockEnumerable
    {
        public WhereScriptBlockEnumerable(ScriptBlock scriptBlock, IEnumerable enumerable)
            : base(scriptBlock, enumerable)
        {
        }

        public override IEnumerator<PSObject> GetEnumerator()
        {
            return new WhereScriptBlockEnumerator(
                _scriptBlock,
                _enumerable.GetEnumerator(),
                _closure);
        }
    }

    internal class ForEachScriptBlockEnumerable : ScriptBlockEnumerable
    {
        public ForEachScriptBlockEnumerable(ScriptBlock scriptBlock, IEnumerable enumerable)
            : base(scriptBlock, enumerable)
        {
        }

        public override IEnumerator<PSObject> GetEnumerator()
        {
            return new ForEachScriptBlockEnumerator(
                _scriptBlock,
                _enumerable.GetEnumerator(),
                _closure);
        }
    }

    internal abstract class ScriptBlockEnumerable : IEnumerable<PSObject>
    {
        private protected readonly ScriptBlock _scriptBlock;

        private protected readonly IEnumerable _enumerable;

        private protected readonly Closure _closure;

        public ScriptBlockEnumerable(ScriptBlock scriptBlock, IEnumerable enumerable)
        {
            _enumerable = enumerable;
            (_scriptBlock, _closure) = ClosureRewriter.CreateNewClosure(scriptBlock);
        }

        public abstract IEnumerator<PSObject> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // public IEnumerator GetEnumerator()
        // {
        //     return WhereScriptBlockEnumerator.Create(
        //         _scriptBlock,
        //         _closure,
        //         _enumerable.GetEnumerator());
        // }
    }
}
