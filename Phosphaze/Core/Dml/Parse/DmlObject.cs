using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse
{

    /// <summary>
    /// Any object represented in Dml code.
    /// </summary>
    public class DmlObject
    {

        /// <summary>
        /// The type of this object.
        /// </summary>
        public DmlType Type { get; private set; }

        /// <summary>
        /// The value of this object.
        /// </summary>
        public object Value = null;

        /// <summary>
        /// The variables bound to this object.
        /// </summary>
        public Dictionary<string, DmlObject> BoundVars { get; private set; }

        public DmlObject(DmlType type, object value)
        {
            Type = type;
            Value = value;
            BoundVars = new Dictionary<string, DmlObject>();
        }

        public void SetVar(string name, DmlObject obj)
        {
            if (name[0] == '"')
                name = name.Substring(1);
            if (name[name.Length - 1] == '"')
                name = name.Substring(0, name.Length - 1);
            BoundVars[name] = obj;
        }

        public class DmlOptionFactory : DmlFunction
        {

            private string name;

            public override string Name { get { return name; } }

            public override bool IsPure { get { return true; } }

            public override bool CompatibleWithArgCount(int argCount)
            {
                return argCount == Parameters.Length;
            }

            public override bool CompatibleWithArgTypes(params DmlType[] types)
            {
                if (types.Length != this.types.Length)
                    return false;
                for (int i = 0; i < types.Length; i++)
                    if (this.types[i] != types[i])
                        return false;
                return true;
            } 

            public string[] Parameters { get; private set; }

            private DmlType[] types;

            private DmlOptionFactory(string name, string[] parameters, DmlType[] types)
            {
                this.name = name;
                Parameters = parameters;
                this.types = types;
            }

            public override DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
            {
                var option = new DmlObject(DmlType.Option, null);
                DmlObject nextArg;
                for (int i = 0; i < argCount; i++)
                {
                    nextArg = stack.Pop();
                    option.BoundVars[Parameters[i]] = nextArg;
                }
                return option;
            }

            public static DmlObject Instantiate(string name, string[] parameters, DmlType[] types)
            {
                return new DmlObject(DmlType.Function, new DmlOptionFactory(name, parameters, types));
            }

        }

    }
}
