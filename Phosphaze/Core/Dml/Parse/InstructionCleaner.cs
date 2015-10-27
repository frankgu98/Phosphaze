using Phosphaze.Core.Dml.Parse.Instructions;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse
{
    public class InstructionCleaner
    {

        List<Instruction> newInstructions = new List<Instruction>();
        Instruction[] oldInstructions;

        public InstructionCleaner(Instruction[] instructions)
        {
            oldInstructions = instructions;
        }

        public Instruction[] Clean()
        {
            FoldLabels();
            return newInstructions.ToArray();
        }

        private void FoldLabels()
        {
            for (int i = 0; i < oldInstructions.Length; i++)
            {
                if (oldInstructions[i] is JumpIfLessThanOrEqual)
                {
                    JumpIfLessThanOrEqual instr = (JumpIfLessThanOrEqual)oldInstructions[i];
                    string label = instr.labelName;
                    int index = new List<Instruction>(oldInstructions).FindIndex(
                        instruction => (instruction as Label) != null &&
                                       (instruction as Label).label == label);
                    instr.position = index;
                    newInstructions.Add(instr);
                }
                else if (oldInstructions[i] is JumpIfFalse)
                {
                    JumpIfFalse instr = (JumpIfFalse)oldInstructions[i];
                    string label = instr.labelName;
                    int index = new List<Instruction>(oldInstructions).FindIndex(
                        instruction => (instruction as Label) != null &&
                                       (instruction as Label).label == label);
                    instr.position = index;
                    newInstructions.Add(instr);
                }
                else
                    newInstructions.Add(oldInstructions[i]);
            }
        }

    }
}
