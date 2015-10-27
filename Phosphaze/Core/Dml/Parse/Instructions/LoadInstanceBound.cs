using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction LoadInstanceBound: Push an instance bound variable onto the stack.
    /// </summary>
    public class LoadInstanceBound : Instruction
    {

        public string name { get; private set; }

        public LoadInstanceBound(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            stack.Push(bullet.BoundVars[name]);
        }

        public override string ToString()
        {
            return "LoadInstanceBound(" + name + ")";
        }

    }
}
