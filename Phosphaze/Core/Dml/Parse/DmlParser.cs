using Phosphaze.Core.Dml.Parse.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse
{
    /// <summary>
    /// The DmlParser is an abstract base class derived from AbstractParser that implements
    /// methods specific to parsing different types of statements in DML. This unifies the
    /// code that turns DML into Instruction objects into one single inheritable interface.
    /// </summary>
    public abstract class DmlParser : AbstractParser
    {

        /// <summary>
        /// DmlParser subclasses always have a list of Instructions.
        /// </summary>
        protected List<Instruction> Instructions = new List<Instruction>();

        public DmlParser(string[] tokens, int currentLine) : base(tokens, currentLine) { }

        /// <summary>
        /// Run a simple check on a list of instructions to check if it evaluates to a constant
        /// value.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsSimpleConstant(Instruction[] expression)
        {
            // First pass, check basic instructions.
            foreach (Instruction instruction in expression)
            {
                // These are "constant" instructions which can be evaluated without any 
                // context (no locals, globals, instance, or system required).
                if (
                    !(instruction is LoadConstant) &&
                    !(instruction is CallFunction) &&
                    !(instruction is BinaryAdd) &&
                    !(instruction is BinaryDiv) &&
                    !(instruction is BinaryMod) &&
                    !(instruction is BinaryMul) &&
                    !(instruction is BinaryPow) &&
                    !(instruction is BinarySub) &&
                    !(instruction is UnaryAbs) &&
                    !(instruction is UnaryNeg)
                    )
                    return false;
            }
            // Second pass, check for pure functions.
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] is CallFunction)
                {
                    var previous = ((LoadConstant)expression[i - 1]).constant;
                    if (!((DmlFunction)previous.Value).IsPure)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Process an array of tokens as an expression and append the instructions to a list.
        /// </summary>
        /// <param name="tokens"></param>
        protected void ProcessExpression(string[] tokens)
        {
            ExpressionBuilder eb = new ExpressionBuilder(tokens, currentLine);
            eb.Parse();
            Instruction[] expression_instructions = (Instruction[])eb.GetResult();
            foreach (Instruction instruction in expression_instructions)
                Instructions.Add(instruction);
        }

        /// <summary>
        /// Process a generic `Assign` statement.
        /// </summary>
        /// <param name="nsType"></param>
        protected void ProcessAssignment(NamespaceType nsType)
        {
            DmlSyntaxError badAssign = DmlSyntaxError.BadAssignmentStatement();
            Advance(exception: badAssign);

            Instruction assigner; // The instruction that performs the assignment.

            // Check for a global identifier.
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.AT))
            {
                Advance(exception: badAssign);
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.UQNAME))
                    assigner = new AssignGlobal(CurrentToken);
                else
                    throw DmlSyntaxError.BadGlobalName(CurrentToken);
            }
            // Check for an instance bound indentifier.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.DOLLAR))
            {
                switch (nsType)
                {
                    case NamespaceType.BulletInit:
                    case NamespaceType.BulletUpdate:
                        Advance(exception: badAssign);
                        if (DmlTokens.IsName(CurrentToken))
                        {
                            switch (CurrentToken)
                            {
                                // Assign directly to the bullet direction.
                                case DmlTokens.INTRINSIC_DIRECTION:
                                    assigner = AssignIntrinsicBulletProperty.Direction;
                                    break;
                                // Assign directly to the bullet speed.
                                case DmlTokens.INTRINSIC_SPEED:
                                    assigner = AssignIntrinsicBulletProperty.Speed;
                                    break;
                                // Assign directly to the bullet colour.
                                case DmlTokens.INTRINSIC_COLOUR:
                                    assigner = AssignIntrinsicBulletProperty.Colour;
                                    break;
                                case DmlTokens.INTRINSIC_SPRITE:
                                    assigner = AssignIntrinsicBulletProperty.Sprite;
                                    break;
                                default:
                                    assigner = new AssignBulletBound(CurrentToken);
                                    break;
                            }
                        }
                        else
                            throw DmlSyntaxError.BadVariableName(CurrentToken);
                        break;
                    default:
                        throw DmlSyntaxError.BadAssignmentNamespace();
                }
                
            }
            // Check for an ordinary local name.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.UQNAME))
                assigner = new AssignLocal(CurrentToken);
            else
            {
                throw DmlSyntaxError.BadVariableName(CurrentToken);
            }

            // Read the expression.
            Advance(exception: badAssign);
            string[] expression_tokens = GetUntil(DmlTokens.EOL);
            ProcessExpression(expression_tokens);
            Instructions.Add(assigner);
        }

        /// <summary>
        /// Parse an "inline-able" range statement.
        /// 
        /// An "inline-able" statement is one where we can infer the values of
        /// the start, end, and step as compile-time constants, and then inline
        /// the statements inside in the range block. 
        /// 
        /// This can make certain simple range statements faster since it requires
        /// less operations than a jump statement and a label instruction (which is
        /// the procedure for range statements that aren't simple).
        /// </summary>
        private void InlineRange(string loopVar, double start, double end, double step)
        {
            // Make a copy of the current instructions and clear the original instructions.
            // This will just make our life easier when we get to copying the instructions in
            // the range block into the previous instructions explicitly.
            List<Instruction> TempInstructions = new List<Instruction>(Instructions); 
            Instructions = new List<Instruction>();

            int angleDepth = 0;

            while (!Done)
            {
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LANGLE))
                    angleDepth++;
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.RANGLE))
                {
                    if (angleDepth == 0)
                        break;
                    angleDepth--;
                }
                ParseNext();
            }

            double i = start;
            while (i <= end)
            {
                foreach (Instruction instruction in Instructions)
                {
                    if (instruction is LoadLocal && ((LoadLocal)instruction).name == loopVar)
                        TempInstructions.Add(new LoadConstant(new DmlObject(DmlType.Number, i)));
                    else
                        TempInstructions.Add(instruction);
                }
                i += step;
            }
            // Reset the actual instructions to the temporary instructions now that we're done
            // copying over the range block instructions.
            Instructions = TempInstructions;
        }

        /// <summary>
        /// Parse a complex range statement.
        /// </summary>
        private void ComplexRange(
            string loopVariable,
            Instruction[] startInstructions, 
            Instruction[] endInstructions, 
            Instruction[] stepInstructions)
        {
            // Generate a random name for the loop label.
            string loopName = String.Format("#{0:X6}", Globals.randGen.Next(0x1000000));
            // These names are just "private" names that the executer uses but the user
            // can never use (as it will always evaluate to a comment in the Tokenizer).
            string endName = "#" + loopName + "_end";
            string stepName = "#" + loopName + "_step";

            // Add the instructions for the start, end, and step respectively.
            foreach (Instruction instruction in startInstructions)
                Instructions.Add(instruction);
            Instructions.Add(new AssignLocal(loopVariable));
            foreach (Instruction instruction in endInstructions)
                Instructions.Add(instruction);
            Instructions.Add(new AssignLocal(endName));
            foreach (Instruction instruction in stepInstructions)
                Instructions.Add(instruction);
            Instructions.Add(new AssignLocal(stepName));

            // Add a label to mark the start of the loop.
            Instructions.Add(new Label(loopName));

            int angleDepth = 0;

            // Begin processing the interior of the loop.
            while (!Done)
            {
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LANGLE))
                    angleDepth++;
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.RANGLE))
                {
                    if (angleDepth == 0)
                        break;
                    angleDepth--;
                }
                ParseNext();
            }

            // Increment the loop variable by the step value.
            Instructions.Add(new LoadLocal(loopVariable));
            Instructions.Add(new LoadLocal(stepName));
            Instructions.Add(BinaryAdd.Instance);
            Instructions.Add(new AssignLocal(loopVariable));

            // Jump to the start of the loop if the loop variable is still less than or equal to the end value.
            Instructions.Add(new LoadLocal(loopVariable));
            Instructions.Add(new LoadLocal(endName));
            Instructions.Add(new JumpIfLessThanOrEqual(loopName));
        }

        /// <summary>
        /// Process a generic `Range` statement.
        /// </summary>
        /// <param name="nsType"></param>
        protected void ProcessRange()
        {
            DmlSyntaxError badRange = DmlSyntaxError.BadRangeStatement();
            Advance(exception: badRange);

            // Parse the loop variable.
            if (!DmlTokens.IsName(CurrentToken))
                throw badRange;

            string loopVariable = CurrentToken;

            Advance(exception: badRange);

            // Parse the range start.
            List<String> startTokens;
            try {
                startTokens = new List<String>(GetUntil(DmlTokens.ELLIPSES));
            } catch (ParserError) {
                throw badRange;
            }
            ExpressionBuilder startExpressionBuilder = new ExpressionBuilder(startTokens.ToArray(), currentLine);
            startExpressionBuilder.Parse();
            var startInstructions = (Instruction[])startExpressionBuilder.GetResult();

            Advance(exception: badRange);

            // Parse the range end and range step.
            List<String> endTokens;
            try {
                endTokens = new List<String>(GetUntil(DmlTokens.LANGLE));
            } catch (ParserError) {
                throw badRange;
            }

            // Check if the range operator is extended to include a step.
            int stepIndex = endTokens.IndexOf(DmlTokens.TRANSOP);
            List<String> stepTokens;

            if (stepIndex == endTokens.Count - 1)
                throw badRange;
            else if (stepIndex != -1)
            {
                stepTokens = new List<string>(endTokens.Skip(stepIndex + 1));
                endTokens = new List<string>(endTokens.Take(stepIndex));
            }
            else
                // Default step size is 1.
                stepTokens = new List<String>() { "1" };

            ExpressionBuilder endExpressionBuilder = new ExpressionBuilder(endTokens.ToArray(), currentLine);
            ExpressionBuilder stepExpressionBuilder = new ExpressionBuilder(stepTokens.ToArray(), currentLine);

            endExpressionBuilder.Parse();
            stepExpressionBuilder.Parse();

            var endInstructions = (Instruction[])endExpressionBuilder.GetResult();
            var stepInstructions = (Instruction[])stepExpressionBuilder.GetResult();

            /*
            if (DmlParser.IsSimpleConstant(startInstructions) &&
                DmlParser.IsSimpleConstant(endInstructions) &&
                DmlParser.IsSimpleConstant(stepInstructions)
                )
            {
                double start, end, step;
                try
                {
                    start = (double)(new CodeBlock(startInstructions).Evaluate(null, null).Value);
                    end   = (double)(new CodeBlock(endInstructions).Evaluate(null, null).Value);
                    step  = (double)(new CodeBlock(stepInstructions).Evaluate(null, null).Value);
                    InlineRange(loopVariable, start, end, step);
                }
                catch (InvalidCastException)
                {
                    // InlineRange can only handle double values.
                    ComplexRange(loopVariable, startInstructions, endInstructions, stepInstructions);
                }
            }
            else
             */
            // FIXME:
            // So there's a problem with InlineRange. If there are jump statements inside the code within
            // the Range block, the labels and the jumps will get repeated, so all jumps lead to the same
            // spot, which leads to infinite recursion.

            // Not sure why but sometimes the current token is set to the left namespace delimiter,
            // and sometimes its set to the first token after the delimiter. It doesn't seem to be
            // dependent on the presense of the step operator \> either. For now this works.
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LANGLE))
                Advance();
            ComplexRange(loopVariable, startInstructions, endInstructions, stepInstructions);
        }
    }
}
