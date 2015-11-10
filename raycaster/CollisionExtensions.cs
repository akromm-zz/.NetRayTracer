using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    public static class CollisionExtensions
    {
        /// <summary>
        /// How close the ray origin can be to the object being collided with before we ignore it
        /// </summary>
        public static readonly float ProximityTolerance = 0.5f;

        /// <summary>
        /// Determine if a ray <paramref name="r"/> intersects with triangle <paramref name="t"/>, and if so
        /// at what point in time <paramref name="time"/>.
        /// </summary>
        /// <param name="r">The ray being cast</param>
        /// <param name="t">The triangle being checked</param>
        /// <param name="time">The point in time</param>
        /// <returns>Whether or not the ray intersects the triangle</returns>
        public static bool CollidesWith(this Ray r, Triangle t, ref float time)
        {
            // Get the vector from the origin of the array to the first point on the triangle
            Vector3 op = r.Origin - t.P0;

            // Calculate the time to the first point from ray origin
            time = -1;
            time = -Vector3.Dot(op, t.Normal) / Vector3.Dot(r.Direction, t.Normal);

            if (time > ProximityTolerance)
            {
                Vector3 coord = r.PointAtDistance(time);

                // Need to detect if the point is inside of the triangle
                // ((p1-p0) cross (x-p0)) dot n >= 0
                Vector3 cross = Vector3.Cross((t.P1 - t.P0), (coord - t.P0));
                float hit = Vector3.Dot(cross, t.Normal);

                if (hit < 0)
                {
                    return false;
                }

                // Second check
                // ((p2-p1) cross (x-p1)) dot n >= 0
                cross = Vector3.Cross((t.P2 - t.P1), (coord - t.P1));
                hit = Vector3.Dot(cross, t.Normal);

                if (hit < 0)
                {
                    return false;
                }

                // Third check
                // ((p0-p2) cross (x-p2)) dot n >= 0
                cross = Vector3.Cross((t.P0 - t.P2), (coord - t.P2));
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
