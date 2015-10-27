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
using Microsoft.Xna.Framework;

namespace Phosphaze.Core.Collision
{

    /// <summary>
    /// A collider that represents a 2D point.
    /// </summary>
    public sealed class ParticleCollider : ICollidable
    {

        public CollisionType GetCollisionType() { return CollisionType.PARTICLE; }

        // The x coordinate of the particle.
        public double x { get; set; }
        // The y coordinate of the particle.
        public double y { get; set; }

        /// <summary>
        /// Construct a new ParticleCollider.
        /// </summary>
        /// <param name="x">The x component of the particle.</param>
        /// <param name="y">The y component of the particle.</param>
        public ParticleCollider(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Construct a new ParticleCollider from a Vector2.
        /// </summary>
        /// <param name="vec"></param>
        public ParticleCollider(Vector2 vec) : this(vec.X, vec.Y) { }

        /// <summary>
        /// Construct a new ParticleCollider from a Point.
        /// </summary>
        /// <param name="point"></param>
        public ParticleCollider(Point point) : this(point.X, point.Y) { }

        /// <summary>
        /// Return this ParticleCollider as a Vector2.
        /// </summary>
        /// <returns></returns>
        public Vector2 AsVector() { return new Vector2((float)x, (float)y); }

        /// <summary>
        /// Return this collider's minimum axis-oriented bounding rectangle.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)x, (int)y, 0, 0);
        }

        /// <summary>
        /// Translate the collider by the given delta Vector2.
        /// </summary>
        /// <param name="delta"></param>
        public void Translate(Vector2 delta)
        {
            x += delta.X;
            y += delta.Y;
        }

    }

    /// <summary>
    /// Base class for checking collisions between ICollidable objects.
    /// </summary>
    public sealed partial class CollisionChecker
    {

        /// <summary>
        /// Check for a collision between a ParticleCollider and a ParticleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(ParticleCollider self, ParticleCollider other)
        {
            return (self.x == other.x && self.y == other.y);
        }

        /// <summary>
        /// Check for a collision between a ParticleCollider and a LineSegmentCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(ParticleCollider self, LineSegmentCollider other)
        {
            return Collision(other, self);
        }

        /// <summary>
        /// Check for a collision between a ParticleCollider and a RayCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(ParticleCollider self, RayCollider other)
        {
            return Collision(other, self);
        }

        /// <summary>
        /// Check for a collision between a ParticleCollider and a RectCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(ParticleCollider self, RectCollider other)
        {
            return Collision(other, self);
        }

        /// <summary>
        /// Check for a collision between a ParticleCollider and a CircleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(ParticleCollider self, CircleCollider other)
        {
            return Collision(other, self);
        }
    }
}
