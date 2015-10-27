using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction LoadLocal: Push a local variable onto the stack.
    /// </summary>
    public class LoadLocal : Instruction
    {

        public string name { get; private set; }

        public LoadLocal(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            stack.Push(locals[name]);
        }

        public override string ToString()
        {
            return "LoadLocal(" + name + ")";
        }
    }
}
