using Xunit;
using raycaster;

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
                new Vector3(2, 1, 0),
                new Vector3(2, 0, 1),
                new Vector3(2, 0, -1));

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
