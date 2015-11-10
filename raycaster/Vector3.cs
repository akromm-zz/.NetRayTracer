using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    public class Vector3 : IEquatable<Vector3>
    {
        private float _x, _y, _z;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3(Vector3 other)
        {
            _x = other.X;
            _y = other.Y;
            _z = other.Z;
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Negate the vectorn (reverse direction)
        /// </summary>
        /// <param name="a">The vector to reverse</param>
        /// <returns>The reversed direction</returns>
        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        /// <summary>
        /// Define the scalar-vector multiplication
        /// </summary>
        /// <param name="s">The scalar part of the multiplication</param>
        /// <param name="a">The vector part of the multiplication</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator *(Vector3 a, float s)
        {
            return new Vector3(a.X * s, a.Y * s, a.Z * s);
        }

        /// <summary>
        /// Compares the dimensions of the two vectors to see if they are the same
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// not equals.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
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
        /// The magnitude of this vector
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(_x * _x + _y * _y + _z * _z);
            }
        }

        /// <summary>
        /// The normalized version of this vector
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
        /// <returns></returns>
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
        /// <param name="a"></param>
        /// <param name="b"></param>
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

        public bool Equals(Vector3 other)
        {
            return this == other;
        }
    }
}
