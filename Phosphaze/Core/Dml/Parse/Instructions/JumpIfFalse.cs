using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

    public class JumpIfFalse : Instruction
    {

        public string labelName;

        public int position;

        public JumpIfFalse(string name)
        {
            labelName = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            if (!(bool)(stack.Pop().Value))
                block.currentPosition = position;
        }

        public override string ToString()
        {
            return "JumpIfFalse(" + position + ")";
        }
    }
}
