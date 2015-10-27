using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// This is a meta-instruction that indicates a position for a jump.
    /// </summary>
    public class Label : Instruction
    {

        public string label { get; private set; }

        public Label(string label)
        {
            this.label = label;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            
        }

        public override string ToString()
        {
            return "Label(" + label + ")";
        }
    }
}
