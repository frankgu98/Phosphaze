using Phosphaze.Core.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class RotateDirection : Behaviour
    {

        public RotateDirection() { }

        bool degrees = false;

        public void Initialize(string[] parameters)
        {
            if (parameters.Contains("AngleD"))
                degrees = true;
            if (parameters.Contains("Angle"))
            {
                if (degrees)
                    throw new BehaviourException("RotateDirection cannot have both Angle and AngleD defined."); ;
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
            b.Direction = GeometryUtils.RotateVector(b.Direction, (double)(stack.Pop().Value), degrees: degrees);
        }

    }
}
