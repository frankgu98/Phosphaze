using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    public class AssignIntrinsicBulletProperty : Instruction
    {

        public static AssignIntrinsicBulletProperty Direction = new AssignIntrinsicBulletProperty(DmlTokens.INTRINSIC_DIRECTION);
        public static AssignIntrinsicBulletProperty Speed = new AssignIntrinsicBulletProperty(DmlTokens.INTRINSIC_SPEED);
        public static AssignIntrinsicBulletProperty Colour = new AssignIntrinsicBulletProperty(DmlTokens.INTRINSIC_COLOUR);
        public static AssignIntrinsicBulletProperty Sprite = new AssignIntrinsicBulletProperty(DmlTokens.INTRINSIC_SPRITE);

        string property;

        private AssignIntrinsicBulletProperty(string property)
        {
            this.property = property;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            switch (property)
            {
                case DmlTokens.INTRINSIC_DIRECTION:
                    ((DmlBullet)bullet.Value).Direction = (Vector2)(stack.Pop().Value);
                    break;
                case DmlTokens.INTRINSIC_SPEED:
                    ((DmlBullet)bullet.Value).Speed = (double)(stack.Pop().Value);
                    break;
                case DmlTokens.INTRINSIC_COLOUR:
                    ((DmlBullet)bullet.Value).Colour = (Color)(stack.Pop().Value);
                    break;
                case DmlTokens.INTRINSIC_SPRITE:
                    string name = (string)(stack.Pop().Value);
                    ((DmlBullet)bullet.Value).SetSprite(name.Substring(1, name.Length - 2));
                    break;
                default:
                    throw new DmlSyntaxError(String.Format("Unknown intrinsic property \"{0}\"", property));
            }
        }

    }
}
