using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze.Core.Dml
{
    public abstract class Component
    {

        public bool Dead = false;

        public abstract void Update(DmlBullet bullet, DmlSystem system);

    }
}
