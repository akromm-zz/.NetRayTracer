using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    public class Material
    {
        public Bitmap Texture { get; set; }
        public Bitmap BumpMap { get; set; }
        public Bitmap DisplacementMap { get; set; }
        public Bitmap AmbientMap { get; set; }
        public Bitmap DiffuseMap { get; set; }
        public Bitmap SpecularMap { get; set; }
        public Bitmap SpecularCoefficientMap { get; set; }
        public Bitmap AlphaMap { get; set; }

        public Vector3 SpecularColor { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public Vector3 AmbientColor { get; set; }
        public float Transparency { get; set; }
        public float SpecularCoefficient { get; set; }
    }
}
