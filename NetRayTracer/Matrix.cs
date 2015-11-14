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
    public class Matrix
    {
        /// <summary>
        /// Stores the Matrix data
        /// </summary>
        private float[,] _data;

        public int Rows { get; private set; }

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
        /// <returns></returns>
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
        /// Multiplies matrix A by matrix B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
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
        /// Override default equals implementation
        /// </summary>
        /// <param name="obj">The other object (matrix) to compare to</param>
        /// <returns>True if this matrix is equal to <paramref name="obj"/>.</returns>
        public override bool Equals(object obj)
        {
            var m = obj as Matrix;
            if (m != null)
            {
                return this == m;
            }

            return false;
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
    }
}
