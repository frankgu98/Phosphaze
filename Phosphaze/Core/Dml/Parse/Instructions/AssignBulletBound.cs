using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstruction AssignBulletBound: Pop the top DmlObject on the stack and assign it to a bullet bound name.
    /// </summary>
    public class AssignBulletBound : Instruction
    {

        /// <summary>
        /// The name to assign to.
        /// </summary>
        public string name { get; private set; }

        public AssignBulletBound(string name)
        {
            this.name = name;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system)
        {
            bullet.BoundVars[name] = stack.Pop();
        }

        public override string ToString()
        {
            return "AssignBulletBound(" + name + ")";
        }

    }
}
