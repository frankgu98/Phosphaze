using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinaryMul: multiply the first two items on the stack and push the result.
	/// </summary>
	public class BinaryMul : Instruction
	{

		public static BinaryMul Instance = new BinaryMul();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();
            switch (first.Type)
            {
                case DmlType.Number:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            stack.Push(new DmlObject(DmlType.Number, (double)first.Value * (double)second.Value));
                            break;
                        case DmlType.Vector:
                            stack.Push(new DmlObject(DmlType.Vector, (float)(double)first.Value * (Vector2)second.Value));
                            break;
                        case DmlType.Colour:
                            stack.Push(new DmlObject(DmlType.Colour, (Color)second.Value * (float)(double)first.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("*", first.Type, second.Type);
                    }
                    break;
                case DmlType.Vector:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            stack.Push(new DmlObject(DmlType.Vector, (Vector2)first.Value * (float)(double)second.Value));
                            break;
                        case DmlType.Vector:
                            stack.Push(new DmlObject(DmlType.Number, Vector2.Dot((Vector2)first.Value, (Vector2)second.Value)));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("*", first.Type, second.Type);
                    }
                    break;
                case DmlType.Colour:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            stack.Push(new DmlObject(DmlType.Colour, (Color)first.Value * (float)(double)second.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("*", first.Type, second.Type);
                    }
                    break;
                default:
                    throw DmlSyntaxError.BadBinaryOperandTypes("*", first.Type, second.Type);
            }
        }

        public override string ToString()
        {
            return "*";
        }
	}

}