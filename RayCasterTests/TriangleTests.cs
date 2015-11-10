using raycaster;
using Xunit;

namespace RayCasterTests
{
    public class TriangleTests
    {
        [Fact]
        public void TriangleInitialize()
        {
            Vertex a = new Vertex();
            a.Position = new Vector3(0, 1, 0);
            Vertex b = new Vertex();
            b.Position = new Vector3(-1, 0, 0);
            Vertex c = new Vertex();
            c.Position = new Vector3(1, 0, 0);

            Triangle t = new Triangle(a,b,c);

            Assert.Equal(a, t.P0);
            Assert.Equal(b, t.P1);
            Assert.Equal(c, t.P2);
            Assert.Equal(new Vector3(0, 0, 1), t.Normal);
        }
    }
}
