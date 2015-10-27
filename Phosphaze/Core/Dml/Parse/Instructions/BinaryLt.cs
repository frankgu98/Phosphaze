using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

    public class BinaryLt : Instruction
    {

        public static BinaryLt Instance = new BinaryLt();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();
            if (first.Type != DmlType.Number || second.Type != DmlType.Number)
                throw DmlSyntaxError.BadBinaryOperandTypes("<", first.Type, second.Type);
            stack.Push(new DmlObject(DmlType.Bool, (double)(first.Value) < (double)(second.Value)));
        }

        public override string ToString()
        {
            return "<";
        }

    }

}