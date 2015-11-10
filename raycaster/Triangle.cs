using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    /// <summary>
    /// Triangle defined by 3 vertices defined in counter-clockwise order
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// The points that make up the triangle
        /// </summary>
        private Vector3 _p0, _p1, _p2;

        /// <summary>
        /// The normal for the triangle
        /// </summary>
        private Vector3 _normal;

        public Vector3 P0 { get { return _p0; } }
        public Vector3 P1 { get { return _p1; } }
        public Vector3 P2 { get { return _p2; } }

        public Vector3 Normal { get { return _normal; } }

        /// <summary>
        /// Constructs an instance of the <see cref="Triangle"/> class.  The points
        /// must be defined in counter-clockwise order
        /// </summary>
        /// <param name="p0">First vertex</param>
        /// <param name="p1">Second vertex</param>
        /// <param name="p2">Third vertex</param>
        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;

            _normal = Vector3.Cross((_p2 - _p1), (_p0 - _p1)).Normalized;
        }
    }
}
