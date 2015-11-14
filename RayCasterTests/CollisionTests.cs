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
    public class CollisionTests
    {
        [Fact]
        public void RayTriangleTest()
        {
            Vector3 origin = new Vector3(0, 0, 0);
            Vector3 direction = new Vector3(1, 0, 0);
            Ray r = new Ray(origin, direction);
            Triangle t = new Triangle(
                new Vertex() { Position = new Vector3(2, 1, 0) },
                new Vertex() { Position = new Vector3(2, 0, -1) },
                new Vertex() { Position = new Vector3(2, 0, 1) });

            float time = -1f;
            bool collides = r.CollidesWith(t, ref time);

            Assert.True(collides);
            Assert.Equal(2f, time);

            Ray r2 = new Ray(origin, -direction);

            collides = r2.CollidesWith(t, ref time);
            Assert.False(collides);
            Assert.Equal(-2f, time);
        }
    }
}
