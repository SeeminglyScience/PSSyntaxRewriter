using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using PSSyntaxRewriter;

namespace ClosureRewriter
{
    internal class WhereScriptBlockEnumerator : ScriptBlockEnumerator
    {
        internal WhereScriptBlockEnumerator(
            ScriptBlock scriptBlock,
            IEnumerator source,
            Closure closure)
            : base(scriptBlock, closure, source)
        {
        }
    }

    internal class ForEachScriptBlockEnumerator : ScriptBlockEnumerator
    {
        private ReadOnlyMemory<PSObject> _currentResult;

        private int _index = -1;

        internal ForEachScriptBlockEnumerator(
            ScriptBlock scriptBlock,
            IEnumerator source,
            Closure closure)
            : base(scriptBlock, closure, source)
        {
        }

        protected override bool IsTrue(ReadOnlySpan<PSObject> output) => true;

        public override PSObject Current
        {
            get
            {
                if (_currentResult.IsEmpty ||
                    _currentResult.Length == 0 ||
                    _currentResult.Length <= _index ||
                    _index < 0)
                {
                    return null;
                }

                return _currentResult.Span[_index];
            }
        }

        public override bool MoveNext()
        {
            while (true)
            {
                if (!(_currentResult.IsEmpty || _currentResult.Length == 0) &&
                    _index + 1 < _currentResult.Length)
                {
                    while (_index + 1 < _currentResult.Length)
                    {
                        _index++;
                        var current = _currentResult.Span[_index];
                        if (current == null || current == AutomationNull.Value)
                        {
                            continue;
                        }

                        return true;
                    }

                    _currentResult = default;
                    _index = -1;
                }

                if (!base.MoveNext())
                {
                    return false;
                }

                _currentResult = GetOutputMemory();
            }
        }
    }

    public abstract class ScriptBlockEnumerator : IEnumerator<PSObject>
    {
        private readonly object _pipe;

        private readonly ScriptBlock _scriptBlock;

        private readonly Closure _closure;

        private readonly SpanEnabledCollection<PSObject> _output;

        private readonly IEnumerator _source;

        private protected ScriptBlockEnumerator(
            ScriptBlock scriptBlock,
            Closure closure,
            IEnumerator source)
        {
            _pipe = Unstable.CreateNewPipe(_output = SpanEnabledCollection<PSObject>.Create());
            _scriptBlock = scriptBlock;
            _closure = closure;
            _source = source;
        }

        public virtual PSObject Current =>
            PSObject.AsPSObject(
                TransformCurrent(
                    _output.GetSpan(),
                    _source.Current));

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            DisposeImpl();
            (_source as IDisposable)?.Dispose();
        }

        protected virtual void DisposeImpl()
        {
        }

        protected ReadOnlySpan<PSObject> GetOutputSpan() => _output.GetSpan();

        protected ReadOnlyMemory<PSObject> GetOutputMemory() => _output.GetMemory();

        public virtual bool MoveNext()
        {
            while (_source.MoveNext())
            {
                _output.Clear();
                Invoke(dollarUnder: _source.Current);
                if (IsTrue(_output.GetSpan()))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual object TransformCurrent(
            ReadOnlySpan<PSObject> output,
            object current)
        {
            return current;
        }

        protected virtual object GetDollarUnder(object current) => current;

        protected virtual object[] GetArgs(object current) => null;

        protected virtual bool IsTrue(ReadOnlySpan<PSObject> output)
        {
            if (output.Length == 0)
            {
                return false;
            }

            if (output.Length == 1)
            {
                return LanguagePrimitives.IsTrue(output[0]);
            }

            return true;
        }

        private void Invoke(object dollarUnder = null)
        {
            var current = _source.Current;

            var thisClosure = _closure.With(
                dollarUnder: GetDollarUnder(current),
                args: GetArgs(current));

            Unstable.InvokeWithPipe(
                _scriptBlock,
                useLocalScope: false,
                AutomationNull.Value,
                AutomationNull.Value,
                AutomationNull.Value,
                _pipe,
                invocationInfo: null,
                propagateAllExceptionsToTop: true,
                variablesToDefine: null,
                functionsToDefine: null,
                new object[] { thisClosure });
        }

        public virtual void Reset()
        {
            _source.Reset();
        }
    }
}
