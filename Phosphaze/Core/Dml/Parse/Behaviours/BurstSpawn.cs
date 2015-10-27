using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{

	public class BurstSpawn : Behaviour
	{

		/*
		 * BurstSpawn | %BulletType,
         *              %Amount,
         * 				%SpeedRange,
		 * 				[%AngleRange | %AngleRangeD]
         * 				[%Origin];
		 *
		 */

		public BurstSpawn() { }

        bool hasAngleRange = false;
        bool hasOrigin = false;
        bool degrees = false;

		public void Initialize(string[] parameters)
		{
            if (parameters.Contains("AngleRangeD"))
            {
                degrees = true;
                hasAngleRange = true;
            }
            if (parameters.Contains("AngleRange"))
            {
                if (degrees)
                    throw new BehaviourException("BurstSpawn cannot have both AngleRange and AngleRangeD defined.");
                hasAngleRange = true;
            }
            if (parameters.Contains("Origin"))
                hasOrigin = true;
		}

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var local = bullet != null;
            DmlBullet parent = null;
            Vector2 origin = Vector2.Zero;
            if (local)
            {
                parent = (DmlBullet)(bullet.Value);
                origin = parent.Position;
            }
            if (hasOrigin)
                origin = (Vector2)(stack.Pop().Value);


            double angleMin = 0, angleMax = 2 * Math.PI, speedMin, speedMax;
            if (hasAngleRange)
            {
                var angleRange = (List<DmlObject>)(stack.Pop().Value);
                angleMin = (double)angleRange[0].Value;
                angleMax = (double)angleRange[1].Value;
                if (degrees)
                {
                    angleMin *= Math.PI / 180;
                    angleMax *= Math.PI / 180;
                }
            }
            var speedRange = (List<DmlObject>)(stack.Pop().Value);
            speedMin = (double)speedRange[0].Value;
            speedMax = (double)speedRange[1].Value;

            var count = (double)(stack.Pop().Value);
            var factory = (DmlBulletFactory)(stack.Pop().Value);
            
            double crntAngle, crntSpeed;
            Vector2 direction;
            DmlObject newObj;
            DmlBullet newBullet;

            for (int i = 0; i < count; i++)
            {
                crntAngle = (angleMax - angleMin) * Globals.randGen.NextDouble() + angleMin;
                crntSpeed = (speedMax - speedMin) * Globals.randGen.NextDouble() + speedMin;
                direction = new Vector2((float)Math.Cos(crntAngle), (float)Math.Sin(crntAngle));

                newObj = factory.Instantiate(origin, system);
                newBullet = (DmlBullet)(newObj.Value);
                newBullet.Direction = direction;
                newBullet.Speed = crntSpeed;

                if (local)
                    parent.Children.Add(newBullet);
                system.AddBullet(newBullet);
            }

        }

	}

}