using System;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    public class BehaviourException : Exception
    {

        public BehaviourException(string msg) : base(msg) { }

        public BehaviourException(string msg, Exception inner) : base(msg, inner) { }

    }
}
