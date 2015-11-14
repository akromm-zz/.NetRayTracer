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

using System;

namespace NetRayTracer
{
    /// <summary>
    /// Represents a vertex in a triangle.  Contains information such as 
    /// - Position
    /// - Normal
    /// - TexCoord
    /// </summary>
    public class Vertex : IEquatable<Vertex>
    {
        /// <summary>
        /// Gets or sets the position of the vertex
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the normal of the vertex
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex
        /// </summary>
        public Vector3 TexCoord { get; set; }

        /// <summary>
        /// Compares two vertices to see if they are the same. Compares all components
        /// </summary>
        /// <param name="a">The first vertex to compare</param>
        /// <param name="b">The second vertex to compare</param>
        /// <returns>Whether or not the two vertices are equal</returns>
        public static bool operator ==(Vertex a, Vertex b)
        {
            if (a.Position != null && b.Position != null)
            {
                if (a.Position != b.Position)
                {
                    return false;
                }
            }
            else if (a.Position != b.Position) //if one is null and the other isnt
            {
                return false;
            }

            if (a.Normal != null && b.Normal != null)
            {
                if (a.Normal != b.Normal)
                {
                    return false;
                }
            }
            else if (a.Normal != b.Normal) //if one is null and the other isnt
            {
                return false;
            }

            if (a.TexCoord != null && b.TexCoord != null)
            {
                if (a.TexCoord != b.TexCoord)
                {
                    return false;
                }
            }
            else if (a.TexCoord != b.TexCoord) //if one is null and the other isnt
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two vertices to see if they are the inequal. Compares all components
        /// </summary>
        /// <param name="a">The first vertex to compare</param>
        /// <param name="b">The second vertex to compare</param>
        /// <returns>Whether or not the two vertices are inequal</returns>
        public static bool operator !=(Vertex a, Vertex b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compares two vertices to see if they are the same. Compares all components
        /// </summary>
        /// <param name="other">The other vertex to compare</param>
        /// <returns>Whether or not the two vertices are equal</returns>
        public bool Equals(Vertex other)
        {
            return this == other;
        }

        /// <summary>
        /// Compares this vertex with another object to see if they are the same. Compares all components
        /// </summary>
        /// <param name="obj">The other object to compare</param>
        /// <returns>Whether or not the two vertices are equal</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Vertex;
            if (other != null)
            {
                return this == other;
            }

            return false;
        }

        /// <summary>
        /// Format the vertex as a string
        /// </summary>
        /// <returns>The formatted string representing the vertex</returns>
        public override string ToString()
        {
            return string.Format("[p:({0}), n:({1}), t:({2})]", 
                Position.ToString(), 
                Normal == null ? "null" : Normal.ToString(), 
                TexCoord == null ? "null" : TexCoord.ToString());
        }

        /// <summary>
        /// Gets the hash code for this vertex
        /// </summary>
        /// <returns>The hashcode for this vertex</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
