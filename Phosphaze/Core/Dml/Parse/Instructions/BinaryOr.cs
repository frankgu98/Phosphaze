using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

    /// <summary>
    /// DmlInstruction BinaryOr: logically or the first two items on the stack and push the result.
    /// </summary>
    public class BinaryOr : Instruction
    {

        public static BinaryOr Instance = new BinaryOr();

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            DmlObject second = stack.Pop();
            DmlObject first = stack.Pop();
            // Add the two objects together ya dingus!
        }

        public override string ToString()
        {
            return "||";
        }

    }

}