
using System;
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
using System.Drawing;

namespace NetRayTracer
{
    /// <summary>
    /// Represents a material attached to a triangle
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Gets or sets the texture for the material
        /// </summary>
        public Bitmap Texture { get; set; }

        /// <summary>
        /// Gets or sets the bump map
        /// </summary>
        public Bitmap BumpMap { get; set; }

        /// <summary>
        /// Gets or sets the displacement map
        /// </summary>
        public Bitmap DisplacementMap { get; set; }

        /// <summary>
        /// Gets or sets the ambient light color map
        /// </summary>
        public Bitmap AmbientMap { get; set; }

        /// <summary>
        /// Gets or sets the diffuse light color map
        /// </summary>
        public Bitmap DiffuseMap { get; set; }

        /// <summary>
        /// Gets or sets the specular light color map
        /// </summary>
        public Bitmap SpecularMap { get; set; }

        /// <summary>
        /// Gets or sets the specular coefficient map
        /// </summary>
        public Bitmap SpecularCoefficientMap { get; set; }

        /// <summary>
        /// Gets or sets the alpha map 
        /// </summary>
        public Bitmap AlphaMap { get; set; }

        /// <summary>
        /// Gets or sets the specular light color
        /// </summary>
        /// <remarks>
        /// This is overriden by the SpecularMap
        /// </remarks>
        public Vector4 SpecularColor { get; set; }

        /// <summary>
        /// Gets or sets the diffuse light color
        /// </summary>
        /// <remarks>
        /// This is overriden by the DiffuseMap
        /// </remarks>
        public Vector4 DiffuseColor { get; set; }

        /// <summary>
        /// Gets or sets the ambient light color
        /// </summary>
        /// <remarks>
        /// This is overriden by the AmbientMap
        /// </remarks>
        public Vector4 AmbientColor { get; set; }

        /// <summary>
        /// Gets or sets the transparency
        /// </summary>
        /// <remarks>
        /// This is overriden by the AlphaMap
        /// </remarks>
        public float Transparency { get; set; }

        /// <summary>
        /// Gets or sets the specular coefficient
        /// </summary>
        /// <remarks>
        /// This is overriden by the SpecularCoefficientMap
        /// </remarks>
        public float SpecularCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the name of this material
        /// </summary>
        public string Name { get; internal set; }

    }
}
