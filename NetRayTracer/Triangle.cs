
using System;
using System.Drawing;
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
        /// The surface area of the triangle
        /// </summary>
        private float _area;

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

            _normal = Vector3.Cross((_p2.Position - _p1.Position), (_p0.Position - _p1.Position));
            _area = _normal.Magnitude;
            _normal = _normal.Normalized;

            Material = mat;
        }

        /// <summary>
        /// Gets the color of the triangle at the given position
        /// </summary>
        /// <param name="closestPosition">The position to get the color at</param>
        /// <returns>The color at the given position</returns>
        internal Vector4 GetColor(Vector3 closestPosition)
        {
            Vector4 c = new Vector4(1,1,1,1);

            if(Material != null)
            {
                // convert to barycentric coordinates for the triangle
                float tu, tv;
                GetTextureCoordinates(closestPosition, out tu, out tv);

                // If the texture coordinate is > 1.0f or < 1.0f then wrap it
                tu %= 1.0f;
                tv %= 1.0f;

                // TODO: Get the color contributions from all the different material properties
                Bitmap diffuse = Material.DiffuseMap;
                Color tc = diffuse.GetPixel((int)(tu * diffuse.Width), (int)(tv * diffuse.Height));
                c.X = tc.R / 255f;
                c.Y = tc.G / 255f;
                c.Z = tc.B / 255f;
                c.W = tc.A / 255f;
            }

            return c;
        }

        /// <summary>
        /// Gets the specular coefficient at the given point
        /// </summary>
        /// <param name="pos">The position to get the specular coefficient</param>
        /// <returns>The specular coefficient</returns>
        internal float GetSpecularCoefficient(Vector3 pos)
        {
            if (Material != null)
            {
                if (Material.SpecularCoefficientMap != null)
                {
                    float tu, tv;
                    GetTextureCoordinates(pos, out tu, out tv);
                    return Material.SpecularCoefficientMap.GetPixel(
                        (int)(tu * Material.SpecularCoefficientMap.Width),
                        (int)(tv * Material.SpecularCoefficientMap.Height)).R / 255f;
                }

                return Material.SpecularCoefficient;
            }

            return 1;
        }

        /// <summary>
        /// Gets the texture coordianates of a triange at a given point
        /// </summary>
        /// <param name="closestPosition">the position at which to get the texture coordinates</param>
        /// <param name="tu">The texture coordinate u</param>
        /// <param name="tv">The texture coordinate v</param>
        private void GetTextureCoordinates(Vector3 closestPosition, out float tu, out float tv)
        {
            Vector3 p0p2 = P0.Position - P2.Position;
            Vector3 p1p2 = P1.Position - P2.Position;
            Vector3 p1p0 = P1.Position - P0.Position;
            Vector3 xp0 = closestPosition - P0.Position;
            Vector3 xp1 = closestPosition - P1.Position;
            Vector3 xp2 = closestPosition - P2.Position;

            // calculate the weight of the contribution of each vertex to the cooridnates
            float weight0 = Vector3.Cross(p1p2, xp1).Magnitude / _area;
            float weight1 = Vector3.Cross(p0p2, xp1).Magnitude / _area;
            float weight2 = Vector3.Cross(p1p0, xp1).Magnitude / _area;

            // calculate the interpolated coordinate
            tu = P0.TexCoord.X * weight0 + P1.TexCoord.X * weight1 + P2.TexCoord.X * weight2;
            tv = P0.TexCoord.Y * weight0 + P1.TexCoord.Y * weight1 + P2.TexCoord.Y * weight2;
        }
    }
}
