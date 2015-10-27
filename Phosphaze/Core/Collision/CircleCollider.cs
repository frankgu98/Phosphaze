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
    /// A collider that represents a circle.
    /// </summary>
    public sealed class CircleCollider : ICollidable
    {

        public CollisionType GetCollisionType() { return CollisionType.CIRCLE; }

        // The x component of the center of the circle.
        public double x { get; private set; }
        // The y component of the center of the circle.
        public double y { get; private set; }
        // The radius.
        public double radius { get; set; }

        // The center fo the circle.
        public Vector2 Center { get { return new Vector2((float)x, (float)y); } }

        /// <summary>
        /// Construct a new CircleCollider.
        /// </summary>
        /// <param name="x">The x component of the center.</param>
        /// <param name="y">The y component of the center.</param>
        /// <param name="r">The radius.</param>
        public CircleCollider(double x, double y, double r)
        {
            this.x = x;
            this.y = y;
            this.radius = r;
        }

        /// <summary>
        /// Return this collider's minimum axis-oriented bounding rectangle.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBoundingBox()
        {
            int diameter = (int)radius * 2;
            return new Rectangle((int)(x - radius), (int)(y - radius), diameter, diameter);
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

        public static bool Collision(CircleCollider self, ParticleCollider other)
        {
            Vector2 center = new Vector2((float)self.x, (float)self.y);
            Vector2 particle = new Vector2((float)other.x, (float)other.y);
            return Vector2.Distance(center, particle) < self.radius;
        }

        public static bool Collision(CircleCollider self, LineSegmentCollider other)
        {
            return GeometryUtils.CircleToSegmentPOI(
                self.x, self.y, self.radius, other.x1, other.y1, other.x2, other.y2).Length > 0;
        }

        public static bool Collision(CircleCollider self, RayCollider other)
        {
            return GeometryUtils.CircleToRayPOI(
                self.x, self.y, self.radius, other.x1, other.y1, other.x2, other.y2).Length > 0;
        }

        public static bool Collision(CircleCollider self, RectCollider other)
        {
            double cx = self.x, cy = self.y, r = self.radius;
            double x = other.x, y = other.y, w = other.width, h = other.height;
            double rx = x + w / 2, ry = y + h / 2;
            double x_offset = Math.Abs(cx - rx);
            double y_offset = Math.Abs(cy - ry);
            double half_width = w / 2;
            double half_height = h / 2;

            if (x_offset > (half_width + r))
                return false;
            else if (y_offset > (half_height + r))
                return false;

            if (x_offset <= half_width)
                return true;
            else if (y_offset <= half_height)
                return true;

            double deltax = x_offset - half_width;
            double deltay = y_offset - half_height;
            double dist = Math.Pow(deltax, 2) + Math.Pow(deltay, 2);
            return dist <= r * r ? true : false;
        }

        public static bool Collision(CircleCollider self, CircleCollider other)
        {
            Vector2 center1 = new Vector2((float)self.x, (float)self.y);
            Vector2 center2 = new Vector2((float)other.x, (float)other.y);
            return Vector2.Distance(center1, center2) < self.radius + other.radius;
        }
    }
}
