using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinaryAdd: add the first two items on the stack and push the result.
	/// </summary>
	public class BinaryAdd : Instruction
	{

		public static BinaryAdd Instance = new BinaryAdd();

		public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system)
		{
			DmlObject second = stack.Pop();
			DmlObject first = stack.Pop();
            if (first.Type == DmlType.String)
                stack.Push(new DmlObject(DmlType.String, (string)first.Value + second.Value.ToString()));
            else if (first.Type == DmlType.List)
            {
                var old = (DmlObject[])first.Value;
                var @new = new DmlObject[old.Length + 1];
                @new[old.Length] = second;
                for (int i = 0; i <= old.Length; i++)
                {
                    @new[i] = old[i];
                }
                stack.Push(new DmlObject(DmlType.List, @new));
            }
            else if (second.Type == DmlType.String)
                stack.Push(new DmlObject(DmlType.String, first.Value.ToString() + (string)second.Value));
            else if (second.Type == DmlType.List)
            {
                var old = (DmlObject[])second.Value;
                var @new = new DmlObject[old.Length + 1];
                @new[0] = first;
                for (int i = 0; i <= old.Length; i++)
                {
                    @new[i + 1] = old[i];
                }
                stack.Push(new DmlObject(DmlType.List, @new));
            }
            else
            {
                switch (first.Type)
                {
                    case DmlType.Number:
                        switch (second.Type)
                        {
                            case DmlType.Number:
                                stack.Push(new DmlObject(DmlType.Number, (double)first.Value + (double)second.Value));
                                break;
                            default:
                                throw DmlSyntaxError.BadBinaryOperandTypes("+", first.Type, second.Type);
                        }
                        break;
                    case DmlType.Vector:
                        switch (second.Type)
                        {
                            case DmlType.Vector:
                                stack.Push(new DmlObject(DmlType.Vector, (Vector2)first.Value + (Vector2)second.Value));
                                break;
                            default:
                                throw DmlSyntaxError.BadBinaryOperandTypes("+", first.Type, second.Type);
                        }
                        break;
                    case DmlType.Colour:
                        switch (second.Type)
                        {
                            case DmlType.Colour:
                                Color c1 = (Color)first.Value;
                                Color c2 = (Color)second.Value;
                                stack.Push(new DmlObject(DmlType.Colour, new Color(
                                    c1.R + c2.R, 
                                    c1.G + c2.G, 
                                    c1.B + c2.B, 
                                    c1.A + c2.A)
                                    ));
                                break;
                            default:
                                throw DmlSyntaxError.BadBinaryOperandTypes("+", first.Type, second.Type);
                        }
                        break;
                    default:
                        throw DmlSyntaxError.BadBinaryOperandTypes("+", first.Type, second.Type);
                }
            }
		}

        public override string ToString()
        {
            return "+";
        }

	}

}