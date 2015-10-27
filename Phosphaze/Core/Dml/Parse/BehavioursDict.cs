using System;
using System.Collections.Generic;
using Phosphaze.Core.Dml.Parse.Behaviours;

namespace Phosphaze.Core.Dml.Parse
{

    /// <summary>
    /// The static list of behaviours available in Dml.
    /// </summary>
    public static class BehavioursDict
    {

        /// <summary>
        /// The dictionary of behaviours. Since each Behaviour instruction is different depending
        /// on the parameters sent in, we store the behaviour class itself and instantiate it using
        /// reflection, instead of individual instances.
        /// </summary>
        public static Dictionary<string, Type> Behaviours = new Dictionary<string, Type>()
        {
            {"Spawn",                     typeof(Spawn)},
            {"RadialSpawn",         typeof(RadialSpawn)},
            {"MultiSpawn",           typeof(MultiSpawn)},
            {"BurstSpawn",           typeof(BurstSpawn)},
            {"TransitionSpeed", typeof(TransitionSpeed)},
            {"MoveTo",                   typeof(MoveTo)},
            {"RotateDirection", typeof(RotateDirection)},
            {"RotateAround",       typeof(RotateAround)},
            {"Gravity",                 typeof(Gravity)},
            {"Kill",                       typeof(Kill)},
            {"KillIfOffscreen", typeof(KillIfOffscreen)}
        };

        /// <summary>
        /// Create an instance of a behaviour with the given name and parameters.
        /// </summary>
        /// <param name="name">The name of the behaviour.</param>
        /// <param name="parameters">The list of parameters to send in.</param>
        public static Instruction InitializeBehaviour(string name, string[] parameters)
        {
            Type behaviour = Behaviours[name];
            Behaviour instruction = (Behaviour)(behaviour.GetConstructor(new Type[] { }).Invoke(new Object[] { }));
            instruction.Initialize(parameters);
            return instruction;
        }

    }
}
