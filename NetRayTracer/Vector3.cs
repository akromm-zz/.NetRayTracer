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
    /// Represents a vector in 3D space
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// The 3 coordinate components
        /// </summary>
        private float _x, _y, _z;

        /// <summary>
        /// Gets or sets the x coordinate
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the y coordinate
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Gets or sets the z coordinate
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class.
        /// </summary>
        /// <param name="other">The other vector to clone</param>
        public Vector3(Vector3 other)
        {
            _x = other.X;
            _y = other.Y;
            _z = other.Z;
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        /// <param name="a">The first vector to add</param>
        /// <param name="b">The second vector to add</param>
        /// <returns>The result of adding the vectors</returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="a">The first part of the subtraction</param>
        /// <param name="b">The second part of the subtraction</param>
        /// <returns>The result of subtracting <paramref name="b"/> from <paramref name="a"/>.</returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Negate the vector (reverse direction)
        /// </summary>
        /// <param name="a">The vector to reverse</param>
        /// <returns>The reversed direction</returns>
        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        /// <summary>
        /// Define the vector-scalar multiplication (vector scaling)
        /// </summary>
        /// <param name="a">The vector part of the multiplication</param>
        /// <param name="s">The scalar part of the multiplication</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator *(Vector3 a, float s)
        {
            return new Vector3(a.X * s, a.Y * s, a.Z * s);
        }

        /// <summary>
        /// Define the scalar-vector multiplication
        /// </summary>
        /// <param name="s">The scalar part of the multiplication</param>
        /// <param name="a">The vector part of the multiplication</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator *(float s, Vector3 a)
        {
            return new Vector3(a.X * s, a.Y * s, a.Z * s);
        }

        /// <summary>
        /// Define scalar-vector division
        /// </summary>
        /// <param name="a">The vector to scale</param>
        /// <param name="s">The scalar to use</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator /(Vector3 a, float s)
        {
            return new Vector3(a.X / s, a.Y / s, a.Z / s);
        }

        /// <summary>
        /// Compares the dimensions of the two vectors to see if they are the same
        /// </summary>
        /// <param name="a">The first part of the equality test</param>
        /// <param name="b">The second part of the equality test</param>
        /// <returns></returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// Checks for inequality
        /// </summary>
        /// <param name="a">The first part of the inequality</param>
        /// <param name="b">The second part of the inequality</param>
        /// <returns>Whether or not they two vectors are inequal</returns>
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Gets the magnitude of this vector
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(_x * _x + _y * _y + _z * _z);
            }
        }

        /// <summary>
        /// Gets the normalized version of this vector
        /// </summary>
        public Vector3 Normalized
        {
            get
            {
                float val = Magnitude;
                return new Vector3(_x / Magnitude, _y / Magnitude, _z / Magnitude);
            }
        }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        /// <returns>A formatted string representing the vector</returns>
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", _x, _y, _z);
        }

        /// <summary>
        /// Performs the dot product of vectors <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        /// <param name="a">The first vector in the dot product</param>
        /// <param name="b">The second vector in the dot product</param>
        /// <returns>The dot product</returns>
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Return the cross product of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first component of the cross product</param>
        /// <param name="b">The second component of the cross product</param>
        /// <returns>The cross product</returns>
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                -(a.X * b.Z) + a.Z * b.X,
                a.X * b.Y - a.Y * b.X);
        }

        /// <summary>
        /// Reflects a vector <paramref name="vec"/> accross the normal vector <paramref name="normal"/>
        /// </summary>
        /// <remarks>The normal vector must be normalized</remarks>
        /// <param name="vec">The vector to reflect</param>
        /// <param name="normal">The normal to reflect the vector over</param>
        /// <returns>The reflected vector</returns>
        public static Vector3 Reflection(Vector3 vec, Vector3 normal)
        {
            //v - 2(v (dot) n) n
            return vec - (2f * Vector3.Dot(vec, normal)) * normal;
        }

        /// <summary>
        /// Gets a vector of length zero
        /// </summary>
        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }

        /// <summary>
        /// Checks whether or not this vector is equal to another
        /// </summary>
        /// <param name="other">The other vector to compare to</param>
        /// <returns>True if the vectors are equal</returns>
        public bool Equals(Vector3 other)
        {
            return this == other;
        }

        /// <summary>
        /// Checks whether or not this vector is equal to another object
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the object is a vector and they are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                Vector3 other = (Vector3)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }

            return false;
        }

        /// <summary>
        /// Gets the hashcode for this vector
        /// </summary>
        /// <returns>The hashcode for this vector</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
