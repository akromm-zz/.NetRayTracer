using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRayTracer;
using Xunit;

namespace RayCasterTests
{
    public class MatrixTests
    {
        [Fact]
        public void MatrixIdentityMultiplication()
        {
            Matrix m1 = Matrix.Identity(2);
            Matrix m2 = Matrix.Identity(2);

            Matrix m1m2 = m1 * m2;

            Assert.True(m1m2 == m1);
        }

        [Fact]
        public void MatrixMultiplication()
        {
            Matrix m1 = new Matrix(2, 2);
            Matrix m2 = new Matrix(2, 2);

            m1[0, 0] = 1;
            m1[0, 1] = 1;
            m1[1, 0] = 0;
            m1[1, 1] = 0;

            m2[0, 0] = 1;
            m2[0, 1] = 1;
            m2[1, 0] = 0;
            m2[1, 1] = 0;

            Matrix m1m2 = m1 * m2;

            Assert.True(m1m2 == m1);

            Matrix m3 = new Matrix(2, 3);
            Matrix m4 = new Matrix(3, 2);

            m3[0, 0] = 0;
            m3[0, 1] = 2;
            m3[0, 2] = 4;
            m3[1, 0] = 1;
            m3[1, 1] = 3;
            m3[1, 2] = 5;

            m4[0, 0] = 0;
            m4[0, 1] = 1;
            m4[1, 0] = 1;
            m4[1, 1] = 0;
            m4[2, 0] = 0;
            m4[2, 1] = 1;

            Matrix m3m4 = m3 * m4;

            Matrix expected = new Matrix(3, 3);
            expected[0, 0] = 1;
            expected[0, 1] = 3;
            expected[0, 2] = 5;
            expected[1, 0] = 0;
            expected[1, 1] = 2;
            expected[1, 2] = 4;
            expected[2, 0] = 1;
            expected[2, 1] = 3;
            expected[2, 2] = 5;

            Assert.True(expected == m3m4);
        }

        [Fact]
        public void MatrixVectorMultiplication()
        {
            Matrix m = new Matrix(4, 4);

            m[0, 0] = 1;
            m[0, 1] = 0;
            m[0, 2] = 1;
            m[0, 3] = 0;

            m[1, 0] = 0;
            m[1, 1] = 1;
            m[1, 2] = 0;
            m[1, 3] = 1;

            m[2, 0] = 1;
            m[2, 1] = 0;
            m[2, 2] = 1;
            m[2, 3] = 0;

            m[3, 0] = 0;
            m[3, 1] = 0;
            m[3, 2] = 0;
            m[3, 3] = 0;

            Vector3 vec = new Vector3(1, 2, 3);

            Vector3 expected = new Vector3(2, 1, 2);

            Vector3 result = m * vec;

            Assert.True(expected == result);
        }

        [Fact]
        public void MatrixLookAt()
        {
            // This should leave coordinate system untouched
            Matrix m = Matrix.LookAt(new Vector3(0, 1, 0), Vector3.Zero, new Vector3(0, 0, -1));
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 up2 = m * up;
            Assert.True(up == up2);



        }

    }
}
