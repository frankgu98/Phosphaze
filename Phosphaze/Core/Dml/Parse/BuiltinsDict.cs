using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Phosphaze.Core.Dml.Builtins;

namespace Phosphaze.Core.Dml.Parse
{
    /// <summary>
    /// A list of all the builtins available in Dml.
    /// </summary>
    public static class BuiltinsDict
    {

        /// <summary>
        /// The dictionary of builtin DmlObjects.
        /// </summary>
        public static Dictionary<string, DmlObject> Builtins = new Dictionary<string, DmlObject>()
        {
            // Basic functions
            {"ConsoleOutput",         ConsoleOutput.Instance},
            {"Vector",                Vector.Instance},
            {"Array",                 Array.Instance},
            // Time Commands
            {"At",                    At.Instance},
            {"Before",                Before.Instance},
            {"After",                 After.Instance},
            {"From",                  From.Instance},
            {"Outside",               Outside.Instance},
            {"AtIntervals",           AtIntervals.Instance},
            {"DuringIntervals",       DuringIntervals.Instance},
            {"AtGlobal",              AtGlobal.Instance},
            {"BeforeGlobal",          BeforeGlobal.Instance},
            {"AfterGlobal",           AfterGlobal.Instance},
            {"FromGlobal",            FromGlobal.Instance},
            {"OutsideGlobal",         OutsideGlobal.Instance},
            {"AtIntervalsGlobal",     AtIntervalsGlobal.Instance},
            {"DuringIntervalsGlobal", DuringIntervalsGlobal.Instance},
            // Basic math
            {"Sign",                  Sign.Instance},
            {"Floor",                 Floor.Instance},
            {"Ceil",                  Ceil.Instance},
            {"Max",                   Max.Instance},
            {"Min",                   Min.Instance},
            {"Log",                   Log.Instance},
            {"LogB",                  LogB.Instance},
            {"Exp",                   Exp.Instance},
            {"Factorial",             Factorial.Instance},
            {"Random",                Random.Instance},
            // Trig functions (radians)
            {"Sin",                   UnaryMath.Sin}, 
            {"Cos",                   UnaryMath.Cos}, 
            {"Tan",                   UnaryMath.Tan}, 
            {"Sec",                   UnaryMath.Sec}, 
            {"Csc",                   UnaryMath.Csc}, 
            {"Cot",                   UnaryMath.Cot}, 
            // Trig functions (degrees)
            {"SinD",                  UnaryMath.SinD},
            {"CosD",                  UnaryMath.CosD},
            {"TanD",                  UnaryMath.TanD},
            {"SecD",                  UnaryMath.SecD},
            {"CscD",                  UnaryMath.CscD},
            {"CotD",                  UnaryMath.CotD},
            // Inverse trig functions (radians)
            {"Arcsin",                UnaryMath.Arcsin},
            {"Arccos",                UnaryMath.Arccos},
            {"Arctan",                UnaryMath.Arctan},
            {"Arcsec",                UnaryMath.Arcsec},
            {"Arccsc",                UnaryMath.Arccsc},
            {"Arccot",                UnaryMath.Arccot},
            {"Atan2",                 Atan2.Instance},
            // Inverse trig functions (degrees)
            {"ArcsinD",               UnaryMath.ArcsinD},
            {"ArccosD",               UnaryMath.ArccosD},
            {"ArctanD",               UnaryMath.ArctanD},
            {"ArcsecD",               UnaryMath.ArcsecD},
            {"ArcscsD",               UnaryMath.ArccscD},
            {"ArccotD",               UnaryMath.ArccotD},
            {"Atan2D",                Atan2D.Instance},
            // Hyperbolic trig functions
            {"Sinh",                  UnaryMath.Sinh},
            {"Cosh",                  UnaryMath.Cosh},
            {"Tanh",                  UnaryMath.Tanh},
            {"Sech",                  UnaryMath.Sech},
            {"Csch",                  UnaryMath.Csch},
            {"Coth",                  UnaryMath.Coth},
            // Inverse hyperbolic trig functions
            {"Arsinh",                UnaryMath.Arsinh},
            {"Arcosh",                UnaryMath.Arcosh},
            {"Artanh",                UnaryMath.Artanh},
            {"Arsech",                UnaryMath.Arsech},
            {"Arcsch",                UnaryMath.Arcsch},
            {"Arcoth",                UnaryMath.Arcoth},
            // Special math functions
            {"Sinc",                  UnaryMath.Sinc},
            {"Tanhc",                 UnaryMath.Tanhc},
            {"Erf",                   UnaryMath.Erf},
            {"Gudermannian",          UnaryMath.Gudermannian},
            {"InverseGudermannian",   UnaryMath.InverseGudermannian},
            {"Si",                    null}, // Sine integral
            {"Ci",                    null}, // Cosine integral
            {"Gamma",                 UnaryMath.Gamma},
            {"Digamma",               null},
            {"LambertW",              null},
            {"Beta",                  null},
            {"NormalizedBeta",        null},
            {"IncompleteBeta",        null},
            {"AiryA",                 UnaryMath.AiryA},
            {"AiryB",                 UnaryMath.AiryB},
            {"BesselJ0",              UnaryMath.BesselJ0},
            {"BesselJ1",              UnaryMath.BesselJ1},
            {"BesselJ",               BesselJ.Instance},
            {"BesselY0",              UnaryMath.BesselY0},
            {"BesselY1",              UnaryMath.BesselY1},
            {"BesselY",               BesselY.Instance},
            // Periodic functions.
            {"TriangleWave",          null},
            {"SawtoothWave",          null},
            {"SquareWave",            SquareWave.Instance},
            {"RectWave",              null},
            {"Clausen",               null},
            // Vector functions
            {"LeftNormal",            LeftNormal.Instance},
            {"RightNormal",           RightNormal.Instance},
            {"Normalized",            Normalized.Instance},
            {"Magnitude",             Magnitude.Instance},
            {"MagnitudeSqrd",         MagnitudeSqrd.Instance},
            {"AngleOf",               AngleOf.Instance},
            {"AngleOfD",              AngleOfD.Instance},
            {"Polar",                 Polar.Instance},
            {"PolarD",                PolarD.Instance},
            {"RotateVector",          RotateVector.Instance},
            {"RotateVectorD",         RotateVectorD.Instance},
            {"Lerp",                  Lerp.Instance},
            // Math constants
            {"Pi",                    new DmlObject(DmlType.Number, 3.141592653589793)},
            {"E",                     new DmlObject(DmlType.Number, 2.718281828459045)},
            {"Infinity",              new DmlObject(DmlType.Number, double.PositiveInfinity)},
            // Vector constants
            {"ZeroVector",                new DmlObject(DmlType.Vector, Vector2.Zero)},
            {"Up",                    new DmlObject(DmlType.Vector, new Vector2(0, -1))},
            {"Down",                  new DmlObject(DmlType.Vector, new Vector2(0, 1))},
            {"Left",                  new DmlObject(DmlType.Vector, new Vector2(-1, 0))},
            {"Right",                 new DmlObject(DmlType.Vector, new Vector2(1, 0))},
            {"Ones",                  new DmlObject(DmlType.Vector, new Vector2(1, 1))},
            {"ScreenCenter",          new DmlObject(DmlType.Vector, new Vector2(
                  Options.Resolutions.X/2f, Options.Resolutions.Y/2f))},
            // Behaviour Options
            {"UniformDistribution",  DmlObject.DmlOptionFactory.Instantiate(
                "UniformDistrubtion", new string[] {"a", "b"}, new DmlType[] {DmlType.Number, DmlType.Number})}
        };

    }
}
