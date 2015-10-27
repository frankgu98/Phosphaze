using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinaryMod: mod the first two items on the stack and push the result.
	/// </summary>
	public class BinaryMod : Instruction
	{

		public static BinaryMod Instance = new BinaryMod();

		public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system)
		{
			DmlObject second = stack.Pop();
			DmlObject first = stack.Pop();
            if (first.Type == DmlType.Number && second.Type == DmlType.Number)
                stack.Push(new DmlObject(DmlType.Number, (double)first.Value % (double)second.Value));
            else
                throw DmlSyntaxError.BadBinaryOperandTypes("%", first.Type, second.Type);
		}

        public override string ToString()
        {
            return "%";
        }
	}

}