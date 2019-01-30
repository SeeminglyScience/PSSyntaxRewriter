using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace SequencePointTraceRewriter
{
    internal class ColorPicker
    {
        private readonly ConsoleColor[] _validColors;

        private int _index;

        public ColorPicker()
        {
            PSHost host = null;
            if (Runspace.DefaultRunspace != null)
            {
                using (var pwsh = PowerShell.Create(RunspaceMode.CurrentRunspace))
                {
                    host = pwsh.AddScript("$Host", false).Invoke<PSHost>().FirstOrDefault();
                }
            }

            ConsoleColor fg = host?.UI?.RawUI?.ForegroundColor ?? Console.ForegroundColor;
            var enumValues = (ConsoleColor[])typeof(ConsoleColor).GetEnumValues();
            var validColors = new ConsoleColor[enumValues.Length - 1];
            var foundCurrent = false;
            for (int i = 0; i < enumValues.Length; i++)
            {
                if (enumValues[i] == fg)
                {
                    foundCurrent = true;
                    continue;
                }

                validColors[i - (foundCurrent ? 1 : 0)] = enumValues[i];
            }

            _validColors = validColors;
        }

        public ColorPicker(params ConsoleColor[] validColors)
        {
            _validColors = validColors ?? throw new ArgumentException(nameof(validColors));
            if (validColors.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(validColors));
            }
        }

        public ConsoleColor Next()
        {
            var result = _validColors[_index];
            if (_index + 1 < _validColors.Length)
            {
                _index++;
                return result;
            }

            _index = 0;
            return result;
        }
    }
}
