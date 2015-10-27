#region License

// Copyright (c) 2015 FCDM
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

// AUTHOR: Michael Ala

using System;

namespace Phosphaze.Core.Utils
{
    /// <summary>
    /// Utilities for exceptions.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Return an ArgumentException with the following message:
        /// 
        /// "Invalid parameter count for {name}."
        /// 
        /// The formatting of the message is dependent on the parameters
        /// passed in.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ArgumentException InvalidParamCount(string name)
        {
            return new ArgumentException(String.Format("Invalid parameter count for {0}.", name));
        }

        /// <summary>
        /// Return an ArgumentException with the following message:
        /// 
        /// "{propName} must be clamped between {lo} and {hi} {inclusive}."
        /// 
        /// The formatting of the message is dependent on the parameters
        /// passed in.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="inclusive">
        /// Whether or not the clamping is "inclusive" or "exclusive". By
        /// default this is true.
        /// </param>
        /// <returns></returns>
        public static ArgumentException NotClamped(string propName, double lo, double hi, bool inclusive = true)
        {
            return new ArgumentException(String.Format("{0} must be clamped between {1} and {2} {3}.", 
                                                       propName, lo, hi, inclusive ? "inclusive" : "exclusive"));
        }
    }
}
