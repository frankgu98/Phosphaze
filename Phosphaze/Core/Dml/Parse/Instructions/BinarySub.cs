using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinarySub: subtract the first two items on the stack and push the result.
	/// </summary>
	public class BinarySub : Instruction
	{

		public static BinarySub Instance = new BinarySub();

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
                            stack.Push(new DmlObject(DmlType.Number, (double)first.Value - (double)second.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("-", first.Type, second.Type);
                    }
                    break;
                case DmlType.Vector:
                    switch (second.Type)
                    {
                        case DmlType.Vector:
                            stack.Push(new DmlObject(DmlType.Vector, (Vector2)first.Value - (Vector2)second.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("-", first.Type, second.Type);
                    }
                    break;
                case DmlType.Colour:
                    switch (second.Type)
                    {
                        case DmlType.Colour:
                            Color c1 = (Color)first.Value;
                            Color c2 = (Color)second.Value;
                            stack.Push(new DmlObject(DmlType.Colour, new Color(
                                c1.R - c2.R,
                                c1.G - c2.G,
                                c1.B - c2.B,
                                c1.A - c2.A)
                                ));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("-", first.Type, second.Type);
                    }
                    break;
                default:
                    throw DmlSyntaxError.BadBinaryOperandTypes("-", first.Type, second.Type);
            }
        }

        public override string ToString()
        {
            return "-";
        }
	}

}