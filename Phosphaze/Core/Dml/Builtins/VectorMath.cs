using Microsoft.Xna.Framework;
using Phosphaze.Core.Dml.Parse;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Builtins
{

    /// <summary>
    /// The builtin function LeftNormal that returns the normal of a vector pointing left
    /// relative to the original vector. The left normal of a vector <x, y> is the vector
    /// <-y, x>.
    /// </summary>
    public class LeftNormal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new LeftNormal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "LeftNormal"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Vector, new Vector2(-vec.Y, vec.X));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, new Vector2(-top.Y, top.X));
        }
    }

    /// <summary>
    /// The builtin function RightNormal that returns the normal of a vector pointing right
    /// relative to the original vector. The right normal of a vector <x, y> is the vector
    /// <y, -x>.
    /// </summary>
    public class RightNormal : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new RightNormal());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "RightNormal"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Vector, new Vector2(vec.Y, -vec.X));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, new Vector2(top.Y, -top.X));
        }
    }

    /// <summary>
    /// The builtin function Normalized that returns the normalized version of a vector.
    /// </summary>
    public class Normalized : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Normalized());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Normalized"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            vec.Normalize();
            return new DmlObject(DmlType.Vector, vec);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            top.Normalize();
            return new DmlObject(DmlType.Vector, top);
        }
    }

    /// <summary>
    /// The builtin function Magnitude which returns the magnitude of an input vector.
    /// </summary>
    public class Magnitude : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Magnitude());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Magnitude"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Number, (double)Vector2.Distance(vec, Vector2.Zero));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, (double)Vector2.Distance(top, Vector2.Zero));
        }
    }

    /// <summary>
    /// The builtin function MagnitudeSqrd that return the squared magnitude of an input vector.
    /// </summary>
    public class MagnitudeSqrd : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new MagnitudeSqrd());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "MagnitudeSqrd"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Number, (double)Vector2.DistanceSquared(vec, Vector2.Zero));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, (double)Vector2.DistanceSquared(top, Vector2.Zero));
        }
    }

    /// <summary>
    /// The builtin function AngleOf which returns the angle in radians in which a vector
    /// points relative to the positive x-axis.
    /// </summary>
    public class AngleOf : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AngleOf());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AngleOf"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Number, Math.Atan2(vec.Y, vec.X));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, Math.Atan2(top.Y, top.X));
        }
    }

    /// <summary>
    /// The builtin function AngleOfD which returns the angle in degrees in which a vector
    /// points relative to the positive x-axis.
    /// </summary>
    public class AngleOfD : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new AngleOfD());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "AngleOfD"; } }

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
            return types[0] == DmlType.Vector;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, top.Type);
            Vector2 vec = (Vector2)(top.Value);
            return new DmlObject(DmlType.Number, 180/Math.PI*Math.Atan2(vec.Y, vec.X));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            Vector2 top = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Number, 180/Math.PI*Math.Atan2(top.Y, top.X));
        }
    }

    /// <summary>
    /// The builtin function Polar that returns the unit vector pointing in the direction 
    /// of the input angle. It is assumed that the input angle is in radians.
    /// </summary>
    public class Polar : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new Polar());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "Polar"; } }

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
            if (top.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            double angle = (double)(top.Value);
            return new DmlObject(DmlType.Vector, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double angle = (double)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }
    }

    /// <summary>
    /// The builtin function PolarD that returns the unit vector pointing in the direction 
    /// of the input angle. It is assumed that the input angle is in degrees.
    /// </summary>
    public class PolarD : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new PolarD());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "PolarD"; } }

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
            if (top.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, top.Type);
            double angle = Math.PI/180*(double)(top.Value);
            return new DmlObject(DmlType.Vector, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double angle = Math.PI/180*(double)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }
    }

    /// <summary>
    /// The builtin function RotateVector that rotates a vector by a given angle counter-clockwise.
    /// It is expected that the input angle is in radians.
    /// </summary>
    public class RotateVector : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new RotateVector());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "RotateVector"; } }

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
            return types[0] == DmlType.Vector && types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            if (first.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, first.Type);

            if (second.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, second.Type);

            double angle = (double)(second.Value);
            Vector2 vec = (Vector2)(first.Value);
            float x = vec.X;
            float y = vec.Y;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            var newVec = new Vector2(x * cos - y * sin, x * sin + y * cos);
            newVec.Normalize(); // This avoids rounding errors that cause the bullets to eventually speed up.
            return new DmlObject(DmlType.Vector, newVec);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double angle = (double)(stack.Pop().Value);
            Vector2 vec = (Vector2)(stack.Pop().Value);
            float x = vec.X;
            float y = vec.Y;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            var newVec = new Vector2(x*cos - y*sin, x*sin + y*cos);
            newVec.Normalize(); // This avoids rounding errors that cause the bullets to eventually speed up.
            return new DmlObject(DmlType.Vector, newVec);
        }
    }

    /// <summary>
    /// The builtin function RotateVector that rotates a vector by a given angle counter-clockwise.
    /// It is expected that the input angle is in degrees.
    /// </summary>
    public class RotateVectorD : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new RotateVectorD());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "RotateVectorD"; } }

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
            return types[0] == DmlType.Vector && types[1] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();

            if (first.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, first.Type);

            if (second.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Number, second.Type);

            double angle = Math.PI/180*(double)(second.Value);
            Vector2 vec = (Vector2)(first.Value);
            float x = vec.X;
            float y = vec.Y;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            var newVec = new Vector2(x * cos - y * sin, x * sin + y * cos);
            newVec.Normalize(); // This avoids rounding errors that cause the bullets to eventually speed up.
            return new DmlObject(DmlType.Vector, newVec);
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double angle = Math.PI/180*(double)(stack.Pop().Value);
            Vector2 vec = (Vector2)(stack.Pop().Value);
            float x = vec.X;
            float y = vec.Y;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            var newVec = new Vector2(x * cos - y * sin, x * sin + y * cos);
            newVec.Normalize(); // This avoids rounding errors that cause the bullets to eventually speed up.
            return new DmlObject(DmlType.Vector, newVec);
        }
    }

    /// <summary>
    /// The builtin function Lerp that linearly interpolates between two vectors by a given amount.
    /// </summary>
    public class Lerp : DmlFunction
    {

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DmlObject Instance = new DmlObject(DmlType.Function, new RotateVectorD());

        /// <summary>
        /// The name of this function.
        /// </summary>
        public override string Name { get { return "RotateVectorD"; } }

        /// <summary>
        /// Whether or not this function is pure.
        /// </summary>
        public override bool IsPure { get { return true; } }

        /// <summary>
        /// Determine whether this function is compatible with a given number of arguments.
        /// </summary>
        public override bool CompatibleWithArgCount(int argCount)
        {
            return argCount == 3;
        }

        /// <summary>
        /// Determine whether this function is compatible with a set of input types.
        /// </summary>
        public override bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return types[0] == DmlType.Vector && types[1] == DmlType.Vector && types[2] == DmlType.Number;
        }

        public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            DmlObject amtObj = stack.Pop();
            DmlObject endObj = stack.Pop();
            DmlObject startObj = stack.Pop();
            
            if (startObj.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 1, DmlType.Vector, startObj.Type);

            if (endObj.Type != DmlType.Vector)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Vector, endObj.Type);

            if (amtObj.Type != DmlType.Number)
                throw DmlSyntaxError.BadArgumentType(Name, 2, DmlType.Number, amtObj.Type);

            double amt = (double)(amtObj.Value);
            Vector2 end = (Vector2)(endObj.Value);
            Vector2 start = (Vector2)(startObj.Value);
            return new DmlObject(DmlType.Vector, Vector2.Lerp(start, end, (float)amt));
        }

        public override DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            double amt = (double)(stack.Pop().Value);
            Vector2 end = (Vector2)(stack.Pop().Value);
            Vector2 start = (Vector2)(stack.Pop().Value);
            return new DmlObject(DmlType.Vector, Vector2.Lerp(start, end, (float)amt));
        }

    }
}
