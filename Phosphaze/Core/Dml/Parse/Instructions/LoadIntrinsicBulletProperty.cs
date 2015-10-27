using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    public class LoadIntrinsicBulletProperty : Instruction
    {

        public static LoadIntrinsicBulletProperty Origin    = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_ORIGIN);
        public static LoadIntrinsicBulletProperty Position  = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_POSITION);
        public static LoadIntrinsicBulletProperty Direction = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_DIRECTION);
        public static LoadIntrinsicBulletProperty Speed     = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_SPEED);
        public static LoadIntrinsicBulletProperty Velocity  = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_VELOCITY);
        public static LoadIntrinsicBulletProperty Colour    = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_COLOUR);
        public static LoadIntrinsicBulletProperty Time      = new LoadIntrinsicBulletProperty(DmlTokens.INTRINSIC_TIME);

        string property;

        private LoadIntrinsicBulletProperty(string property)
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
                case DmlTokens.INTRINSIC_ORIGIN:
                    stack.Push(new DmlObject(DmlType.Vector, ((DmlBullet)bullet.Value).Origin));
                    break;
                case DmlTokens.INTRINSIC_POSITION:
                    stack.Push(new DmlObject(DmlType.Vector, ((DmlBullet)bullet.Value).Position));
                    break;
                case DmlTokens.INTRINSIC_DIRECTION:
                    stack.Push(new DmlObject(DmlType.Vector, ((DmlBullet)bullet.Value).Direction));
                    break;
                case DmlTokens.INTRINSIC_SPEED:
                    stack.Push(new DmlObject(DmlType.Vector, ((DmlBullet)bullet.Value).Speed));
                    break;
                case DmlTokens.INTRINSIC_COLOUR:
                    stack.Push(new DmlObject(DmlType.Colour, ((DmlBullet)bullet.Value).Colour));
                    break;
                case DmlTokens.INTRINSIC_TIME:
                    stack.Push(new DmlObject(DmlType.Number, ((DmlBullet)bullet.Value).LocalTime));
                    break;
                case DmlTokens.INTRINSIC_VELOCITY:
                    DmlBullet b = (DmlBullet)bullet.Value;
                    stack.Push(new DmlObject(DmlType.Vector, b.Direction * (float)b.Speed));
                    break;
                default:
                    throw new DmlSyntaxError(String.Format("Unknown bullet property \"{0}\"", property));
            }
        }

    }
}
