using Phosphaze.Core.Dml.Parse;
using Phosphaze.Core.Utils;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Builtins
{
    /// <summary>
    /// The builtin Sign function that returns 1 if x >= 0 else -1.
    /// </summary>
    public class Sign : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Sign());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Sign"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = (double)top.Value < 0 ? -1 : 1;
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, top < 0 ? -1 : 1);
        }
    }

    /// <summary>
    /// The builtin Floor function that returns the floor of a given number.
    /// </summary>
    public class Floor : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Floor());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Floor"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = Math.Floor((double)top.Value);
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Floor(top));
        }
    }

    /// <summary>
    /// The builting Ceil function that returns the ceiling of a given number.
    /// </summary>
    public class Ceil : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Ceil());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Ceil"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = Math.Ceiling((double)top.Value);
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Ceiling(top));
        }
    }

    /// <summary>
    /// The builtin Max function. If two arguments are supplied, then the max of the two is returned.
    /// If one argument is supplied, then it is expected that it is a list of numbers, in which case
    /// the max of the list is returned.
    /// </summary>
    public class Max : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Max());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Max"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount <= 2 && argCount != 0;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.List;
                case 2: return types[0] == types[1] && types[0] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            switch (argCount)
            {
                case 1:
                    DmlObject top = stack.Pop();
                    if (top.Type != DmlType.List)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
                    double high = Double.NegativeInfinity;
                    
                    try {
                        double val;
                        foreach (DmlObject v in (DmlObject[])top.Value) 
                        {
                            val = (double)v.Value;
                            if (val >= high)
                                high = val;
                        }    
                    }
                    catch (InvalidCastException)
                    {
                        throw new DmlSyntaxError(
                            "Invalid syntax; `Max` requires input array to be composed entirely of numbers."
                            );
                    }
                    return new DmlObject(DmlType.Number, high);
                case 2:
                    DmlObject second = stack.Pop();
                    DmlObject first = stack.Pop();

                    if (first.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);

                    else if (second.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);

                    return new DmlObject(
                        DmlType.Number, 
                        (double)first.Value > (double)second.Value ? first.Value : second.Value
                        );
                default:
                    throw new DmlSyntaxError(
                        "Invalid syntax; `Max` requires either 1 or 2 arguments."
                        );
            }
        }
    }

    /// <summary>
    /// The builtin Min function. If two arguments are supplied, then the min of the two is returned.
    /// If one argument is supplied, then it is expected that it is a list of numbers, in which case
    /// the min of the list is returned.
    /// </summary>
    public class Min : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Min());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Min"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount <= 2 && argCount != 0;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.List;
                case 2: return types[0] == types[1] && types[0] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            switch (argCount)
            {
                case 1:
                    DmlObject top = stack.Pop();
                    if (top.Type != DmlType.List)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
                    double low = Double.PositiveInfinity;
                    
                    try {
                        double val;
                        foreach (DmlObject v in (DmlObject[])top.Value) 
                        {
                            val = (double)v.Value;
                            if (val <= low)
                                low = val;
                        }    
                    }
                    catch (InvalidCastException)
                    {
                        throw new DmlSyntaxError(
                            "Invalid syntax; `Min` requires input array to be composed entirely of numbers."
                            );
                    }
                    return new DmlObject(DmlType.Number, low);
                case 2:
                    DmlObject second = stack.Pop();
                    DmlObject first = stack.Pop();

                    if (first.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);

                    else if (second.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);

                    return new DmlObject(
                        DmlType.Number, 
                        (double)first.Value < (double)second.Value ? first.Value : second.Value
                        );
                default:
                    throw new DmlSyntaxError(
                        "Invalid syntax; `Min` requires either 1 or 2 arguments."
                        );
            }
        }
    }

    /// <summary>
    /// The builtin Log function that returns the natural logarithm of a given number.
    /// </summary>
    public class Log : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Log());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Log"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = Math.Log((double)top.Value);
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Log(top));
        }
    }

    /// <summary>
    /// The builtin LogB function that returns the logarithm of a number in an arbitrary base.
    /// </summary>
    public class LogB : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new LogB());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "LogB"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            return new DmlObject(DmlType.Number, Math.Log((double)first.Value, (double)second.Value));
                        default:
                            // throw new syntax error.
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double b = (double)(stack.Pop().Value);
            double x = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Log(x, b));
        }
    }

    /// <summary>
    /// The builtin Exp function that returns the exponential of a number (e^x).
    /// </summary>
    public class Exp : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Exp());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Exp"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = Math.Exp((double)top.Value);
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Exp(top));
        }
    }

    /// <summary>
    /// The builtin Factorial function that returns the factorial of an input number.
    /// </summary>
    public class Factorial : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Factorial());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Factorial"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    double p = (double)top.Value;
                    double val = (int)p;
                    if (val != Math.Floor(p))
                        throw new DmlSyntaxError(
                            "Invalid syntax; `Factorial` requires that argument one be an integer."
                            );
                    int factorial = 1;
                    for (int i = 2; i < val; i++)
                        factorial *= i;
                    top.Value = val;
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double top = (double)(stack.Pop().Value);
            int factorial = 1;
            for (int i = 2; i < top; i++)
                factorial *= i;
            return new DmlObject(DmlType.Number, factorial);
        }
    }

    public class Atan2 : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Atan2());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Atan2"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            return new DmlObject(DmlType.Number, Math.Atan2((double)first.Value, (double)second.Value));
                        default:
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double second = (double)(stack.Pop().Value);
            double first = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Atan2(first, second));
        }
    }

    public class Atan2D : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Atan2D());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Atan2D"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            return new DmlObject(DmlType.Number, 180/Math.PI*Math.Atan2((double)first.Value, (double)second.Value));
                        default:
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double second = (double)(stack.Pop().Value);
            double first = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, 180/Math.PI*Math.Atan2(first, second));
        }
    }

    /// <summary>
    /// The builtin BesselJ function that represents the Bessel J function of the first kind arbitrary order.
    /// </summary>
    public class BesselJ : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new BesselJ());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "BesselJ"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            return new DmlObject(DmlType.Number, SpecialFunctions.JN((int)(double)first.Value, (double)second.Value));
                        default:
                            // throw new syntax error.
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double z = (double)(stack.Pop().Value);
            int n = (int)(double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, SpecialFunctions.JN(n, z));
        }
    }

    /// <summary>
    /// The builtin BesselY function that represents the Bessel J function of the second kind arbitrary order.
    /// </summary>
    public class BesselY : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new BesselY());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "BesselY"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            return new DmlObject(DmlType.Number, SpecialFunctions.YN((int)(double)first.Value, (double)second.Value));
                        default:
                            // throw new syntax error.
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double z = (double)(stack.Pop().Value);
            int n = (int)(double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, SpecialFunctions.YN(n, z));
        }
    }

    
    public class Random : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Random());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Random"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 2;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number &&
                   types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            var max = (double)(second.Value);
                            var min = (double)(first.Value);
                            return new DmlObject(DmlType.Number, (max - min)*Globals.randGen.NextDouble() + min);
                        default:
                            // throw new syntax error.
                            throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, second.Type);
                    }
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, first.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double b = (double)(stack.Pop().Value);
            double x = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Log(x, b));
        }
    }

    /// <summary>
    /// A multiton describing the various unary math functions in Dml.
    /// </summary>
    public class UnaryMath : DmlFunction
    {

        Func<double, double> func;

        string name;

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return name; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Number;
        }

        private UnaryMath(Func<double, double> func, string name)
        {
            this.func = func;
            this.name = name;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            switch (top.Type)
            {
                case DmlType.Number:
                    top.Value = func((double)top.Value);
                    return top;
                default:
                    throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            }
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            return new DmlObject(DmlType.Number, func((double)(stack.Pop().Value)));
        }

        /// <summary>
        /// The builtin function Sin that returns the sine of a number.
        /// </summary>
        public static readonly DmlObject Sin = new DmlObject(DmlType.Function, new UnaryMath(Math.Sin, "Sin"));
        /// <summary>
        /// The builtin function SinD that returns the sine of a number (in degrees).
        /// </summary>
        public static readonly DmlObject SinD = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Sin(Math.PI / 180 * d), "SinD"));
        /// <summary>
        /// The builtin function Cos that returns the cosine of a number.
        /// </summary>
        public static readonly DmlObject Cos = new DmlObject(DmlType.Function, new UnaryMath(Math.Cos, "Cos"));
        /// <summary>
        /// The builtin function CosD that returns the cosine of a number (in degrees).
        /// </summary>
        public static readonly DmlObject CosD = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Cos(Math.PI / 180 * d), "CosD"));
        /// <summary>
        /// The builtin function Tan that returns the tangent of a number.
        /// </summary>
        public static readonly DmlObject Tan = new DmlObject(DmlType.Function, new UnaryMath(Math.Tan, "Tan"));
        /// <summary>
        /// The builtin function TanD that returns the tangent of a number (in degrees).
        /// </summary>
        public static readonly DmlObject TanD = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Tan(Math.PI / 180 * d), "TanD"));
        /// <summary>
        /// The builtin function Sec that returns the secant of a number.
        /// </summary>
        public static readonly DmlObject Sec = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Cos(d), "Sec"));
        /// <summary>
        /// The builtin function SecD that returns the secant of a number (in degrees).
        /// </summary>
        public static readonly DmlObject SecD = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Cos(Math.PI / 180 * d), "SecD"));
        /// <summary>
        /// The builtin function Csc that returns the cosecant of a number.
        /// </summary>
        public static readonly DmlObject Csc = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Sin(d), "Csc"));
        /// <summary>
        /// The builtin function CscD that returns the cosecant of a number (in degrees).
        /// </summary>
        public static readonly DmlObject CscD = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Sin(Math.PI / 180 * d), "CscD"));
        /// <summary>
        /// The builtin function Cot that returns the cotangent of a number.
        /// </summary>
        public static readonly DmlObject Cot = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Tan(d), "Cot"));
        /// <summary>
        /// The builtin function CotD that returns the cotangent of a number (in degrees).
        /// </summary>
        public static readonly DmlObject CotD = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Tan(Math.PI / 180 * d), "CotD"));


        /// <summary>
        /// The builtin function Arcsin that returns the inverse sine of a number.
        /// </summary>
        public static readonly DmlObject Arcsin = new DmlObject(DmlType.Function, new UnaryMath(Math.Asin, "Arcsin"));
        /// <summary>
        /// The builtin function ArcsinD that returns the inverse sine of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArcsinD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Asin(d), "ArcsinD"));
        /// <summary>
        /// The builtin function Arccos that returns the inverse cosine of a number.
        /// </summary>
        public static readonly DmlObject Arccos = new DmlObject(DmlType.Function, new UnaryMath(Math.Acos, "ArccosD"));
        /// <summary>
        /// The builtin function ArccosD that returns the inverse cosine of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArccosD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Acos(d), "ArccosD"));
        /// <summary>
        /// The builtin function Arctan that returns the inverse tangent of a number.
        /// </summary>
        public static readonly DmlObject Arctan = new DmlObject(DmlType.Function, new UnaryMath(Math.Atan, "Arctan"));
        /// <summary>
        /// The builtin function ArctanD that returns the inverse tangent of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArctanD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Atan(d), "ArctanD"));
        /// <summary>
        /// The builtin function Arcsec that returns the inverse secant of a number.
        /// </summary>
        public static readonly DmlObject Arcsec = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Acos(1 / d), "Arcsec"));
        /// <summary>
        /// The builtin function ArcsecD that returns the inverse secant of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArcsecD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Acos(1 / d), "ArcsecD"));
        /// <summary>
        /// The builtin function Arccsc that returns the inverse cosecant of a number.
        /// </summary>
        public static readonly DmlObject Arccsc = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Asin(1 / d), "Arccsc"));
        /// <summary>
        /// The builtin function ArccscD that returns the inverse cosecant of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArccscD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Asin(1 / d), "ArccscD"));
        /// <summary>
        /// The builtin function Arccot that returns the inverse cotangent of a number.
        /// </summary>
        public static readonly DmlObject Arccot = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Atan(1 / d), "Arccot"));
        /// <summary>
        /// The builtin function ArccotD that returns the inverse cotangent of a number (in degrees).
        /// </summary>
        public static readonly DmlObject ArccotD = new DmlObject(DmlType.Function, new UnaryMath(d => 180 / Math.PI * Math.Atan(1 / d), "ArccotD"));


        /// <summary>
        /// The builtin function Sinh that returns the hyperbolic sine of a number.
        /// </summary>
        public static readonly DmlObject Sinh = new DmlObject(DmlType.Function, new UnaryMath(Math.Sinh, "Sinh"));
        /// <summary>
        /// The builtin function Cosh that returns the hyperbolic cosine of a number.
        /// </summary>
        public static readonly DmlObject Cosh = new DmlObject(DmlType.Function, new UnaryMath(Math.Cosh, "Cosh"));
        /// <summary>
        /// The builtin function Tanh that returns the hyperbolic tangent of a number.
        /// </summary>
        public static readonly DmlObject Tanh = new DmlObject(DmlType.Function, new UnaryMath(Math.Tanh, "Tanh"));
        /// <summary>
        /// The builtin function Sech that returns the hyperbolic secant of a number.
        /// </summary>
        public static readonly DmlObject Sech = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Cosh(d), "Sech"));
        /// <summary>
        /// The builtin function Csch that returns the hyperbolic cosecant of a number.
        /// </summary>
        public static readonly DmlObject Csch = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Sinh(d), "Csch"));
        /// <summary>
        /// The builtin function Coth that returns the hyperbolic cotangent of a number.
        /// </summary>
        public static readonly DmlObject Coth = new DmlObject(DmlType.Function, new UnaryMath(d => 1 / Math.Tanh(d), "Coth"));


        /// <summary>
        /// The builtin function Asinh that returns the inverse hyperbolic sine of a number.
        /// </summary>
        public static readonly DmlObject Arsinh = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Asinh, "Arsinh"));
        /// <summary>
        /// The builtin function Arcosh that returns the inverse hyperbolic cosine of a number.
        /// </summary>
        public static readonly DmlObject Arcosh = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Acosh, "Arcosh"));
        /// <summary>
        /// The builtin function Artanh that returns the inverse hyperbolic tangent of a number.
        /// </summary>
        public static readonly DmlObject Artanh = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Atanh, "Artanh"));
        /// <summary>
        /// The builtin function Arsech that returns the inverse hyperbolic secant of a number.
        /// </summary>
        public static readonly DmlObject Arsech = new DmlObject(DmlType.Function, new UnaryMath(d => SpecialFunctions.Acosh(1 / d), "Arsech"));
        /// <summary>
        /// The builtin function Arcsch that returns the inverse hyperbolic cosecant of a number.
        /// </summary>
        public static readonly DmlObject Arcsch = new DmlObject(DmlType.Function, new UnaryMath(d => SpecialFunctions.Asinh(1 / d), "Arcsch"));
        /// <summary>
        /// The builtin function Arcoth that returns the inverse hyperbolic cotangent of a number.
        /// </summary>
        public static readonly DmlObject Arcoth = new DmlObject(DmlType.Function, new UnaryMath(d => SpecialFunctions.Atanh(1 / d), "Arcoth"));

        /// <summary>
        /// The builtin function Sinc that returns the cardinal sine of a number.
        /// </summary>
        public static readonly DmlObject Sinc = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Sin(d) / d, "Sinc"));
        /// <summary>
        /// The builtin function Tanhc that returns the cardinal hyperbolic tangent of a number.
        /// </summary>
        public static readonly DmlObject Tanhc = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Tanh(d) / d, "Tanhc"));
        /// <summary>
        /// The builtin function Erf that represents the Error function.
        /// </summary>
        public static readonly DmlObject Erf = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Erf, "Erf"));
        /// <summary>
        /// The builtin function Gudermannian that represents the Gudermannian function.
        /// </summary>
        public static readonly DmlObject Gudermannian = new DmlObject(DmlType.Function, new UnaryMath(d => Math.Asin(Math.Tanh(d)), "Gudermannian"));
        /// <summary>
        /// The builtin function InverseGudermannian that represents the inverse to the Gudermannian function.
        /// </summary>
        public static readonly DmlObject InverseGudermannian = new DmlObject(DmlType.Function, new UnaryMath(d => SpecialFunctions.Atanh(Math.Sin(d)), "InverseGudermannian"));
        /// <summary>
        /// The builtin function Gamma that represents the Gamma function.
        /// </summary>
        public static readonly DmlObject Gamma = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Gamma, "Gamma"));
        /// <summary>
        /// The builtin funcion BesselJ0 that represents the Bessel function of the first kind of order 0.
        /// </summary>
        public static readonly DmlObject BesselJ0 = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.J0, "BesselJ0"));
        /// <summary>
        /// The builtin function BesselJ1 that represents the Bessel function of the first kind of order 1.
        /// </summary>
        public static readonly DmlObject BesselJ1 = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.J1, "BesselJ1"));
        /// <summary>
        /// The builtin function BesselY0 that represents the Bessel function of the second kind of order 0.
        /// </summary>
        public static readonly DmlObject BesselY0 = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Y0, "BesselY0"));
        /// <summary>
        /// The builtin function BesselY1 that represents the Bessel function of the second kind of order 1.
        /// </summary>
        public static readonly DmlObject BesselY1 = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.Y1, "BesselY1"));
        /// <summary>
        /// The builtin function AiryA that represents the Airy Ai function.
        /// </summary>
        public static readonly DmlObject AiryA = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.AiryA, "AiryA"));
        /// <summary>
        /// The builtin function AiryB that represents the Airy Bi function.
        /// </summary>
        public static readonly DmlObject AiryB = new DmlObject(DmlType.Function, new UnaryMath(SpecialFunctions.AiryB, "AiryB"));

    }
}
