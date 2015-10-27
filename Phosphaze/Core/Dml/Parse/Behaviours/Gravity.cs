using Microsoft.Xna.Framework;
using Phosphaze.Core.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class Gravity : Behaviour
    {
        /*
         * Gravity | %Direction,
         *           %Weight;
         */
        public Gravity() { }

        public void Initialize(string[] parameters)
        {

        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var weight = (float)(double)(stack.Pop().Value);
            var direction = (Vector2)(stack.Pop().Value);
            var b = (DmlBullet)(bullet.Value);
            b.Direction += weight * direction / 100f;
        }

    }
}
