using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse 
{

	public class CodeBlock 
	{

		// The commands this code block executes.
		private Instruction[] Instructions;

        public int currentPosition = 0;

		public CodeBlock(Instruction[] instructions)
		{
			Instructions = new InstructionCleaner(instructions).Clean();
            //Instructions = instructions;
		}

		public DmlObject Evaluate(DmlObject bullet, DmlSystem system)
		{
			// The command stack.
			Stack<DmlObject> stack = new Stack<DmlObject>();

			// The variables local to this code block.
			Dictionary<string, DmlObject> locals = new Dictionary<string, DmlObject>();

            while (currentPosition < Instructions.Length)
                Instructions[currentPosition++].Perform(this, stack, locals, bullet, system);
            currentPosition = 0;
            return stack.Count == 0 ? null : stack.Pop();
		}

        public void Execute(DmlObject bullet, DmlSystem system)
        {
            // The command stack.
            Stack<DmlObject> stack = new Stack<DmlObject>();

            // The variables local to this code block.
            Dictionary<string, DmlObject> locals = new Dictionary<string, DmlObject>();

            while (currentPosition < Instructions.Length)
                Instructions[currentPosition++].Perform(this, stack, locals, bullet, system);
            currentPosition = 0;
        }


	}

}