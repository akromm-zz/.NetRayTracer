using Xunit;
using raycaster;

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
