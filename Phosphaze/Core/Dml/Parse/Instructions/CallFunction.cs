using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{
    public class CallFunction : Instruction
    {

        public int argCount { get; private set; }

        public CallFunction(int argCount)
        {
            this.argCount = argCount;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject top = stack.Pop();
            if (top.Type != DmlType.Function)
                throw new DmlSyntaxError("Error: attempt to call uncallable object.");
            DmlFunction func = (DmlFunction)(top.Value);
            stack.Push(func.CallDynamic(argCount, stack, bullet, system));
        }

        public override string ToString()
        {
            return "CallFunction(" + argCount + ")";
        }
    }
}
