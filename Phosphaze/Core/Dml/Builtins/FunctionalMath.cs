using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze.Core.Utils;
using Phosphaze.Core.Dml.Parse;

namespace Phosphaze.Core.Dml.Builtins
{

    public class SquareWave : DmlFunction
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new SquareWave());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "SquareWave"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 5;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types.AllEqual(DmlType.Number);
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            var time = (DmlObject)(stack.Pop());
            var period = (DmlObject)(stack.Pop());
            var start = (DmlObject)(stack.Pop());
            var max = (DmlObject)(stack.Pop());
            var min = (DmlObject)(stack.Pop());

            if (min.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, min.Type);

            if (max.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, max.Type);

            if (start.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 3, DmlType.Number, start.Type);

            if (period.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 4, DmlType.Number, period.Type);
            
            if (time.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 5, DmlType.Number, time.Type);

            return new DmlObject(DmlType.Number, _squareWave(
                (double)(min.Value),
                (double)(max.Value),
                (double)(start.Value),
                (double)(period.Value),
                (double)(time.Value)
                ));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            var time = (double)(stack.Pop().Value);
            var period = (double)(stack.Pop().Value);
            var start = (double)(stack.Pop().Value);
            var max = (double)(stack.Pop().Value);
            var min = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, _squareWave(min, max, start, period, time));
        }

        private double _squareWave(double min, double max, double start, double period, double time)
        {
            return ((time - start) % (2*period)) < period ? min : max;
        }
    }

}
