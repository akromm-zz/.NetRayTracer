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
    /// Represents the light in a scene
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Get or sets the diffuse color of the light
        /// </summary>
        Vector3 DiffuseColor { get; set;}

        /// <summary>
        /// Gets or sets the specular color of the light
        /// </summary>
        Vector3 SpecularColor { get; set; }

        /// <summary>
        /// Gets or sets the ambient color of the light
        /// </summary>
        Vector3 AmbientColor { get; set; }

        /// <summary>
        /// Gets or sets the location of the light
        /// </summary>
        Vector3 Location { get; set; }
    }
}
