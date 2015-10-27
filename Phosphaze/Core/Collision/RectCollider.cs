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
using Phosphaze.Core.Utils;

namespace Phosphaze.Core.Collision
{

    /// <summary>
    /// A collider that represents a rectangle.
    /// </summary>
    public sealed class RectCollider : ICollidable
    {

        public CollisionType GetCollisionType() { return CollisionType.RECT; }

        // The x component of the topleft corner.
        public double x { get; set; }
        // The y component of the topleft corner.
        public double y { get; set; }
        // The width of the rectangle.
        public double width { get; set; }
        // The height of the rectangle.
        public double height { get; set; }

        // Return the corners of this rectangle..
        public Vector2[] Corners
        {
            get
            {
                return new Vector2[] {
                    new Vector2((float)x, (float)y),
                    new Vector2((float)(x + width), (float)y),
                    new Vector2((float)(x + width), (float)(y + height)),
                    new Vector2((float)x, (float)(y + height))
                };
            }
        }

        /// <summary>
        /// Construct a new RectCollider.
        /// </summary>
        /// <param name="x">The x component of the topleft corner.</param>
        /// <param name="y">The y component of the topleft corner.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public RectCollider(double x, double y, double w, double h)
        {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }

        /// <summary>
        /// Construct a new RectCollider from a Rectanlge.
        /// </summary>
        /// <param name="rect"></param>
        public RectCollider(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        /// <summary>
        /// Return a new Rectangle object from this RectCollider.
        /// </summary>
        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
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
        /// Check for a collision between a RectCollider and a ParticleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RectCollider self, ParticleCollider other)
        {
            return (self.x <= other.x && other.x <= self.x + self.width) && 
                   (self.y <= other.y && other.y <= self.y + self.height);
        }

        /// <summary>
        /// Check for a collision between a RectCollider and a LineSegmentCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RectCollider self, LineSegmentCollider other)
        {
            return GeometryUtils.SeparatingAxisTheorem(self.Corners, new Vector2[] { other.Start, other.End });
        }

        /// <summary>
        /// Check for a collision between a RectCollider and a RayCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RectCollider self, RayCollider other)
        {
            Vector2[] corners = self.Corners;
            Vector2 p1, p2;
            for (int i = 0; i < 4; i++)
            {
                p1 = corners[i];
                p2 = corners[(i + 1) % 4];
                if (GeometryUtils.RayToSegmentPOI(
                    other.x1, other.y1, other.x2, other.y2,
                    p1.X, p1.Y, p2.X, p2.Y).HasValue)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check for a collision between a RectCollider and a RectCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RectCollider self, RectCollider other)
        {
            return (
                (self.x <= other.x + other.width) && 
                (self.x + self.width >= other.x) &&
                (self.y <= other.y + other.height) &&
                (self.y + self.height >= other.y)
                );
        }

        /// <summary>
        /// Check for a collision between a RectCollider and a CircleCollider.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public static bool Collision(RectCollider self, CircleCollider other)
        {
            return Collision(other, self);
        }
    }
}
