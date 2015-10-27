using System;

namespace Phosphaze.Core.Dml.Parse
{
    /// <summary>
    /// Exception raised whenever invalid syntax is encountered while parsing.
    /// </summary>
    public class DmlSyntaxError : Exception
    {

        public DmlSyntaxError(string msg) : base(msg) { }

        public DmlSyntaxError(string msg, Exception inner) : base(msg, inner) { }

        public DmlSyntaxError(int line, DmlSyntaxError inner) : base(
            String.Format("An error occurred on line {0}.\n{1}", line, inner.Message)
            ) { }

        public static DmlSyntaxError UnexpectedToken(string expected) 
        {
        	return new DmlSyntaxError(
        		String.Format("Invalid syntax; expected token {0} doesn't match current token.", expected)
        		);
        }

        /// <summary>
        /// The default error thrown when the argument count for a function is invalid.
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="expected"></param>
        /// <param name="received"></param>
        /// <returns></returns>
        public static DmlSyntaxError BadArgumentCount(string funcName, int expected, int received)
        {
            return new DmlSyntaxError(
                String.Format(
                  "Invalid syntax; function {0} expects {1} arguments; got {2} instead.",
                    funcName, expected, received)
                );
        }

        /// <summary>
        /// The default error thrown when the argument type for a function is invalid.
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="argNum"></param>
        /// <param name="expected"></param>
        /// <param name="received"></param>
        /// <returns></returns>
        public static DmlSyntaxError BadArgumentType(string funcName, int argNum, DmlType expected, DmlType received)
        {
            return new DmlSyntaxError(
                String.Format(
                    "Invalid syntax; function {0} expects type {1} as argument {2}; got type {3} instead.",
                    funcName, expected, argNum, received)
                );
        }

        public static DmlSyntaxError BadBinaryOperandTypes(string @operator, DmlType first, DmlType second)
        {
            return new DmlSyntaxError(
                String.Format(
                    "Invalid syntax; invalid operand types {0} and {1} for operator {2}.", @operator, first, second)
                    );
        }

        public static DmlSyntaxError BadUnaryOperandType(string @operator, DmlType operand)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid operand type {0} for unary operator {1}.", @operator, operand)
                );
        }

        /// <summary>
        /// The default error thrown when an assignment statement is found in an invalid namespace.
        /// Assignment statements can only occur in the global namespace or a non-static namespace.
        /// </summary>
        /// <returns></returns>
        public static DmlSyntaxError BadAssignmentNamespace()
        {
            return new DmlSyntaxError(
                "Invalid syntax; assignment statements must either be global or local to a bullet."
                );
        }

        /// <summary>
        /// The default error thrown when an assignment statement doesn't follow the proper format.
        /// </summary>
        /// <returns></returns>
        public static DmlSyntaxError BadAssignmentStatement()
        {
            return new DmlSyntaxError(
                "Invalid syntax; bad assignment statement syntax. All assignment statements " + 
                "must be of the form `Assign [[global|instance]name] [expression]`."
                );
        }

        /// <summary>
        /// The default error thrown when a global name is invalid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DmlSyntaxError BadGlobalName(string name)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid name \"{0}\" for global assignment.", name)
                );
        }

        /// <summary>
        /// The default error thrown when a bullet bound name is invalid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DmlSyntaxError BadBulletInstanceName(string name)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid name \"{0}\" for bullet bound assignment.", name)
                );
        }

        /// <summary>
        /// The default error thrown when a bullet name is invalid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DmlSyntaxError BadBulletName(string name)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid name \"{0}\" for bullet.", name)
                );
        }

        /// <summary>
        /// The generic error for an invalid variable name.
        /// </summary>
        public static DmlSyntaxError BadVariableName(string name)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid variable name \"{0}\".", name)
                );
        }

        /// <summary>
        /// The default error thrown when a bullet declaration is found in a non-global namespace.
        /// </summary>
        /// <returns></returns>
        public static DmlSyntaxError BadBulletNamespace()
        {
            return new DmlSyntaxError(
                "Invalid syntax; Bullet declarations can only be in the global namespace."
                );
        }

        /// <summary>
        /// The default error thrown when a bullet declaration doesn't follow the proper format.
        /// </summary>
        /// <returns></returns>
        public static DmlSyntaxError BadBulletDeclaration()
        {
            return new DmlSyntaxError(
                "Invalid syntax; Bullet declarations must take the form `Bullet [global][name] < [code] >`."
                );
        }

        /// <summary>
        /// The default error thrown when a range statement doesn't follow the proper format.
        /// </summary>
        public static DmlSyntaxError BadRangeStatement()
        {
            return new DmlSyntaxError(
                "Invalid syntax; Range declarations must take the form `Range " +
                "[variable] [expression]...[expression][.[expression]]`."
                );
        }

        public static DmlSyntaxError InvalidTokenForContext(string token, string context)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; invalid token \"{0}\" for context \"{1}\".", token, context)
                );
        }

        public static DmlSyntaxError MismatchedParentheses()
        {
            return new DmlSyntaxError(
                "Invalid syntax; mismatched parentheses."
                );
        }

        public static DmlSyntaxError MismatchedNamespaceDelimiters()
        {
            return new DmlSyntaxError(
                "Invalid syntax; mismatched namespace delimiters."
                );
        }

        public static DmlSyntaxError BadTimeCommandSyntax()
        {
            return new DmlSyntaxError(
                "Invalid syntax; invalid time command syntax."
                );
        }

        public static DmlSyntaxError BadTimeCommandPlacement()
        {
            return new DmlSyntaxError(
                "Invalid syntax; time commands may not be nested."
                );
        }

        public static DmlSyntaxError BlockMissingDelimiters(string blockName)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; `{0}` block requires namespace delimiters.", blockName)
                );
        }

        public static DmlSyntaxError DuplicateNamespaceInBullet(string name)
        {
            return new DmlSyntaxError(
                String.Format("Invalid syntax; cannot have multiple {0} blocks in the same bullet namespace.", name)
                );
        }

        public static DmlSyntaxError DuplicateTimeline()
        {
            return new DmlSyntaxError(
                "Invalid syntax; cannot have multiple Timeline blocks in the same file."
                );
        }

        public static DmlSyntaxError BadBehaviourSyntax()
        {
            return new DmlSyntaxError(
                "Invalid syntax; behaviours must take the form `[behaviour] | " +
                "%[param1] [expression], %[param2] [expression], ... ;`."
                );
        }

        public static DmlSyntaxError InvalidSpawnPlacement()
        {
            return new DmlSyntaxError(
                "Invalid syntax; Spawn statements must be contained within time commands."
                );
        }
    }
}
