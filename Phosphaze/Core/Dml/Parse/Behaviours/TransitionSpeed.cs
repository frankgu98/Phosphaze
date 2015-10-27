using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class TransitionSpeed : Behaviour
    {
        /*
         * TransitionSpeed | %End,
         *                   %Duration;
         */

        private class TransitionSpeedComponent : Component
        {

            double speedIncrement;
            double finalSpeed;
            double endTime;

            public TransitionSpeedComponent(double startSpeed, double endSpeed, double startTime, double duration)
            {
                finalSpeed = endSpeed;
                endTime = startTime + duration;
                speedIncrement = (endSpeed - startSpeed) / duration;
            }

            public override void Update(DmlBullet bullet, DmlSystem system)
            {
                bullet.Speed += speedIncrement * Globals.deltaTime;
                if (bullet.LocalTime >= endTime)
                {
                    Dead = true;
                    bullet.Speed = finalSpeed;
                }
            }
        }

        public void Initialize(string[] parameters)
        {

        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var duration = (double)(stack.Pop().Value);
            var endSpeed = (double)(stack.Pop().Value);
            var b = (DmlBullet)bullet.Value;
            b.Components.Add(new TransitionSpeedComponent(b.Speed, endSpeed, b.LocalTime, duration));
        }

    }
}
