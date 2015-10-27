using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class KillIfOffscreen : Behaviour
    {

        bool hasLeeway;

        int max_x, max_y;

        public KillIfOffscreen() { }

        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 0 && parameters[0] == "Leeway")
                hasLeeway = true;
            max_x = Options.Resolutions.X;
            max_y = Options.Resolutions.Y;
        }

        public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
            double leeway = 0;
            if (hasLeeway)
                leeway = (double)(stack.Pop().Value);
            DmlBullet b = (DmlBullet)(bullet.Value);
            double x = b.Position.X;
            double y = b.Position.Y;
            if (-leeway <= x && x <= max_x + leeway && -leeway <= y && y <= max_y + leeway)
                return;
            b.Dead = true;
        }

    }
}
