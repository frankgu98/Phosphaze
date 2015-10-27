using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

    public class JumpIfLessThanOrEqual : Instruction
    {
        public string labelName;

        public int position;

        public JumpIfLessThanOrEqual(string name)
        {
            labelName = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            double second = (double)(stack.Pop().Value);
            double first = (double)(stack.Pop().Value);
            if (first <= second)
                block.currentPosition = position;
        }

        public override string ToString()
        {
            return "JumpIfLtEq(" + position + ")";
        }
    }
}
