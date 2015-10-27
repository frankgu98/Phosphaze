using Phosphaze.Core.Dml.Parse;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Builtins
{
    public class At : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new At());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "At"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, time <= ltime && ltime < time + Globals.deltaTime);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, time <= ltime && ltime < time + Globals.deltaTime);
        }
    }

    public class From : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new From());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "From"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            return types[0] == types[1] && types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject endObj = stack.Pop();
            DmlObject startObj = stack.Pop();

            if (startObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, startObj.Type);

            if (endObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, endObj.Type);

            double end = (double)(endObj.Value);
            double start = (double)(startObj.Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, start <= ltime && ltime < end);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double end = (double)(stack.Pop().Value);
            double start = (double)(stack.Pop().Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, start <= ltime && ltime < end);
        }
    }

    public class Outside : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Outside());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Outside"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            return types[0] == types[1] && types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject endObj = stack.Pop();
            DmlObject startObj = stack.Pop();

            if (startObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, startObj.Type);

            if (endObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, endObj.Type);

            double end = (double)(endObj.Value);
            double start = (double)(startObj.Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, start >= ltime && ltime > end);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double end = (double)(stack.Pop().Value);
            double start = (double)(stack.Pop().Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, start >= ltime && ltime > end);
        }
    }

    public class Before : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Before());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Before"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, ltime < time);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, ltime < time);
        }
    }

    public class After : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new After());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "After"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, ltime >= time);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = ((DmlBullet)bullet.Value).LocalTime;
            return new DmlObject(DmlType.Bool, ltime >= time);
        }
    }

    public class DuringIntervals : DmlFunction
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new DuringIntervals());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "DuringIntervals"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1 || argCount == 3;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.Number;
                case 3: return types[0] == DmlType.Number && types[1] == DmlType.Number && types[2] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject intervalObj;
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    interval = (double)(intervalObj.Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    return new DmlObject(DmlType.Bool, (ltime % (2 * interval)) < interval);
                case 3:
                    DmlObject endObj = stack.Pop();
                    DmlObject startObj = stack.Pop();
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    if (startObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, startObj.Type);

                    if (endObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 3, DmlType.Number, endObj.Type);

                    end = (double)(endObj.Value);
                    start = (double)(startObj.Value);
                    interval = (double)(intervalObj.Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % (2 * interval)) < interval);
                default:
                    return null; // Should never get here.
            }
        }        

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    interval = (double)(stack.Pop().Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;

                    return new DmlObject(DmlType.Bool, (ltime % (2 * interval)) < interval);
                case 3:
                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);

                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % (2 * interval)) < interval);
                default:
                    return null; // Should never get here.
            }
        }
    }

    public class AtIntervals : DmlFunction
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AtIntervals());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AtIntervals"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1 || argCount == 3;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.Number;
                case 3: return types[0] == DmlType.Number && types[1] == DmlType.Number && types[2] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject intervalObj;
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    interval = (double)(intervalObj.Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    return new DmlObject(DmlType.Bool, (ltime % interval) < Globals.deltaTime);
                case 3:
                    DmlObject endObj = stack.Pop();
                    DmlObject startObj = stack.Pop();
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    if (startObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, startObj.Type);

                    if (endObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 3, DmlType.Number, endObj.Type);

                    end = (double)(endObj.Value);
                    start = (double)(startObj.Value);
                    interval = (double)(intervalObj.Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % interval) < Globals.deltaTime);
                default:
                    return null; // Should never get here.
            }
        } 

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    interval = (double)(stack.Pop().Value);
                    ltime = ((DmlBullet)bullet.Value).LocalTime;

                    return new DmlObject(DmlType.Bool, (ltime % interval) < Globals.deltaTime);
                case 3:
                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);

                    ltime = ((DmlBullet)bullet.Value).LocalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % interval) < Globals.deltaTime);
                default:
                    return null; // Should never get here.
            }
        }
    }

    public class AtGlobal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AtGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AtGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, time <= ltime && ltime < time + Globals.deltaTime);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, time <= ltime && ltime < time + Globals.deltaTime);
        }
    }

    public class FromGlobal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new FromGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "FromGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            return types[0] == types[1] && types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject endObj = stack.Pop();
            DmlObject startObj = stack.Pop();

            if (startObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, startObj.Type);

            if (endObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, endObj.Type);

            double end = (double)(endObj.Value);
            double start = (double)(startObj.Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, start <= ltime && ltime < end);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double end = (double)(stack.Pop().Value);
            double start = (double)(stack.Pop().Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, start <= ltime && ltime < end);
        }
    }

    public class OutsideGlobal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new OutsideGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "OutsideGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            return types[0] == types[1] && types[0] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject endObj = stack.Pop();
            DmlObject startObj = stack.Pop();

            if (startObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, startObj.Type);

            if (endObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, endObj.Type);

            double end = (double)(endObj.Value);
            double start = (double)(startObj.Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, start >= ltime && ltime > end);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double end = (double)(stack.Pop().Value);
            double start = (double)(stack.Pop().Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, start >= ltime && ltime > end);
        }
    }

    public class BeforeGlobal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new BeforeGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "BeforeGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, ltime < time);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, ltime < time);
        }
    }

    public class AfterGlobal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AfterGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AfterGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

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
            DmlObject timeObj = stack.Pop();
            if (timeObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, timeObj.Type);
            double time = (double)(timeObj.Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, ltime >= time);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double time = (double)(stack.Pop().Value);
            double ltime = system.GlobalTime;
            return new DmlObject(DmlType.Bool, ltime >= time);
        }
    }

    public class DuringIntervalsGlobal : DmlFunction
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new DuringIntervalsGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "DuringIntervalsGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1 || argCount == 3;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.Number;
                case 3: return types[0] == DmlType.Number && types[1] == DmlType.Number && types[2] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject intervalObj;
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;
                    return new DmlObject(DmlType.Bool, (ltime % (2 * interval)) < interval);
                case 3:
                    DmlObject endObj = stack.Pop();
                    DmlObject startObj = stack.Pop();
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    if (startObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, startObj.Type);

                    if (endObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 3, DmlType.Number, endObj.Type);

                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % (2 * interval)) < interval);
                default:
                    return null; // Should never get here.
            }
        }        

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;

                    return new DmlObject(DmlType.Bool, (ltime % (2 * interval)) < interval);
                case 3:
                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);

                    ltime = system.GlobalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % (2 * interval)) < interval);
                default:
                    return null; // Should never get here.
            }
        }
    }

    public class AtIntervalsGlobal : DmlFunction
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AtIntervalsGlobal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AtIntervalsGlobal"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return false; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 1 || argCount == 3;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            switch (types.Length)
            {
                case 1: return types[0] == DmlType.Number;
                case 3: return types[0] == DmlType.Number && types[1] == DmlType.Number && types[2] == DmlType.Number;
                default: return false;
            }
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject intervalObj;
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;
                    return new DmlObject(DmlType.Bool, (ltime % interval) < Globals.deltaTime);
                case 3:
                    DmlObject endObj = stack.Pop();
                    DmlObject startObj = stack.Pop();
                    intervalObj = stack.Pop();

                    if (intervalObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, intervalObj.Type);

                    if (startObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, startObj.Type);

                    if (endObj.Type != DmlType.Number)
                        throw DmlSyntaxError.BadArgumentType(Name, 3, DmlType.Number, endObj.Type);

                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % interval) < Globals.deltaTime);
                default:
                    return null; // Should never get here.
            }
        } 

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double start, end, interval, ltime;
            switch (argCount)
            {
                case 1:
                    interval = (double)(stack.Pop().Value);
                    ltime = system.GlobalTime;

                    return new DmlObject(DmlType.Bool, (ltime % interval) < Globals.deltaTime);
                case 3:
                    end = (double)(stack.Pop().Value);
                    start = (double)(stack.Pop().Value);
                    interval = (double)(stack.Pop().Value);

                    ltime = system.GlobalTime;
                    double t2 = ltime - start;
                    return new DmlObject(DmlType.Bool,
                        (t2 >= 0 && ltime <= end) && (t2 % interval) < Globals.deltaTime);
                default:
                    return null; // Should never get here.
            }
        }
    }
}
