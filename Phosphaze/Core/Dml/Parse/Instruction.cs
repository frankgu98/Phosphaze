using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse
{
    public interface Instruction
    {

        void Perform(
            CodeBlock block,
            Stack<DmlObject> stack, 
            Dictionary<string, DmlObject> locals, 
            DmlObject bullet, DmlSystem system);

    }
}
