using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class Spawn : Behaviour
    {
        /*
         * Spawn | %BulletType, 
         *         [%Origin], 
         *         [%Direction | %Angle], 
         *         [%Speed],
         *         [%Param]*
         *          
         */
        
        private enum DirectionType { None, Direction, Angle }

        DirectionType direction = DirectionType.None;
        bool hasOrigin = false, hasSpeed = false;
        int paramCount = 0;

        public Spawn() { }

        public void Initialize(string[] parameters)
        {
            if (parameters.Contains("Origin"))
                hasOrigin = true;

            if (parameters.Contains("Direction"))
                direction = DirectionType.Direction;
            else if (parameters.Contains("Angle"))
                direction = DirectionType.Angle;

            if (parameters.Contains("Speed"))
                hasSpeed = true;

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
            string[] paramNames = new string[paramCount];
            DmlObject[] values = new DmlObject[paramCount];

            DmlObject top;
            List<DmlObject> param;

            for (int i = 0; i < paramCount; i++)
            {
                top = stack.Pop();
                param = (List<DmlObject>)top.Value;
                paramNames[i] = (string)(param[0].Value);
                values[i] = param[1];
            }
            
            var speed = 0d;
            var direction = new Vector2(0, 1);
            Vector2 origin;

            if (hasSpeed)
                speed = (double)(stack.Pop().Value);

            switch (this.direction)
            {
                case DirectionType.Direction:
                    direction = (Vector2)(stack.Pop().Value);
                    direction.Normalize();
                    break;
                case DirectionType.Angle:
                    double angle = (double)(stack.Pop().Value);
                    direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    break;
                default:
                    break;
            }

            if (hasOrigin)
                origin = (Vector2)(stack.Pop().Value);
            else
                origin = ((DmlBullet)bullet.Value).Position;

            var type = (DmlBulletFactory)(stack.Pop().Value);

            DmlObject newObj = type.Instantiate(origin, system);
            DmlBullet newBullet = (DmlBullet)newObj.Value;
            if (this.direction != DirectionType.None)
                newBullet.Direction = direction;
            if (hasSpeed)
                newBullet.Speed = speed;

            for (int i = 0; i < paramCount; i++)
                newObj.SetVar(paramNames[i], values[i]);

            if (bullet != null)
                // We have to check if bullet is null because Spawn is one of the few behaviours
                // that can be used in a Timeline. When the Timeline's code is executed, `null`
                // is sent in for `bullet`.
                ((DmlBullet)bullet.Value).Children.Add(newBullet);
            system.AddBullet(newBullet);
        }

    }
}
