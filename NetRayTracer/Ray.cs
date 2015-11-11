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
    /// Represents a ray used for ray tracing
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// The origin of the ray
        /// </summary>
        private Vector3 _origin;

        /// <summary>
        /// The direction the ray is going
        /// </summary>
        private Vector3 _direction;
        
        /// <summary>
        /// Gets the origin of the ray
        /// </summary>
        public Vector3 Origin
        {
            get { return _origin; }
        }

        /// <summary>
        /// Gets the direction of the ray
        /// </summary>
        public Vector3 Direction
        {
            get { return _direction; }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Ray"/> class.
        /// </summary>
        /// <param name="origin">The origin of the ray</param>
        /// <param name="direction">The direction the ray is travelling</param>
        public Ray(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction.Normalized;
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