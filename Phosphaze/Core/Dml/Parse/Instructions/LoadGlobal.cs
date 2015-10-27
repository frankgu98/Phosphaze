using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction LoadGlobal: Push a global variable onto the stack.
    /// </summary>
    public class LoadGlobal : Instruction
    {

        public string name { get; private set; }

        public LoadGlobal(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            stack.Push(system.GlobalVars[name]);
        }

        public override string ToString()
        {
            return "LoadGlobal(" + name + ")";
        }
    }
}
