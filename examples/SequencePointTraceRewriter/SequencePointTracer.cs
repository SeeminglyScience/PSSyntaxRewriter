using System;
using System.Collections.Concurrent;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;

namespace SequencePointTraceRewriter
{
    public static class SequencePointTracer
    {
        private static readonly ConcurrentDictionary<Type, string> s_astNameMap =
            new ConcurrentDictionary<Type, string>();

        public static void TraceExpression(
            PSHost host,
            ConsoleColor foregroundColor,
            Type astType,
            int stackIndex,
            int line,
            int column,
            object value,
            bool isStarting)
        {
            string message;
            Type resultType;
            if (value == null)
            {
                resultType = typeof(object);
                message = null;
            }
            else
            {
                resultType = value.GetType();
                message = LanguagePrimitives.ConvertTo<string>(value);
            }

            message = GetHostText(
                host,
                isStarting ? astType : resultType,
                stackIndex,
                line,
                column,
                message,
                isStarting);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            host.UI.WriteLine(foregroundColor, host.UI.RawUI.BackgroundColor, message);
        }

        private static string GetHostText(
            PSHost host,
            Type astType,
            int stackIndex,
            int line,
            int column,
            string message,
            bool isStarting)
        {
            var maxWidth = host?.UI?.RawUI?.WindowSize.Width ?? 0;
            if (maxWidth == 0)
            {
                return string.Empty;
            }

            var text = new StringBuilder(maxWidth);
            text.Append('-', (stackIndex - 1) * 4)
                .Append(isStarting ? '>' : '<')
                .Append(" Ln ")
                .Append(line)
                .Append(", Col ")
                .Append(column)
                .Append("; ")
                .Append(isStarting ? GetAstName(astType) : ("Result <" + astType.Name + ">"))
                .Append(": ");

            if (string.IsNullOrEmpty(message))
            {
                return text.Append("<null>").ToString();
            }

            text.Append('"');
            int length = text.Length;
            for (int i = 0; i < message.Length; i++, length++)
            {
                if (length >= maxWidth)
                {
                    var overage = text.Length - maxWidth;
                    return text
                        .Remove((text.Length - overage - 5), 4 + overage)
                        .Append("...\"")
                        .ToString();
                }

                char c = message[i];
                if (c == '\r')
                {
                    text.Append(@"\r");
                    length++;
                    continue;
                }

                if (c == '\n')
                {
                    text.Append(@"\n");
                    length++;
                    continue;
                }

                text.Append(c);
            }

            return text.Append('"').ToString();
        }

        private static string GetAstName(Type astType)
        {
            return s_astNameMap.GetOrAdd(
                astType,
                t => t.Name.Replace("Ast", string.Empty));
        }
    }
}
