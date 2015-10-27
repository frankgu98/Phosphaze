using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction LoadConstant: Add a constant DmlObject to the top of the stack.
    /// </summary>
    public class LoadConstant : Instruction
    {

        public DmlObject constant { get; private set; }

        public LoadConstant(DmlObject constant)
        {
            this.constant = constant;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            stack.Push(constant);
        }

        public override string ToString()
        {
            return "LoadConstant(" + (constant == null ? constant : constant.Value) + ")";
        }
    }
}
