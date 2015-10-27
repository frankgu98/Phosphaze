using System.Collections.Generic;
using System.IO;

namespace Phosphaze.Core.Dml.Parse
{
    public class DmlFileReader : DmlParser
    {

        string filename;

        Dictionary<string, DmlBulletFactory> Bullets = new Dictionary<string, DmlBulletFactory>();

        DmlTimeline globalTimeline;

        public DmlFileReader(string filename)
            : base(Tokenizer.Tokenize(new StreamReader(filename).ReadToEnd()), 0)
        {
            this.filename = filename;
        }

        protected override void ProcessNext()
        {
            try
            {
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_ASSIGN))
                    ProcessAssignment(NamespaceType.Global);
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_RANGE))
                    ProcessRange();
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.UQNAME) ||
                         DmlTokens.IsBuiltin(CurrentToken))
                    ProcessExpression(GetUntil(DmlTokens.EOL));
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_BULLET))
                {
                    SetExpecting(DmlTokens.AT, DmlTokens.UQNAME, DmlTokens.LANGLE);
                    Advance(2, exception: DmlSyntaxError.BadBulletDeclaration());

                    string bulletName = CurrentToken;
                    Advance(exception: DmlSyntaxError.BadBulletDeclaration());

                    BulletBuilder bulletBuilder = new BulletBuilder(bulletName, GetNamespaceBlock(), currentLine);
                    bulletBuilder.Parse();

                    Bullets[bulletName] = (DmlBulletFactory)bulletBuilder.GetResult();
                }
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_TIMELINE))
                {
                    if (globalTimeline != null)
                        throw DmlSyntaxError.DuplicateTimeline();
                    SetExpecting(DmlTokens.LANGLE);
                    Advance(exception: DmlSyntaxError.BlockMissingDelimiters("Timeline"));

                    TimelineBuilder timelineBuilder = new TimelineBuilder(GetNamespaceBlock(), currentLine);
                    timelineBuilder.Parse();

                    globalTimeline = (DmlTimeline)timelineBuilder.GetResult();
                }
            }
            catch (DmlSyntaxError exception)
            {
                throw new DmlSyntaxError(currentLine, exception);
            }
            
        }

        public override object GetResult()
        {
            if (globalTimeline == null)
                globalTimeline = new DmlTimeline();
            return new DmlSystem(new CodeBlock(Instructions.ToArray()), globalTimeline, Bullets);
        }

    }
}
