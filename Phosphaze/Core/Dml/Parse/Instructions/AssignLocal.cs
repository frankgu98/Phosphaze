using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction AssignLocal: Pop the top DmlObject on the stack and assign it to a local name.
    /// </summary>
    public class AssignLocal : Instruction
    {

        /// <summary>
        /// The name to assign to.
        /// </summary>
        public string name { get; private set; }

        public AssignLocal(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            locals[name] = stack.Pop();
        }

        public override string ToString()
        {
            return "AssignLocal(" + name + ")";
        }

    }
}
