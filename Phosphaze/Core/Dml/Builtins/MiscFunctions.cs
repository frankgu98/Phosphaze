using Microsoft.Xna.Framework;
using Phosphaze.Core.Dml.Parse;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Builtins
{
    
    /// <summary>
    /// The builtin function ConsoleOutput that writes a set of DmlObjects to the console separated by spaces.
    /// </summary>
    public class ConsoleOutput : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new ConsoleOutput());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "ConsoleOutput"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public virtual bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public virtual bool CompatibleWithArgCount(int argCount)
        {
            return true;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public virtual bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return true;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top;
            string msg = "";
            for (int i = 0; i < argCount; i++)
            {
                top = stack.Pop();
                msg += top.Value.ToString();
                if (i != argCount - 1)
                    msg += " ";

            }
            Console.WriteLine(msg);
            return null;
        }
    }

    /// <summary>
    /// The builtin function Vector that constructs a new vector with a given abcissa and ordinate.
    /// </summary>
    public class Vector : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Vector());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Vector"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public virtual bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public virtual bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public virtual bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            if (first.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);

            if (second.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);

            double y = (double)(second.Value);
            double x = (double)(first.Value);

            return new DmlObject(DmlType.Vector, new Vector2((float)x, (float)y));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double y = (double)(stack.Pop().Value);
            double x = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, new Vector2((float)x, (float)y));
        }
    }

    /// <summary>
    /// The builtin function Array that constructs a new array containing the given elements.
    /// </summary>
    public class Array : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Array());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Array"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public virtual bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public virtual bool CompatibleWithArgCount(int argCount)
        {
            return true;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public virtual bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return true;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            var list = new List<DmlObject>();
            for (int i = 0; i < argCount; i++)
                list.Add(stack.Pop());
            list.Reverse(); // We have to reverse it since we pop the objects from the stack in reverse order.
            return new DmlObject(DmlType.List, list);
        }
    }

}
