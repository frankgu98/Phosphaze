using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class Kill : Behaviour
    {

        public Kill() { }

        public void Initialize(string[] parameters)
        {
            if (parameters.Length > 0)
                Console.WriteLine("Hey man, not cool."); // Really not in the mood to write a proper exception.
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            ((DmlBullet)bullet.Value).Dead = true;
        }

    }
}
