using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class RadialSpawn : Behaviour
    {   
        /*
         * RadialSpawn | %BulletType,
         *               %Streams,
         *               %Speed,
         *               [%AngleOffset],
         *               [%Param]*
         */

        public RadialSpawn() { }

        bool hasAngleOffset = false, hasAngleOffsetD = false;
        int paramCount = 0;

        public void Initialize(string[] parameters)
        {
            if (parameters.Contains("AngleOffset"))
                hasAngleOffset = true;
            if (parameters.Contains("AngleOffsetD"))
                hasAngleOffsetD = true;

            if (hasAngleOffset && hasAngleOffsetD)
                throw new BehaviourException("RadialSpawn cannot have both AngleOffset and AngleOffsetD defined.");

            foreach (string param in parameters)
                if (param == "Param")
                    paramCount++;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            var paramNames = new string[paramCount];
            var values = new DmlObject[paramCount];

            DmlObject top;
            List<DmlObject> param;

            for (int i = 0; i < paramCount; i++)
            {
                top = stack.Pop();
                param = (List<DmlObject>)top.Value;
                paramNames[i] = (string)(param[0].Value);
                values[i] = param[1];
            }

            var currentAngle = 0d;
            if (hasAngleOffset)
                currentAngle = (double)(stack.Pop().Value);
            if (hasAngleOffsetD)
                currentAngle = Math.PI / 180 * (double)(stack.Pop().Value);

            var speed = (double)(stack.Pop().Value);
            var streams = (int)(double)(stack.Pop().Value);
            var factory = (DmlBulletFactory)(stack.Pop().Value);
            var oldBullet = (DmlBullet)(bullet.Value);

            var angleIncrement = 2*Math.PI/streams;

            DmlObject newBulletObj;
            DmlBullet newBullet;
            Vector2 direction;

            bool local = bullet != null;

            for (int i = 0; i < streams; i++)
            {
                newBulletObj = factory.Instantiate(oldBullet.Origin, system);
                newBullet = (DmlBullet)(newBulletObj.Value);
                direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                newBullet.Direction = direction;
                newBullet.Speed = speed;

                for (int j = 0; j < paramCount; j++)
                    newBulletObj.SetVar(paramNames[j], values[j]);

                if (local)
                    oldBullet.Children.Add(newBullet);
                system.AddBullet(newBullet);

                currentAngle += angleIncrement;
            }
        }

    }
}
