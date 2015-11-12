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

using System.Collections.Generic;
using System.Drawing;

namespace NetRayTracer
{
    /// <summary>
    /// Converter for converting Obj data into scene data
    /// </summary>
    public static class ObjToSceneConverter
    {
        /// <summary>
        /// Does the conversion
        /// </summary>
        /// <param name="data">The obj data to convert</param>
        /// <returns>The constructed Scene</returns>
        public static Scene Convert(ObjData data)
        {
            Scene s = new Scene();
            Dictionary<string, Material> materials = new Dictionary<string, Material>();
            Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();

            foreach (var mat in data.materials)
            {
                LoadBitmap(mat.AlphaTextureMap, bitmaps);
                LoadBitmap(mat.AmbientTextureMap, bitmaps);
                LoadBitmap(mat.BumpMap, bitmaps);
                LoadBitmap(mat.DiffuseTextureMap, bitmaps);
                LoadBitmap(mat.DisplacementMap, bitmaps);
                LoadBitmap(mat.SpecularCoefficientMap, bitmaps);
                LoadBitmap(mat.SpecularTextureMap, bitmaps);

                Material m = new Material();
                m.AlphaMap = bitmaps[mat.AlphaTextureMap];
                m.AmbientMap = bitmaps[mat.AmbientTextureMap];
                m.BumpMap = bitmaps[mat.AmbientTextureMap];
                m.DiffuseMap = bitmaps[mat.AmbientTextureMap];
                m.DisplacementMap = bitmaps[mat.AmbientTextureMap];
                m.SpecularCoefficientMap = bitmaps[mat.AmbientTextureMap];
                m.SpecularMap = bitmaps[mat.AmbientTextureMap];
                m.Texture = bitmaps[mat.DiffuseTextureMap];

                m.AmbientColor = mat.Ambient;
                m.DiffuseColor = mat.Diffuse;
                m.SpecularCoefficient = mat.SpecularCoefficient;
                m.SpecularColor = mat.Specular;
                m.Transparency = mat.Transparency;
                m.Name = mat.Name;
                materials[m.Name] = m;
            }

            foreach (var f in data.faces)
            {
                Vertex v1 = new Vertex();
                Vertex v2 = new Vertex();
                Vertex v3 = new Vertex();

                v1.Position = data.vertices[f.Vert1.vertex - 1];
                v2.Position = data.vertices[f.Vert2.vertex - 1];
                v3.Position = data.vertices[f.Vert3.vertex - 1];

                v1.Normal = data.normals[f.Vert1.normal - 1];
                v2.Normal = data.normals[f.Vert2.normal - 1];
                v3.Normal = data.normals[f.Vert3.normal - 1];

                v1.TexCoord = data.texCoords[f.Vert1.texCoord - 1];
                v2.TexCoord = data.texCoords[f.Vert2.texCoord - 1];
                v3.TexCoord = data.texCoords[f.Vert3.texCoord - 1];
                
                Triangle t = new Triangle(v1, v2, v3, materials[f.Material]);

                s.Triangles.Add(t);
            }
            
            return s;
        }

        /// <summary>
        /// Load the bitmap at path <paramref name="path"/> into dictionary <paramref name="bitmaps"/>.
        /// </summary>
        /// <param name="path">The path to load</param>
        /// <param name="bitmaps">The dictionary to store the bitmap in</param>
        private static void LoadBitmap(string path, Dictionary<string, Bitmap> bitmaps)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!bitmaps.ContainsKey(path))
                {
                    Bitmap b = new Bitmap(path);
                    bitmaps[path] = b;
                }
            }
        }
    }
}
