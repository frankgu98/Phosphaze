using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Instructions
{

    public class BinaryLtEq : Instruction
    {

        public static BinaryLtEq Instance = new BinaryLtEq();

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
            return "<=";
        }

    }

}