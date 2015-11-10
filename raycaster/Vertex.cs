using System;

namespace raycaster
{
    public class Vertex : IEquatable<Vertex>
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 TexCoord { get; set; }

        public static bool operator ==(Vertex a, Vertex b)
        {
            return
                a.Position == b.Position
                && a.Normal == b.Normal
                && a.TexCoord == b.TexCoord;
        }

        public static bool operator != (Vertex a, Vertex b)
        {
            return !(a == b);
        }

        public bool Equals(Vertex other)
        {
            return this == other;
        }
    }
}
