using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstructino UnaryNeg: negate the top item on the stack..
    /// </summary>
    public class UnaryNeg : Instruction
    {

        public static UnaryNeg Instance = new UnaryNeg();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type == DmlType.Number)
                stack.Push(new DmlObject(DmlType.Number, -(double)top.Value));
            else if (top.Type == DmlType.Vector)
                stack.Push(new DmlObject(DmlType.Vector, -(Vector2)top.Value));
            else
                throw DmlSyntaxError.BadUnaryOperandType("-", top.Type);
        }

        public override string ToString()
        {
            return "u-";
        }
    }
}
