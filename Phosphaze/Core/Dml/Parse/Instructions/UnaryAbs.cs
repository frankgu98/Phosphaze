using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstructino UnaryAbs: take the absolute value of the top item on the stack.
    /// </summary>
    public class UnaryAbs : Instruction
    {

        public static UnaryAbs Instance = new UnaryAbs();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type == DmlType.Number)
                stack.Push(new DmlObject(DmlType.Number, Math.Abs((double)top.Value)));
            else
                throw DmlSyntaxError.BadUnaryOperandType("~", top.Type);
        }

        public override string ToString()
        {
            return "u~";
        }

    }
}
