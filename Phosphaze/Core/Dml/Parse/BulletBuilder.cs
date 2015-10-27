using Phosphaze.Core.Dml.Parse.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse
{
    public class BulletBuilder : AbstractParser
    {

        private string name;

        /// <summary>
        /// The bullet init block.
        /// </summary>
        private CodeBlock Init = null;

        /// <summary>
        /// The bullet update block.
        /// </summary>
        private CodeBlock Update = null;

        public BulletBuilder(string name, string[] tokens, int currentLine) 
            : base(tokens, currentLine) 
        {
            this.name = name;
        }

        protected override void ProcessNext()
        {
            // Check for an assignment statement.
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_ASSIGN))
            {
                throw DmlSyntaxError.BadAssignmentNamespace();
            }
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_INIT))
            {
                if (Init != null)
                    throw DmlSyntaxError.DuplicateNamespaceInBullet("Init");
                SetExpecting(DmlTokens.LANGLE);
                Advance(exception: DmlSyntaxError.BlockMissingDelimiters("Init"));

                string[] tokens = GetNamespaceBlock();
                BulletInitBuilder initBuilder = new BulletInitBuilder(tokens, currentLine);
                initBuilder.Parse();

                var initBlock = (Instruction[])initBuilder.GetResult();
                Init = new CodeBlock(initBlock);
            }
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_UPDATE))
            {
                if (Update != null)
                    throw DmlSyntaxError.DuplicateNamespaceInBullet("Update");
                SetExpecting(DmlTokens.LANGLE);
                Advance(exception: DmlSyntaxError.BlockMissingDelimiters("Update"));

                string[] tokens = GetNamespaceBlock();
                BulletUpdateBuilder updateBuilder = new BulletUpdateBuilder(tokens, currentLine);
                updateBuilder.Parse();

                var updateBlock = (Instruction[])updateBuilder.GetResult();
                Update = new CodeBlock(updateBlock);
            }
            else
                throw DmlSyntaxError.InvalidTokenForContext(CurrentToken, "bullet");
        }

        public override object GetResult()
        {
            if (Init == null)
                Init = new CodeBlock(new Instruction[] {});
            if (Update == null)
                Update = new CodeBlock(new Instruction[] {});
            return new DmlBulletFactory(name, Init, Update);
        }

    }

    /// <summary>
    /// Builder for bullet initializer.
    /// </summary>
    public class BulletInitBuilder : DmlParser
    {

        public BulletInitBuilder(string[] tokens, int currentLine) : base(tokens, currentLine) { }

        protected override void ProcessNext()
        {
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_ASSIGN))
                ProcessAssignment(NamespaceType.BulletInit);
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_RANGE))
                ProcessRange();
            else if (DmlTokens.IsName(CurrentToken) || DmlTokens.IsBuiltin(CurrentToken))
                ProcessExpression(GetUntil(DmlTokens.EOL));
            else
                throw DmlSyntaxError.InvalidTokenForContext(CurrentToken, "bullet init");
        }

        public override object GetResult()
        {
            return Instructions.ToArray();
        }
    }

    public class BulletUpdateBuilder : DmlParser
    {

        public BulletUpdateBuilder(string[] tokens, int currentLine) : base(tokens, currentLine) { }

        protected override void ProcessNext()
        {
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_ASSIGN))
                ProcessAssignment(NamespaceType.BulletUpdate);
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_RANGE))
                ProcessRange();
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_CHILDREN))
                throw new NotImplementedException("This hasn't been implemented yet, sorry!");
            else if (DmlTokens.IsTimeCommand(CurrentToken))
                ProcessTimeCommand();
            else if (DmlTokens.IsName(CurrentToken) || DmlTokens.IsBuiltin(CurrentToken))
                ProcessExpression(GetUntil(DmlTokens.EOL));
            else if (DmlTokens.IsBehaviour(CurrentToken))
                ProcessBehaviour();
            else
                throw DmlSyntaxError.InvalidTokenForContext(CurrentToken, "bullet update");
        }

        protected virtual void ProcessTimeCommand()
        {
            DmlSyntaxError badTimeCommand = DmlSyntaxError.BadTimeCommandSyntax();

            ProcessExpression(GetUntil(DmlTokens.LANGLE));

            string jumpName = String.Format("#{0:X6}", Globals.randGen.Next(0x1000000));
            Instructions.Add(new JumpIfFalse(jumpName));

            Advance(exception: badTimeCommand);

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
            if (angleDepth != 0) // We want it to break forcefully, otherwise there would be invalid syntax.
                throw DmlSyntaxError.MismatchedNamespaceDelimiters();

            Instructions.Add(new Label(jumpName));
        }

        protected void ProcessBehaviour()
        {
            DmlSyntaxError badBehaviour = DmlSyntaxError.BadBehaviourSyntax();

            string behaviourName = CurrentToken;

            SetExpecting(DmlTokens.VBAR);
            Advance(2, exception: badBehaviour);

            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.EOL))
            {
                // The behaviour is empty.
                Instructions.Add(BehavioursDict.InitializeBehaviour(behaviourName, new string[] { }));
                return;
            }

            var paramExpressions = new List<List<string>>();
            var parameters = new List<string>();
            bool insideExpression = false;
            int parenthDepth = 0;

            while (!Done)
            {
                if (!insideExpression && DmlTokens.IsMatch(CurrentToken, DmlTokens.PERCENT))
                {
                    Advance(exception: badBehaviour);
                    if (!DmlTokens.IsNonKeywordName(CurrentToken))
                        throw DmlSyntaxError.BadVariableName(CurrentToken);
                    parameters.Add(CurrentToken);
                    paramExpressions.Add(new List<string>());
                    insideExpression = true;
                }
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.COMMA) && parenthDepth == 0)
                {
                    insideExpression = false;
                    SetExpecting(DmlTokens.PERCENT);
                }
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.EOL) && parenthDepth == 0)
                    break;
                else
                {
                    if (DmlTokens.IsLeftBracket(CurrentToken))
                    {
                        if (!insideExpression)
                            throw badBehaviour;
                        parenthDepth++;
                    }
                    else if (DmlTokens.IsRightBracket(CurrentToken))
                    {
                        if (!insideExpression)
                            throw badBehaviour;
                        parenthDepth--;
                    }
                    paramExpressions.Last().Add(CurrentToken);
                }
                Advance();
            }
            if (parenthDepth != 0) // We want it to break forcefully, otherwise there would be invalid syntax.
                throw badBehaviour;

            foreach (var expression in paramExpressions)
                ProcessExpression(expression.ToArray());
            Instructions.Add(BehavioursDict.InitializeBehaviour(behaviourName, parameters.ToArray()));
        }

        public override object GetResult()
        {
            return Instructions.ToArray();
        }
    }
}
