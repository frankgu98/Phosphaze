using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

	/// <summary>
	/// DmlInstruction BinaryPow: Exponentiate the first two items on the stack and push the result.
	/// </summary>
	public class BinaryPow : Instruction
	{

		public static BinaryPow Instance = new BinaryPow();

		public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();
            if (first.Type == DmlType.Number && second.Type == DmlType.Number)
                stack.Push(new DmlObject(DmlType.Number, Math.Pow((double)first.Value, (double)second.Value)));
            else
                throw DmlSyntaxError.BadBinaryOperandTypes("^", first.Type, second.Type);
        }

        public override string ToString()
        {
            return "^";
        }

	}

}