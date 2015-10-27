// ONLY WIZARDS ARE ALLOWED TO READ THIS FILE.
//
// Are you a wizard? Find out at AmIAWizard.gov
// Answer a few simple questions and give us your credit card number
// and we'll determine if you're a wizard or not. Just look at
// all our satisfied customers!
// "This is legit." 
//          - Wizard
// "I lost all my money, but at least I'm a wizard."
//          - Meth addict

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

// AUTHOR: Michael Ala, Authorized Wizard/Meth Addict

using System;
using System.Linq;
using System.Collections.Generic;

namespace Phosphaze.Core.Collision
{
    /// <summary>
    /// Base class for checking collisions between ICollidable objects.
    /// </summary>
    public sealed partial class CollisionChecker
    {

        // Prevent instantiation.
        private CollisionChecker() { }

        private static Dictionary<CollisionType, Type> class_map = new Dictionary<CollisionType, Type>()
        {
            {CollisionType.PARTICLE, typeof(ParticleCollider)},
            {CollisionType.SEGMENT, typeof(LineSegmentCollider)},
            {CollisionType.RAY, typeof(RayCollider)},
            {CollisionType.RECT, typeof(RectCollider)},
            {CollisionType.CIRCLE, typeof(CircleCollider)}
        };

        /// <summary>
        /// Calculate a collision between two arbitrary ICollidable objects.
        /// 
        /// This method is meant to automatically delegate to the proper overloaded
        /// Collision method for the given types.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Collision(ICollidable a, ICollidable b)
        {
            Type class_a = class_map[a.GetCollisionType()];
            Type class_b = class_map[b.GetCollisionType()];
            return (bool)typeof(CollisionChecker).GetMethod("Collision", new Type[] { class_a, class_b })
                                                 .Invoke(null, new Object[] {a, b});
        }

    }
}
