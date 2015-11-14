/// Copyright (c) 2015 Adam Kromm
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.

namespace NetRayTracer
{
    /// <summary>
    /// Contains extension methods for doing collision detection
    /// </summary>
    public static class CollisionExtensions
    {
        /// <summary>
        /// How close the ray origin can be to the object being collided with before we ignore it
        /// </summary>
        public static readonly float ProximityTolerance = 0.5f;

        /// <summary>
        /// Determine if a ray <paramref name="r"/> intersects with triangle <paramref name="t"/>, and if so
        /// at what point in time <paramref name="time"/>.  Backfaces are ignored (time is negative number)
        /// </summary>
        /// <param name="r">The ray being cast</param>
        /// <param name="t">The triangle being checked</param>
        /// <param name="time">The point in time</param>
        /// <returns>Whether or not the ray intersects the triangle</returns>
        public static bool CollidesWith(this Ray r, Triangle t, ref float time)
        {
            // Get the vector from the origin of the array to the first point on the triangle
            Vector3 op = r.Origin - t.P0.Position;

            // Calculate the time to the first point from ray origin
            time = -1;
            float opt = Vector3.Dot(op, t.Normal);

            // We need to detect backfaces for when doing refractions
            //if(opt < 0)
            //{
                // Ignore backfaces
            //    return false;
            //}

            time = -opt / Vector3.Dot(r.Direction, t.Normal);
            
            if (time > ProximityTolerance && !float.IsInfinity(time))
            {
                Vector3 coord = r.PointAtDistance(time);

                // Need to detect if the point is inside of the triangle
                // ((p1-p0) cross (x-p0)) dot n >= 0
                Vector3 cross = Vector3.Cross((t.P1.Position - t.P0.Position), (coord - t.P0.Position));
                float hit = Vector3.Dot(cross, t.Normal);

                if (hit < 0)
                {
                    return false;
                }

                // Second check
                // ((p2-p1) cross (x-p1)) dot n >= 0
                cross = Vector3.Cross((t.P2.Position - t.P1.Position), (coord - t.P1.Position));
                hit = Vector3.Dot(cross, t.Normal);

                if (hit < 0)
                {
                    return false;
                }

                // Third check
                // ((p0-p2) cross (x-p2)) dot n >= 0
                cross = Vector3.Cross((t.P0.Position - t.P2.Position), (coord - t.P2.Position));
                hit = Vector3.Dot(cross, t.Normal);

                if (hit < 0)
                {
                    return false;
                }

                // At this point we have determined that the point is inside the triangle
                return true;
            }

            return false;
        }
    }
}
