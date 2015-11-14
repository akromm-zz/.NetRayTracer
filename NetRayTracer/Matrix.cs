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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRayTracer
{
    /// <summary>
    /// Represents a matrix 
    /// </summary>
    public struct Matrix
    {
        /// <summary>
        /// Stores the Matrix data
        /// </summary>
        private float[,] _data;

        /// <summary>
        /// The number of rows in this matrix
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// the number of columns in this matrix
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="columns">The number of columns to have</param>
        /// <param name="rows">The number of rows to have</param>
        public Matrix(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            _data = new float[columns, rows];
        }

        /// <summary>
        /// Define array style access for the matrix
        /// </summary>
        /// <param name="x">The column to get data from</param>
        /// <param name="y">The row to get data from</param>
        /// <returns>The value stored in column x, and row y</returns>
        public float this[int x, int y]
        {
            get
            {
                return _data[x, y];
            }
            set
            {
                _data[x, y] = value;
            }
        }

        /// <summary>
        /// Multiplies matrix A by matrix B (M = AB)
        /// </summary>
        /// <param name="A">The first part of the matrix multiplication</param>
        /// <param name="B">The second part of the matrix multiplication</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.Columns != B.Rows)
            {
                throw new InvalidOperationException("Matrix A column count must be the same as matrix B row count");
            }

            Matrix m = new Matrix(A.Rows, B.Columns);


            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 0; j < B.Columns; j++)
                {
                    for (int k = 0; k < A.Columns; k++)
                    {
                        m[j, i] += A[k, i] * B[j, k];
                    }
                }
            }

            return m;
        }

        /// <summary>
        /// Does matrix scalar multiplication
        /// </summary>
        /// <param name="scalar">The scalar to use to multiply the matrix with</param>
        /// <param name="A">The matrix to scale</param>
        /// <returns>The result of the multiplication</returns>
        public static Matrix operator *(float scalar, Matrix A)
        {
            Matrix m = new Matrix(A.Columns, A.Rows);

            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 0; j < A.Columns; j++)
                {
                    m[j, i] = scalar * A[j, i];
                }
            }

            return m;
        }

        /// <summary>
        /// Determine if matrix <paramref name="a"/> is the same as matrix <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first matrix to compare</param>
        /// <param name="b">The second matrix to compare</param>
        /// <returns>Whether or not they are equal</returns>
        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows)
            {
                return false;
            }

            for (int i = 0; i < a.Columns; i++)
            {
                for (int j = 0; j < a.Rows; j++)
                {
                    if (a[i, j] != b[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determine whether two matrices are not equal
        /// </summary>
        /// <param name="a">The first matrix to compare</param>
        /// <param name="b">The second matrix to compare</param>
        /// <returns>Whether or not they are inequal</returns>
        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Multiply a Matrix and a vector together result = Ab.  Matrix must have dimension 4x4
        /// </summary>
        /// <param name="A">The matrix to use</param>
        /// <param name="b">The vector to use</param>
        /// <returns>The result of  multiplying the matrix and vector together</returns>
        public static Vector3 operator*(Matrix A, Vector3 b)
        {
            if(A.Columns != A.Rows || A.Columns != 4)
            {
                throw new ArgumentException("Matrix must be of size 4x4");
            }

            Vector3 result = new Vector3();

            result.X = A[0, 0] * b.X + A[1, 0] * b.Y + A[2, 0] * b.Z + A[3, 0];
            result.Y = A[0, 1] * b.X + A[1, 1] * b.Y + A[2, 1] * b.Z + A[3, 1];
            result.Z = A[0, 2] * b.X + A[1, 2] * b.Y + A[2, 2] * b.Z + A[3, 2];
            float w = A[0, 3] * b.X + A[1, 3] * b.Y + A[2, 3] * b.Z + A[3, 3];

            result /= w;

            return result;
        }

        /// <summary>
        /// Creates an identity matrix of size n x n
        /// </summary>
        /// <param name="n">The dimension of the matrix</param>
        /// <returns>The identity matrix</returns>
        public static Matrix Identity(int n)
        {
            Matrix m = new Matrix(n, n);
            for (int x = 0; x < n; x++)
            {
                m[x, x] = 1;
            }

            return m;
        }

        /// <summary>
        /// Transposes a matrix
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public Matrix Transpose(Matrix A)
        {
            Matrix m = new Matrix(A.Rows, A.Columns);

            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 0; j < A.Columns; j++)
                {
                    m[i, j] = A[j, i];
                }
            }

            return m;
        }

        /// <summary>
        /// Override default equals implementation
        /// </summary>
        /// <param name="obj">The other object (matrix) to compare to</param>
        /// <returns>True if this matrix is equal to <paramref name="obj"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                Matrix m = (Matrix)obj;
                return this == m;
            }

            return false;
        }

        /// <summary>
        /// Overrides the default ToString behaviour
        /// </summary>
        /// <returns>The string representation of this matrix</returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            b.Append("[");

            for (int i = 0; i < Rows; i++)
            {
                b.Append(string.Format("r{0}:[", i));

                for (int j = 0; j < Columns; j++)
                {
                    b.Append(this[j, i]);
                    if(j < Columns - 1)
                    {
                        b.Append(",");
                    }
                }

                b.Append("]");
            }

            b.Append("]");

            return b.ToString();
        }

        /// <summary>
        /// Override the get hash code implementation
        /// </summary>
        /// <returns>The hash code of this matrix</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
