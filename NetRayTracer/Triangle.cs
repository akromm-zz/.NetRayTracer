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
    /// Triangle defined by 3 vertices defined in counter-clockwise order
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// The Vertices that make up the triangle
        /// </summary>
        private Vertex _p0, _p1, _p2;

        /// <summary>
        /// The normal for the triangle
        /// </summary>
        private Vector3 _normal;

        /// <summary>
        /// Gets the first vertex
        /// </summary>
        public Vertex P0 { get { return _p0; } }

        /// <summary>
        /// Gets the second vertex
        /// </summary>
        public Vertex P1 { get { return _p1; } }

        /// <summary>
        /// Gets the third vertex
        /// </summary>
        public Vertex P2 { get { return _p2; } }

        /// <summary>
        /// Gets the normal for the triangle
        /// </summary>
        public Vector3 Normal { get { return _normal; } }

        /// <summary>
        /// Gets or sets the material for the triangle
        /// </summary>
        public Material Material
        {
            get; set;
        }

        /// <summary>
        /// Constructs an instance of the <see cref="Triangle"/> class.  The points
        /// must be defined in counter-clockwise order
        /// </summary>
        /// <param name="p0">First vertex</param>
        /// <param name="p1">Second vertex</param>
        /// <param name="p2">Third vertex</param>
        public Triangle(Vertex p0, Vertex p1, Vertex p2)
            : this(p0, p1, p2, null)
        { }

        /// <summary>
        /// Constructs an instance of the <see cref="Triangle"/> class.  The points
        /// must be defined in counter-clockwise order
        /// </summary>
        /// <param name="p0">First vertex</param>
        /// <param name="p1">Second vertex</param>
        /// <param name="p2">Third vertex</param>
        /// <param name="mat">The material for the triangle</param>
        public Triangle(Vertex p0, Vertex p1, Vertex p2, Material mat)
        {
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;

            _normal = Vector3.Cross((_p2.Position - _p1.Position), (_p0.Position - _p1.Position)).Normalized;

            Material = mat;
        }
    }
}
