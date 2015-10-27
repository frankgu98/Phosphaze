using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class MoveTo : Behaviour
    {
        /*
         * TransitionSpeed | %End,
         *                   %Duration,
         *                   [%AbsolutePosition = false];
         */

        private class MoveToComponent : Component
        {

            Vector2 endPos;
            Vector2 posIncrement;
            double endTime;

            public MoveToComponent(Vector2 startPos, Vector2 endPos, Vector2 endPos2, double startTime, double duration)
            {
                endTime = startTime + duration;
                this.endPos = endPos2;
                posIncrement = (endPos - startPos) / (float)duration;
            }

            public override void Update(DmlBullet bullet, DmlSystem system)
            {
                bullet.RelativePosition += posIncrement * (float)(Globals.deltaTime);
                if (bullet.LocalTime >= endTime)
                {
                    Dead = true;
                    bullet.RelativePosition = endPos;
                }
            }
        }

        bool hasAbsolutePosition = false;

        public void Initialize(string[] parameters)
        {
            if (parameters.Contains("AbsolutePosition"))
                hasAbsolutePosition = true;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var absolute = false;
            if (hasAbsolutePosition)
                absolute = (bool)(stack.Pop().Value);
            var duration = (double)(stack.Pop().Value);
            var endPos = (Vector2)(stack.Pop().Value);
            var b = (DmlBullet)bullet.Value;
            if (absolute)
                b.Components.Add(new MoveToComponent(b.Position, endPos, endPos - b.Origin, b.LocalTime, duration));
            else
                b.Components.Add(new MoveToComponent(Vector2.Zero, endPos, b.RelativePosition + endPos, b.LocalTime, duration));
        }

    }
}
