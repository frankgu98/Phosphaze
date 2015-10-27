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
using Phosphaze.Core.Utils;
using Microsoft.Xna.Framework;

namespace Phosphaze.Core.Collision
{

    /// <summary>
    /// A collider that represents a 2D ray.
    /// </summary>
    public sealed class RayCollider : ICollidable
    {

        public CollisionType GetCollisionType() { return CollisionType.RAY; }

        // The x component of the start coordinate.
        public double x1 { get; set; }
        // The y component of the start coordinate.
        public double y1 { get; set; }
        // The x component of the end coordinate.
        public double x2 { get; set; }
        // The y component of the end coordinate.
        public double y2 { get; set; }

        /// <summary>
        /// Construct a new RayCollider.
        /// </summary>
        /// <param name="x1">The x component of the start point.</param>
        /// <param name="y1">The y component of the start point.</param>
        /// <param name="x2">The x component of the direction point.</param>
        /// <param name="y2">The y component of the direction point.</param>
        public RayCollider(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        /// <summary>
        /// Return this collider's minimum axis-oriented bounding rectangle.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBoundingBox()
        {
            return new Rectangle(); // Not really sure what to do here.
        }

        /// <summary>
        /// Translate the collider by the given delta Vector2.
        /// </summary>
        /// <param name="delta"></param>
        public void Translate(Vector2 delta)
        {
            x1 += delta.X;
            x2 += delta.X;
            y1 += delta.Y;
            y2 += delta.Y;
        }
    }

    /// <summary>
    /// Base class for checking collisions between ICollidable objects.
    /// </summary>
    public sealed partial class CollisionChecker
    {

        /// <summary>
        /// Check for a collision between a RayCollider and a ParticleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RayCollider self, ParticleCollider other)
        {
            return GeometryUtils.IsPointOnRay(other.x, other.y, self.x1, self.y1, self.x2, self.y2);
        }

        /// <summary>
        /// Check for a collision between a RayCollider and a LineSegmentCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RayCollider self, LineSegmentCollider other)
        {
            return GeometryUtils.RayToSegmentPOI(
                self.x1, self.y1, self.x2, self.y1,
                other.x1, other.y1, other.x2, other.y2).HasValue;
        }

        /// <summary>
        /// Check for a collision between a RayCollider and a RayCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RayCollider self, RayCollider other)
        {
            return GeometryUtils.AreRaysIntersecting(
                self.x1, self.y1, self.x2, self.y2,
                other.x1, other.y1, other.x2, other.y2);
        }

        /// <summary>
        /// Check for a collision between a RayCollider and a RectCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RayCollider self, RectCollider other)
        {
            return Collision(other, self);
        }

        /// <summary>
        /// Check for a collision between a RayCollider and a CircleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RayCollider self, CircleCollider other)
        {
            return Collision(other, self);
        }
    }
}
