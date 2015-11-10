using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    public class Ray
    {
        private Vector3 _origin;
        private Vector3 _direction;
        private float _length;

        public Vector3 Origin
        {
            get { return _origin; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
        }

        public float Length
        {
            get { return _length; }
        }

        public Ray(Vector3 origin, Vector3 direction)
            : this(origin, direction, float.MaxValue)
        { }

        public Ray(Vector3 origin, Vector3 direction, float length)
        {
            _origin = origin;
            _direction = direction.Normalized;
            _length = length;
        }

        /// <summary>
        /// Get the point on this ray at distance <paramref name="t"/> from the origin
        /// </summary>
        /// <param name="t">The distance along the ray</param>
        /// <returns>The point at the given distance</returns>
        public Vector3 PointAtDistance(float t)
        {
            return _origin + _direction * t;
        }
    }
}
