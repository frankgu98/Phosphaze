using Microsoft.Xna.Framework;
using Phosphaze.Core.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class RotateAround : Behaviour
    {

        /*
         * RotateAround | %Point,
         *                %Angle | %AngleD
         */

        public RotateAround() { }

        bool degrees = false;

        public void Initialize(string[] parameters)
        {
            if (!parameters.Contains("Angle") && !parameters.Contains("AngleD"))
                throw new BehaviourException("RotateAround must have one of Angle or AngleD defined.");
            if (parameters.Contains("AngleD"))
                degrees = true;
            if (parameters.Contains("Angle"))
            {
                if (degrees)
                    throw new BehaviourException("RotateAround cannot have both Angle and AngleD defined."); ;
                degrees = false;
            }
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var b = (DmlBullet)(bullet.Value);
            var amt = (double)(stack.Pop().Value);
            var pnt = (Vector2)(stack.Pop().Value);
            b.Position = GeometryUtils.RotateVector(b.Position, pnt, amt, degrees: degrees);
        }

    }
}
