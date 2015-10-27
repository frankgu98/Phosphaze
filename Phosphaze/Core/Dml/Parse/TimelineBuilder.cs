using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse
{
    public class TimelineBuilder : BulletUpdateBuilder
    {
        private DmlTimeline timeline = new DmlTimeline();

        private DmlTimestamp currentTimestamp;

        private bool inTimeStamp = false;

        public TimelineBuilder(string[] tokens, int currentLine)
            : base(tokens, currentLine) { }

        protected override void ProcessNext()
        {
            if (DmlTokens.IsTimeCommand(CurrentToken))
                ProcessTimeCommand();
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_ASSIGN))
                ProcessAssignment(NamespaceType.Timeline);
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_RANGE))
                ProcessRange();
            else if (DmlTokens.IsMatch(CurrentToken, "Spawn"))
            {
                if (!inTimeStamp)
                    throw DmlSyntaxError.InvalidSpawnPlacement();
                ProcessBehaviour();
            }
            else
                throw DmlSyntaxError.InvalidTokenForContext(CurrentToken, "timeline");
        }

        protected override void ProcessTimeCommand()
        {
            if (inTimeStamp)
                throw DmlSyntaxError.BadTimeCommandPlacement();
            inTimeStamp = true;

            DmlSyntaxError badTimeCommand = DmlSyntaxError.BadTimeCommandSyntax();
            double time, start = -1, end = -1;
            string timeCommand = CurrentToken;

            switch (timeCommand)
            {
                case "At":
                case "Before":
                case "After":
                    SetExpecting(DmlTokens.LPAR, DmlTokens.NUMBER, DmlTokens.RPAR, DmlTokens.LANGLE);

                    Advance(2, exception: badTimeCommand);
                    time = Double.Parse(CurrentToken);
                    
                    Advance(2, exception: badTimeCommand);
                    if (timeCommand == "At")
                        currentTimestamp = new TimestampAt(time);
                    else if (timeCommand == "Before")
                        currentTimestamp = new TimestampBefore(time);
                    else
                        currentTimestamp = new TimestampAfter(time);
                    break;

                case "AtIntervals":
                case "DuringIntervals":
                    SetExpecting(DmlTokens.LPAR, DmlTokens.NUMBER);

                    Advance(2, exception: badTimeCommand);
                    time = Double.Parse(CurrentToken);

                    if (DmlTokens.IsMatch(NextToken, DmlTokens.COMMA))
                    {
                        SetExpecting(DmlTokens.COMMA, DmlTokens.NUMBER);
                        Advance(2, exception: badTimeCommand);
                        start = Double.Parse(CurrentToken);

                        SetExpecting(DmlTokens.COMMA, DmlTokens.NUMBER);
                        Advance(2, exception: badTimeCommand);
                        end = Double.Parse(CurrentToken);
                    }

                    SetExpecting(DmlTokens.RPAR, DmlTokens.LANGLE);
                    Advance(2, exception: badTimeCommand);

                    if (timeCommand == "AtIntervals")
                    {
                        if (start == -1)
                            currentTimestamp = new TimestampAtIntervals(time);
                        else
                            currentTimestamp = new TimestampAtIntervals(time, start, end);
                    }
                    else
                    {
                        if (start == -1)
                            currentTimestamp = new TimestampDuringIntervals(time);
                        else
                            currentTimestamp = new TimestampDuringIntervals(time, start, end);
                    }
                    break;

                case "From":
                    SetExpecting(
                        DmlTokens.LPAR, DmlTokens.NUMBER, DmlTokens.COMMA, 
                        DmlTokens.NUMBER, DmlTokens.RPAR, DmlTokens.LANGLE
                        );
                    Advance(2, exception: badTimeCommand);
                    start = Double.Parse(CurrentToken);

                    Advance(2, exception: badTimeCommand);
                    end = Double.Parse(CurrentToken);

                    Advance(2, exception: badTimeCommand);
                    currentTimestamp = new TimestampFrom(start, end);
                    break;
            }

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

            currentTimestamp.SetCode(new CodeBlock(Instructions.ToArray()));
            Instructions = new List<Instruction>();
            timeline.AddTimestamp(currentTimestamp);
            inTimeStamp = false;
        }

        public override object GetResult()
        {
            return timeline;
        }

    }
}
