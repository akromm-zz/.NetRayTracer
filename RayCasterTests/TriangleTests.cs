using raycaster;
using Xunit;

namespace RayCasterTests
{
    public class TriangleTests
    {
        [Fact]
        public void TriangleInitialize()
        {
            Vector3 a = new Vector3(0, 1, 0);
            Vector3 b = new Vector3(-1, 0, 0);
            Vector3 c = new Vector3(1, 0, 0);

            Triangle t = new Triangle(a,b,c);

            Assert.Equal(a, t.P0);
            Assert.Equal(b, t.P1);
            Assert.Equal(c, t.P2);
            Assert.Equal(new Vector3(0, 0, 1), t.Normal);
        }
    }
}
