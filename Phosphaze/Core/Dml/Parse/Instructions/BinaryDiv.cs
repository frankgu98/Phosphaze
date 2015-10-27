using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinaryDiv: divide the first two items on the stack and push the result.
	/// </summary>
	public class BinaryDiv : Instruction
	{

		public static BinaryDiv Instance = new BinaryDiv();

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
                            stack.Push(new DmlObject(DmlType.Number, (double)first.Value / (double)second.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("/", first.Type, second.Type);
                    }
                    break;
                case DmlType.Vector:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            stack.Push(new DmlObject(DmlType.Vector, (Vector2)first.Value / (float)second.Value));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("/", first.Type, second.Type);
                    }
                    break;
                case DmlType.Colour:
                    switch (second.Type)
                    {
                        case DmlType.Number:
                            stack.Push(new DmlObject(DmlType.Colour, (Color)first.Value * (1/(float)second.Value)));
                            break;
                        default:
                            throw DmlSyntaxError.BadBinaryOperandTypes("/", first.Type, second.Type);
                    }
                    break;
                default:
                    throw DmlSyntaxError.BadBinaryOperandTypes("/", first.Type, second.Type);
            }
		}

        public override string ToString()
        {
            return "/";
        }
	}

}