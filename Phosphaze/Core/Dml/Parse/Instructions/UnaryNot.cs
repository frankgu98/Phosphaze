using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    /// <summary>
    /// DmlInstructino UnaryNot: logically negate the top item on the stack.
    /// </summary>
    public class UnaryNot : Instruction
    {

        public static UnaryNot Instance = new UnaryNot();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            // Literally do nothing.  
        }

        public override string ToString()
        {
            return "!";
        }
    }
}
