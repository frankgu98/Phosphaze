using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction AssignGlobal: Pop the top DmlObject on the stack and assign it to a global name.
    /// </summary>
    public class AssignGlobal : Instruction
    {

        /// <summary>
        /// The name to assign to.
        /// </summary>
        public string name { get; private set; }

        public AssignGlobal(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system)
        {
            system.GlobalVars.Add(name, stack.Pop());
        }

        public override string ToString()
        {
            return "AssignGlobal(" + name + ")";
        }

    }
}
