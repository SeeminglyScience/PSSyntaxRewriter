using System;
using System.Collections.Concurrent;
using System.Management.Automation.Language;

namespace PSSyntaxRewriter
{
    public static class Empty
    {
        public static Ast Ast(IScriptExtent extent = null)
        {
            extent = Extent(extent);
            return FileBasedCache<Ast>.Instance.GetOrAdd(
                extent?.File ?? string.Empty,
                file => new StringConstantExpressionAst(
                    extent,
                    string.Empty,
                    StringConstantType.BareWord));
        }

        public static IScriptPosition Position (IScriptPosition position = null)
        {
            return PositionImpl(position?.File, position?.GetFullScript());
        }

        public static IScriptPosition Position(string fileName, string fullScript)
        {
            return PositionImpl(fileName, fullScript);
        }

        private static EmptyPosition PositionImpl(string fileName, string fullScript)
        {
            return FileBasedCache<EmptyPosition>.Instance.GetOrAdd(
                fileName ?? string.Empty,
                new EmptyPosition(fileName ?? string.Empty, fullScript ?? string.Empty));
        }

        public static IScriptExtent Extent(IScriptExtent extent = null)
        {
            var position = PositionImpl(
                extent?.StartScriptPosition.File,
                extent?.StartScriptPosition.GetFullScript());

            return FileBasedCache<EmptyExtent>.Instance.GetOrAdd(
                position.File ?? string.Empty,
                new EmptyExtent(position));
        }

        private class EmptyPosition : IScriptPosition
        {
            private readonly string _fullScript;

            internal EmptyPosition(string fileName, string fullScript)
            {
                File = fileName ?? string.Empty;
                _fullScript = fullScript ?? string.Empty;
            }

            public int ColumnNumber => 1;

            public string File { get; }

            public string Line => string.Empty;

            public int LineNumber => 1;

            public int Offset => 0;

            public string GetFullScript() => _fullScript;
        }

        private class EmptyExtent : IScriptExtent
        {
            public EmptyExtent(EmptyPosition position)
            {
                StartScriptPosition = position;
            }

            public int EndColumnNumber => 1;

            public int EndLineNumber => 1;

            public int EndOffset => 0;

            public IScriptPosition EndScriptPosition => StartScriptPosition;

            public string File => StartScriptPosition.File;

            public int StartColumnNumber => 1;

            public int StartLineNumber => 1;

            public int StartOffset => 0;

            public IScriptPosition StartScriptPosition { get; }

            public string Text => string.Empty;
        }

        private static class FileBasedCache<T>
        {
            public static readonly ConcurrentDictionary<string, T> Instance =
                new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
