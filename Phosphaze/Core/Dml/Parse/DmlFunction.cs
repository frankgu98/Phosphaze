using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse
{
    public abstract class DmlFunction
    {

        /// <summary>
        /// This function's name. This is used by certain logging methods.
        /// </summary>
        public virtual string Name { get { return "DEFAULT"; } }

        /// <summary>
        /// Determine whether a function is compatible with a given number of arguments.
        /// </summary>
        /// <param name="argCount"></param>
        /// <returns></returns>
        public virtual bool CompatibleWithArgCount(int argCount)
        {
            return true;
        }

        /// <summary>
        /// Determine whether a function is compatible with a set of input types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public virtual bool CompatibleWithArgTypes(params DmlType[] types)
        {
            return true;
        }

        /// <summary>
        /// Whether or not the function is pure. A function is pure iff it always
        /// evaluates to the same result for the same input arguments, for all possible
        /// input arguments. For example, Sin(x) is pure, whereas Random() is not.
        /// </summary>
        public virtual bool IsPure { get { return false; } }

        /// <summary>
        /// Call this function. CallDynamic does not assume the types of the objects on the stack are
        /// safe for use by the function. CallTypeSafe does.
        /// </summary>
        /// <param name="argCount"></param>
        /// <param name="stack"></param>
        /// <param name="bullet"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public abstract DmlObject CallDynamic(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system);

        /// <summary>
        /// Call this function. Unlike CallDynamic, this assumes the types on the stack are safe (i.e.
        /// they satisfy CompatibleWithArgTypes.
        /// 
        /// By default this just defers to CallDynamic, but this can be override in child classes. The
        /// point is that CallTypeSafe is supposed to be faster than CallDynamic because it doesn't have
        /// to check the types of the items on the stack, it can instead directly cast the DmlObjects
        /// to their C# types.
        /// </summary>
        /// <param name="argCount"></param>
        /// <param name="stack"></param>
        /// <param name="bullet"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual DmlObject CallTypeSafe(int argCount, Stack<DmlObject> stack, DmlObject bullet, DmlSystem system)
        {
            return CallDynamic(argCount, stack, bullet, system);
        }

        /// <summary>
        /// Check if the given argument count is equal to the expected argument count, and throw
        /// an error if not.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="received"></param>
        [Obsolete("CompatibleWithArgCount will have already checked to ensure the given argument count is acceptable.")]
        protected void CheckArgumentCount(int expected, int received)
        {
            if (expected != received)
                throw DmlSyntaxError.BadArgumentCount(Name, expected, received);  
        }

    }
}
