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
using System.Linq;
using Microsoft.Xna.Framework;

namespace Phosphaze.Core.Utils
{
    /// <summary>
    /// A set of generally useful geometric tools and algorithms.
    /// </summary>
    public static class GeometryUtils
    {

        /// <summary>
        /// Rotate a vector by a given amount relative to the origin.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="amt"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 RotateVector(Vector2 vec, double amt, bool degrees = true)
        {
            if (degrees)
                amt = Math.PI * amt / 180f;
            double nx, ny;
            double x = vec.X, y = vec.Y;

            double cos_theta = Math.Cos(amt);
            double sin_theta = Math.Sin(amt);

            nx = x * cos_theta - y * sin_theta;
            ny = x * sin_theta + y * cos_theta;

            return new Vector2((float)nx, (float)ny);
        }

        /// <summary>
        /// Rotate a vector relative to a given anchor.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="anchor"></param>
        /// <param name="amt"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 RotateVector(Vector2 vec, Vector2 anchor, double amt, bool degrees = true)
        {
            return RotateVector(vec - anchor, amt, degrees) + anchor;
        }

        /// <summary>
        /// Check if a point is in the "range" of a line segment.
        /// </summary>
        private static bool IsPointInSegmentRange(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            if (x1 == x2)
                return (y1 <= py) == (py <= y2);
            return (x1 <= px) == (px <= x2);
        }

        /// <summary>
        /// Check if a point is in the "range" of a ray.
        /// </summary>
        private static bool IsPointInRayRange(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            if (x1 == x2)
                return (y1 <= py) == (y1 < y2);
            return (x1 <= px) == (x1 < x2);
        }

        /// <summary>
        /// Check if a point lies on a line.
        /// </summary>
        /// <param name="px">The x component of the point.</param>
        /// <param name="py">The y component of the point.</param>
        /// <param name="x1">The x component of the start of the line.</param>
        /// <param name="y1">The y component of the start of the line.</param>
        /// <param name="x2">The x component of the end of the line.</param>
        /// <param name="y2">The y component of the end of the line.</param>
        public static bool IsPointOnLine(
            double px, double py, 
            double x1, double y1, 
            double x2, double y2)
        {
            if (x2 == x1)
                return (x1 == px);
            else if (y2 == y1)
                return (y1 == py);
            double m = (y2 - y1) / (x2 - x1);
            return (py - y1) == m*(px - x1);
        }

        /// <summary>
        /// Check if a point lies on a line segment.
        /// </summary>
        /// <param name="px">The x component of the point.</param>
        /// <param name="py">The y component of the point.</param>
        /// <param name="x1">The x component of the start of the line segment.</param>
        /// <param name="y1">The y component of the start of the line segment.</param>
        /// <param name="x2">The x component of the end of the line segment.</param>
        /// <param name="y2">The y component of the end of the line segment.</param>
        public static bool IsPointOnSegment(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            return IsPointOnLine(px, py, x1, y1, x2, y2) && IsPointInSegmentRange(px, py, x1, y1, x2, y2);
        }

        /// <summary>
        /// Check if a point lies on a ray.
        /// </summary>
        /// <param name="px">The x component of the point.</param>
        /// <param name="py">The y component of the point.</param>
        /// <param name="x1">The x component of the start of the ray.</param>
        /// <param name="y1">The y component of the start of the ray.</param>
        /// <param name="x2">The x component of the end of the ray.</param>
        /// <param name="y2">The y component of the end of the ray.</param>
        public static bool IsPointOnRay(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            return IsPointOnLine(px, py, x1, y1, x2, y2) && IsPointInRayRange(px, py, x1, y1, x2, y2);
        }

        /// <summary>
        /// Determine the intersection point of two lines (if it exists, otherwise null).
        /// </summary>
        /// <param name="x1">The x component of the start point of the first line.</param>
        /// <param name="y1">The y component of the start point of the first line.</param>
        /// <param name="x2">The x component of the end point of the first line.</param>
        /// <param name="y2">The y component of the end point of the first line.</param>
        /// <param name="x3">The x component of the start point of the second line.</param>
        /// <param name="y3">The y component of the start point of the second line.</param>
        /// <param name="x4">The x component of the end point of the second line.</param>
        /// <param name="y4">The y component of the end point of the second line.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? LineToLinePOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            if (((x1 == x2) && (x3 == x4)) || ((y1 == y2) && (y3 == y4)))
                return null;
            else if (x1 == x2)
            {
                double m1 = (y4 - y3)/(x4 - x3);
                double b1 = y3 - m1 * x3;
                double py = m1*x1 + b1;
                return new Vector2((float)x1, (float)py);
            }
            else if (x3 == x4)
            {
                double m1 = (y2 - y1)/(x2 - x1);
                double b1 = y1 - m1*x1;
                double py = m1*x3 + b1;
                return new Vector2((float)x3, (float)py);
            }
            else
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double m2 = (y4 - y3) / (x4 - x3);

                if (m1 == m2)
                    return null;

                double b1 = y1 - m1*x1;
                double b2 = y3 - m2*x3;

                double px = (b2 - b1)/(m1 - m2);
                double py = m1 * px + b1;

                return new Vector2((float)px, (float)py);
            }
        }

        /// <summary>
        /// Determine the intersection point of a line and a line segment (if it exists).
        /// </summary>
        /// <param name="x1">The x component of the start point of the line.</param>
        /// <param name="y1">The y component of the start point of the line.</param>
        /// <param name="x2">The x component of the end point of the line.</param>
        /// <param name="y2">The y component of the end point of the line.</param>
        /// <param name="x3">The x component of the start point of the line segment.</param>
        /// <param name="y3">The y component of the start point of the line segment.</param>
        /// <param name="x4">The x component of the end point of the line segment.</param>
        /// <param name="y4">The y component of the end point of the line segment.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? LineToSegmentPOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            Vector2? poi = LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!poi.HasValue || !IsPointInSegmentRange(poi.Value.X, poi.Value.Y, x3, y3, x4, y4))
                return null;
            return poi;
        }

        /// <summary>
        /// Determine the intersection point of a line and a ray (if it exists).
        /// </summary>
        /// <param name="x1">The x component of the start point of the line.</param>
        /// <param name="y1">The y component of the start point of the line.</param>
        /// <param name="x2">The x component of the end point of the line.</param>
        /// <param name="y2">The y component of the end point of the line.</param>
        /// <param name="x3">The x component of the start point of the ray.</param>
        /// <param name="y3">The y component of the start point of the ray.</param>
        /// <param name="x4">The x component of the end point of the ray.</param>
        /// <param name="y4">The y component of the end point of the ray.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? LineToRayPOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            Vector2? poi = LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!poi.HasValue || !IsPointInRayRange(poi.Value.X, poi.Value.Y, x3, y3, x4, y4))
                return null;
            return poi;
        }

        /// <summary>
        /// Determine the intersection point of two rays (if it exists, otherwise null).
        /// </summary>
        /// <param name="x1">The x component of the start point of the first ray.</param>
        /// <param name="y1">The y component of the start point of the first ray.</param>
        /// <param name="x2">The x component of the end point of the first ray.</param>
        /// <param name="y2">The y component of the end point of the first ray.</param>
        /// <param name="x3">The x component of the start point of the second ray.</param>
        /// <param name="y3">The y component of the start point of the second ray.</param>
        /// <param name="x4">The x component of the end point of the second ray.</param>
        /// <param name="y4">The y component of the end point of the second ray.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? RayToRayPOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            Vector2? poi = LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!poi.HasValue ||
                !IsPointInRayRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2) ||
                !IsPointInRayRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2))
                return null;
            return poi;
        }

        /// <summary>
        /// Determine the intersection point of a ray and a line segment (if it exists).
        /// </summary>
        /// <param name="x1">The x component of the start point of the ray.</param>
        /// <param name="y1">The y component of the start point of the ray.</param>
        /// <param name="x2">The x component of the end point of the ray.</param>
        /// <param name="y2">The y component of the end point of the ray.</param>
        /// <param name="x3">The x component of the start point of the line segment.</param>
        /// <param name="y3">The y component of the start point of the line segment.</param>
        /// <param name="x4">The x component of the end point of the line segment.</param>
        /// <param name="y4">The y component of the end point of the line segment.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? RayToSegmentPOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            Vector2? poi = LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!poi.HasValue ||
                !IsPointInRayRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2) ||
                !IsPointInSegmentRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2))
                return null;
            return poi;
        }

        /// <summary>
        /// Determine the intersection point of two line segments (if it exists, otherwise null).
        /// </summary>
        /// <param name="x1">The x component of the start point of the first line segment.</param>
        /// <param name="y1">The y component of the start point of the first line segment.</param>
        /// <param name="x2">The x component of the end point of the first line segment.</param>
        /// <param name="y2">The y component of the end point of the first line segment.</param>
        /// <param name="x3">The x component of the start point of the second line segment.</param>
        /// <param name="y3">The y component of the start point of the second line segment.</param>
        /// <param name="x4">The x component of the end point of the second line segment.</param>
        /// <param name="y4">The y component of the end point of the second line segment.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? SegmentToSegmentPOI(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            Vector2? poi = LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!poi.HasValue ||
                !IsPointInSegmentRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2) ||
                !IsPointInSegmentRange(poi.Value.X, poi.Value.Y, x1, y1, x2, y2))
                return null;
            return poi;
        }

        /// <summary>
        /// Check if two lines are intersecting or not.
        /// </summary>
        /// <param name="x1">The x component of the start point of the first line.</param>
        /// <param name="y1">The y component of the start point of the first line.</param>
        /// <param name="x2">The x component of the end point of the first line.</param>
        /// <param name="y2">The y component of the end point of the first line.</param>
        /// <param name="x3">The x component of the start point of the second line.</param>
        /// <param name="y3">The y component of the start point of the second line.</param>
        /// <param name="x4">The x component of the end point of the second line.</param>
        /// <param name="y4">The y component of the end point of the second line.</param>
        public static bool AreLinesIntersecting(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4
            )
        {
            return (LineToLinePOI(x1, y1, x2, y2, x3, y3, x4, y4).HasValue);
        }

        /// <summary>
        /// Check if two rays are intersecting or not.
        /// </summary>
        /// <param name="x1">The x component of the start point of the first ray.</param>
        /// <param name="y1">The y component of the start point of the first ray.</param>
        /// <param name="x2">The x component of the end point of the first ray.</param>
        /// <param name="y2">The y component of the end point of the first ray.</param>
        /// <param name="x3">The x component of the start point of the second ray.</param>
        /// <param name="y3">The y component of the start point of the second ray.</param>
        /// <param name="x4">The x component of the end point of the second ray.</param>
        /// <param name="y4">The y component of the end point of the second ray.</param>
        public static bool AreRaysIntersecting(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4
            )
        {
            return (RayToRayPOI(x1, y1, x2, y2, x3, y3, x4, y4).HasValue);
        }

        /// <summary>
        /// Check if two line segments are intersecting or not.
        /// </summary>
        /// <param name="x1">The x component of the start point of the first line segment.</param>
        /// <param name="y1">The y component of the start point of the first line segment.</param>
        /// <param name="x2">The x component of the end point of the first line segment.</param>
        /// <param name="y2">The y component of the end point of the first line segment.</param>
        /// <param name="x3">The x component of the start point of the second line segment.</param>
        /// <param name="y3">The y component of the start point of the second line segment.</param>
        /// <param name="x4">The x component of the end point of the second line segment.</param>
        /// <param name="y4">The y component of the end point of the second line segment.</param>
        public static bool AreSegmentsIntersecting(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4
            )
        {
            return (SegmentToSegmentPOI(x1, y1, x2, y2, x3, y3, x4, y4).HasValue);
        }

        private static Vector2[] Edges(Vector2[] coords)
        {
            Vector2[] edges = new Vector2[coords.Length];
            edges[0] = coords[coords.Length] - coords[0];
            for (int i = 0; i < coords.Length - 1; i++){
                edges[i + 1] = coords[i + 1] - coords[i];
            }
            return edges;
        }

        private static float[] ProjectPoly(Vector2[] polygon, Vector2 axis)
        {
            var _projections = from v in polygon
                               select Vector2.Dot(v, axis);
            float[] projections = _projections.ToArray<float>();
            float[] min_max = new float[2] {
                projections.Min(),
                projections.Max()
            };
            return min_max;
        }

        private static Vector2 EdgeToAxis(Vector2 edge) 
        {
            edge.Normalize();
            return new Vector2(edge.Y, -edge.X);
        }

        /// <summary>
        /// Perform a Separating Axis Theorem check on two convex polygons
        /// to determine if they are intersecting or not.
        /// </summary>
        /// <param name="self_coords">The first polygon.</param>
        /// <param name="other_coords">The second polygon.</param>
        /// <returns></returns>
        public static bool SeparatingAxisTheorem(Vector2[] self_coords, Vector2[] other_coords)
        {
            Vector2[] edges = new Vector2[self_coords.Length + other_coords.Length];
            Edges(self_coords).CopyTo(edges, 0);
            Edges(other_coords).CopyTo(edges, self_coords.Length);

            Vector2 axis;
            float[] projection_self, projection_other;

            foreach (Vector2 edge in edges) {
                axis = EdgeToAxis(edge);

                projection_self = ProjectPoly(self_coords, axis);
                projection_other = ProjectPoly(other_coords, axis);

                if (!(projection_self[1] > projection_other[0] && projection_self[0] < projection_other[1]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Calculate the intersection point of a circle and a line.
        /// </summary>
        /// <param name="h">The x component of the center of the circle.</param>
        /// <param name="k">The y component of the center of the circle.</param>
        /// <param name="r">The radius of the circle.</param>
        /// <param name="x1">The x component of the start of the line.</param>
        /// <param name="y1">The y component of the start of the line.</param>
        /// <param name="x2">The x component of the end of the line.</param>
        /// <param name="y2">The y component of the end of the line.</param>
        /// <returns></returns>
        public static Vector2[] CircleToLinePOI(
            double h, double k, double r,
            double x1, double y1,
            double x2, double y2)
        {
            if (x1 == x2)
            {
                if (Math.Abs(x1 - h) > r)
                    return new Vector2[] { };
                double py = Math.Sqrt(r * r - Math.Pow(x1 - h, 2) + k);
                double qy = -py;
                return new Vector2[] {
                    new Vector2((float)x1, (float)py),
                    new Vector2((float)x1, (float)qy)
                };
            }
            double m = (y2 - y1) / (x2 - x1);
            double b = y1 - m*x1;

            double A = m * m + 1;
            double B = 2 * (m * b - m * k - h);
            double C = (k * k - r * r + h * h - 2 * b * k + b * b);
            double d = B * B - 4 * A * C;
            if (d < 0)
                return new Vector2[] { };
            else if (d == 0)
            {
                double px = -B / (2 * A);
                double py = y1 - m * px;
                return new Vector2[] { new Vector2((float)px, (float)py) };
            }
            double denominator = 1 / (2 * A);
            double ax = denominator * (-B + Math.Sqrt(d));
            double bx = denominator * (-B - Math.Sqrt(d));
            double ay = m * ax + b;
            double by = m * bx + b;
            return new Vector2[] { 
                new Vector2((float)ax, (float)ay),
                new Vector2((float)bx, (float)by)
            };
        }

        /// <summary>
        /// Calculate the intersection point of a circle and a ray.
        /// </summary>
        /// <param name="h">The x component of the center of the circle.</param>
        /// <param name="k">The y component of the center of the circle.</param>
        /// <param name="r">The radius of the circle.</param>
        /// <param name="x1">The x component of the start of the ray.</param>
        /// <param name="y1">The y component of the start of the ray.</param>
        /// <param name="x2">The x component of the end of the ray.</param>
        /// <param name="y2">The y component of the end of the ray.</param>
        /// <returns></returns>
        public static Vector2[] CircleToRayPOI(
            double h, double k, double r,
            double x1, double y1,
            double x2, double y2)
        {
            Vector2[] points = CircleToLinePOI(h, k, r, x1, y1, x2, y2);
            Vector2[] _new_points = new Vector2[points.Length];
            int count = 0;
            foreach (Vector2 point in points)
            {
                if (IsPointInRayRange(point.X, point.Y, x1, y1, x2, y2))
                {
                    _new_points[count] = point;
                    count++;
                }
            }
            // This just ensures that the length of the array is no greater
            // than the number of intersection points found.
            Vector2[] new_points = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                new_points[i] = _new_points[i];
            }
            return new_points;
        }

        /// <summary>
        /// Calculate the intersection point of a circle and a line segment.
        /// </summary>
        /// <param name="h">The x component of the center of the circle.</param>
        /// <param name="k">The y component of the center of the circle.</param>
        /// <param name="r">The radius of the circle.</param>
        /// <param name="x1">The x component of the start of the line segment.</param>
        /// <param name="y1">The y component of the start of the line segment.</param>
        /// <param name="x2">The x component of the end of the line segment.</param>
        /// <param name="y2">The y component of the end of the line segment.</param>
        /// <returns></returns>
        public static Vector2[] CircleToSegmentPOI(
            double h, double k, double r,
            double x1, double y1,
            double x2, double y2)
        {
            Vector2[] points = CircleToLinePOI(h, k, r, x1, y1, x2, y2);
            Vector2[] _new_points = new Vector2[points.Length];
            int count = 0;
            foreach (Vector2 point in points)
            {
                if (IsPointInSegmentRange(point.X, point.Y, x1, y1, x2, y2))
                {
                    _new_points[count] = point;
                    count++;
                }
            }
            // This just ensures that the length of the array is no greater
            // than the number of intersection points found.
            Vector2[] new_points = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                new_points[i] = _new_points[i];
            }
            return new_points;
        }
    }
}
