using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace ClosureRewriter
{
    internal readonly struct Closure
    {
        private const int DollarUnder = 0;

        private const int Args = 1;

        private const int ScriptThis = 2;

        public readonly PSVariable[] Inherits;

        public readonly object[] Locals;

        public readonly object[] Parameters;

        public readonly CommandInfo[] Commands;

        public readonly ScriptBlock ScriptBlock;

        public readonly List<object> OutputStream;

        public readonly object[] Autos;

        internal Closure(
            PSVariable[] inherits,
            object[] locals,
            object[] parameters,
            CommandInfo[] commands,
            ScriptBlock scriptBlock)
            : this(inherits, locals, parameters, commands, scriptBlock, new List<object>(), new object[3])
        {
        }

        internal Closure(
            PSVariable[] inherits,
            object[] locals,
            object[] parameters,
            CommandInfo[] commands,
            ScriptBlock scriptBlock,
            List<object> outputList)
            : this(inherits, locals, parameters, commands, scriptBlock, outputList, new object[3])
        {
        }

        internal Closure(
            PSVariable[] inherits,
            object[] locals,
            object[] parameters,
            CommandInfo[] commands,
            ScriptBlock scriptBlock,
            object[] autos)
            : this(inherits, locals, parameters, commands, scriptBlock, new List<object>(), autos)
        {
        }

        internal Closure(
            PSVariable[] inherits,
            object[] locals,
            object[] parameters,
            CommandInfo[] commands,
            ScriptBlock scriptBlock,
            List<object> outputList,
            object[] autos)
        {
            Inherits = inherits;
            Locals = locals;
            Parameters = parameters;
            Commands = commands;
            ScriptBlock = scriptBlock;
            OutputStream = outputList;
            Autos = autos ?? new object[3];
        }

        internal Closure With(
            object dollarUnder = null,
            object[] args = null,
            bool shouldReset = false)
        {
            PrepareStatesForClone(
                shouldReset,
                out object[] locals,
                out object[] parameters,
                out object[] autos);

            autos[DollarUnder] = dollarUnder ?? AutomationNull.Value;
            autos[Args] = args ?? Array.Empty<object>();
            return new Closure(
                Inherits,
                locals,
                parameters,
                Commands,
                ScriptBlock,
                OutputStream,
                autos);
        }

        internal Closure WithDollarUnder(object dollarUnder, bool shouldReset)
        {
            PrepareStatesForClone(
                shouldReset,
                out object[] locals,
                out object[] parameters,
                out object[] autos);

            autos[DollarUnder] = dollarUnder;
            return new Closure(
                Inherits,
                locals,
                parameters,
                Commands,
                ScriptBlock,
                OutputStream,
                autos);
        }

        private void PrepareStatesForClone(
            bool shouldReset,
            out object[] locals,
            out object[] parameters,
            out object[] autos)
        {
            if (shouldReset)
            {
                locals = GetEmptyCopy(Locals);
                parameters = GetEmptyCopy(Parameters);
                autos = new object[3];
                autos[Args] = GetEmptyCopy(parameters);
                autos[ScriptThis] = Autos[ScriptThis];
                return;
            }

            locals = Locals ?? Array.Empty<object>();
            parameters = Parameters ?? Array.Empty<object>();
            autos = Autos ?? new object[3];
        }

        private static object[] GetEmptyCopy(object[] source, int defaultLength = 0)
        {
            if (source == null)
            {
                if (defaultLength > 0)
                {
                    return new object[defaultLength];
                }

                return Array.Empty<object>();
            }

            if (source.Length == 0)
            {
                return source;
            }

            return new object[source.Length];
        }

        internal Closure Clone(object[] parameters = null)
        {
            return new Closure(
                Inherits,
                Locals.Length > 0 ? new object[Locals.Length] : Locals,
                parameters ?? Array.Empty<object>(),
                Commands,
                ScriptBlock,
                OutputStream,
                Autos);
        }
    }
}
