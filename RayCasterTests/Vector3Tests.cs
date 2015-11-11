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

using Xunit;
using NetRayTracer;

namespace RayCasterTests
{
    public class Vector3Tests
    {
        [Fact]
        public void Vector3Initialize()
        {
            Vector3 a = new Vector3(1, -2, 3);
            Assert.Equal(1, a.X);
            Assert.Equal(-2, a.Y);
            Assert.Equal(3, a.Z);

            Vector3 b = new Vector3(a);
            Assert.Equal(1, b.X);
            Assert.Equal(-2, b.Y);
            Assert.Equal(3, b.Z);
        }

        [Fact]
        public void Vector3Add()
        {
            Vector3 a = new Vector3(2, 3, 4);
            Vector3 b = new Vector3(5, 6, 7);

            Vector3 c = a + b;
            Assert.Equal(7, c.X);
            Assert.Equal(9, c.Y);
            Assert.Equal(11, c.Z);
        }

        [Fact]
        public void Vector3Subtract()
        {
            Vector3 a = new Vector3(2, 3, 4);
            Vector3 b = new Vector3(5, 7, 9);

            Vector3 c = a - b;
            Assert.Equal(-3, c.X);
            Assert.Equal(-4, c.Y);
            Assert.Equal(-5, c.Z);
        }

        [Fact]
        public void Vector3ScalarMultiply()
        {
            Vector3 a = new Vector3(2, 3, 4);

            Vector3 c = a * 5;
            Assert.Equal(10, c.X);
            Assert.Equal(15, c.Y);
            Assert.Equal(20, c.Z);
        }

        [Fact]
        public void Vector3DotProduct()
        {
            Vector3 a = new Vector3(2, 3, 4);
            Vector3 b = new Vector3(5, 6, 7);

            float res = Vector3.Dot(a, b);
            Assert.Equal(56, res);
        }

        [Fact]
        public void Vector3CrossProduct()
        {
            Vector3 a = new Vector3(2, 3, 4);
            Vector3 b = new Vector3(5, 6, 7);

            Vector3 c = Vector3.Cross(a, b);
            Vector3 fact = new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                -(a.X * b.Z) + a.Z * b.X,
                a.X * b.Y - a.Y * b.X);
            Assert.Equal(fact, c);
        }

        [Fact]
        public void Vector3Reflection()
        {
            Vector3 a = new Vector3(1, -1, 0);
            Vector3 n = new Vector3(0, -1, 0);

            Vector3 r = Vector3.Reflection(a, n);
            Assert.Equal(new Vector3(1, 1, 0), r);
        }
    }
}
