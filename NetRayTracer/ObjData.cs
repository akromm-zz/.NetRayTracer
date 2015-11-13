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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetRayTracer
{
    /// <summary>
    /// Represents the data loaded from a .obj file
    /// </summary>
    public class ObjData
    {
        /// <summary>
        /// Represents a Face in the obj file (we only accept triangles)
        /// </summary>
        public struct Triangle
        {
            /// <summary>
            /// Contains the indices into the vertex, normal, and tex coord lists
            /// </summary>
            public struct Vertex
            {
                /// <summary>
                /// Index into the vertex list
                /// </summary>
                public int vertex;

                /// <summary>
                /// Index into the normal list
                /// </summary>
                public int normal;

                /// <summary>
                /// Index into the texture coordinate list
                /// </summary>
                public int texCoord;
            }

            /// <summary>
            /// Contains the three vertices that make up the face
            /// </summary>
            public Vertex Vert1, Vert2, Vert3;

            /// <summary>
            /// Used to specify which object this face is a part of.
            /// </summary>
            public string ObjectName;

            /// <summary>
            /// Used to specify which group this face is a part of.
            /// </summary>
            public List<string> GroupNames;

            /// <summary>
            /// Used to specify which smoothing group this face is a part of
            /// </summary>
            public string SmoothingGroupName;

            /// <summary>
            /// Used to specify which material is being used
            /// </summary>
            public string Material;
        }

        /// <summary>
        /// Represents a meterial from a .mtl file linked to from a .obj file
        /// </summary>
        public struct Material
        {
            /// <summary>
            /// The name of the material
            /// </summary>
            public string Name;

            /// <summary>
            /// The ambient, diffuse, and specular color. xyz = rgb
            /// </summary>
            public Vector3 Ambient, Diffuse, Specular;

            /// <summary>
            /// The specular coefficient for this material
            /// </summary>
            public float SpecularCoefficient;

            /// <summary>
            /// The transparency of the material
            /// </summary>
            public float Transparency;

            /// <summary>
            /// Path to an ambient texture map
            /// </summary>
            public string AmbientTextureMap;

            /// <summary>
            /// Path to a diffuse texture map
            /// </summary>
            public string DiffuseTextureMap;

            /// <summary>
            /// Path to a specular texture map
            /// </summary>
            public string SpecularTextureMap;

            /// <summary>
            /// Path to specular coefficient map
            /// </summary>
            public string SpecularCoefficientMap;

            /// <summary>
            /// Path to alpha texture map
            /// </summary>
            public string AlphaTextureMap;

            /// <summary>
            /// Path to bump map
            /// </summary>
            public string BumpMap;

            /// <summary>
            /// Path to displacement map
            /// </summary>
            public string DisplacementMap;
        }

        /// <summary>
        /// List of all vertices read from file
        /// </summary>
        public List<Vector3> vertices;

        /// <summary>
        /// List of all normals read from file
        /// </summary>
        public List<Vector3> normals;

        /// <summary>
        /// List of all texture coordinates read from file
        /// </summary>
        public List<Vector3> texCoords;

        /// <summary>
        /// List of all faces/Trianlges read from file
        /// </summary>
        public List<Triangle> faces;

        /// <summary>
        /// List of all materials loaded from file
        /// </summary>
        public List<Material> materials;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjData"/> class.
        /// </summary>
        public ObjData()
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texCoords = new List<Vector3>();
            faces = new List<Triangle>();
            materials = new List<Material>();
        }

        /// <summary>
        /// Load a .obj file (and any .mtl files referenced within).
        /// </summary>
        /// <param name="filepath">The path to the .obj file</param>
        /// <returns>The loaded object data</returns>
        public static ObjData LoadFile(string filepath)
        {
            ObjData data = new ObjData();

            if (!File.Exists(filepath))
            {
                throw new ArgumentException(string.Format("The path '{0}' does not exist", filepath));
            }

            // List of state variables
            List<string> currentGroups = null;
            string currentObject = string.Empty;
            string currentSmoothingGroup = string.Empty;
            string currentMaterial = string.Empty;

            // List of material files we will need to load
            List<string> materialFiles = new List<string>();

            // Load up the file for reading
            using (FileStream stream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                // Read through the whole file
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length == 0)
                        continue;

                    switch (tokens[0])
                    {
                        case "v": // Vertex
                            {
                                float x = float.Parse(tokens[1]);
                                float y = float.Parse(tokens[2]);
                                float z = float.Parse(tokens[3]);

                                data.vertices.Add(new Vector3(x, y, z));
                            }
                            break;
                        case "vn": // Vertex Normal
                            {
                                float x = float.Parse(tokens[1]);
                                float y = float.Parse(tokens[2]);
                                float z = float.Parse(tokens[3]);

                                data.normals.Add(new Vector3(x, y, z));
                            }
                            break;
                        case "vt": // Vertex Texture Coord
                            {
                                float u = float.Parse(tokens[1]);
                                float v = float.Parse(tokens[2]);
                                float w = 1;
                                if (tokens.Length > 3)
                                {
                                    w = float.Parse(tokens[3]);
                                }
                                data.texCoords.Add(new Vector3(u, v, w));
                            }
                            break;
                        case "f": // Face
                            {
                                if (tokens.Length > 4)
                                {
                                    throw new FormatException("Only support loading .obj files with triangle faces");
                                }

                                Triangle t = ParseFaceData(tokens);
                                t.GroupNames = currentGroups;
                                t.ObjectName = currentObject;
                                t.SmoothingGroupName = currentSmoothingGroup;
                                t.Material = currentMaterial;
                                data.faces.Add(t);
                            }
                            break;
                        case "mtllib":
                            {
                                if (materialFiles == null)
                                {
                                    materialFiles = new List<string>();
                                }
                                materialFiles.AddRange(tokens.Reverse().Take(tokens.Length - 1));
                            }
                            break;
                        case "usemtl":
                            {
                                currentMaterial = tokens[1];
                            }
                            break;
                        case "o":
                            currentObject = tokens[1];
                            break;
                        case "g":
                            currentGroups = new List<string>();

                            // reverse the list of groups, and take all but the last element ("g").
                            currentGroups.AddRange(tokens.Reverse().Take(tokens.Length - 1));
                            break;
                    }
                }
            }

            // Load all the material files
            foreach (var m in materialFiles)
            {
                data.materials.AddRange(LoadMaterials(m));
            }

            return data;
        }

        /// <summary>
        /// Parses a line of face data: "f a/b/c d/e/f g/h/i".
        /// </summary>
        /// <param name="tokens">The line containing the face data split on spaces</param>
        /// <returns>The triangle that the face makes</returns>
        private static Triangle ParseFaceData(string[] tokens)
        {
            // tokens[0] will be "f"
            Triangle t = new Triangle();

            t.Vert1 = ParseFaceVertex(tokens[1]);
            t.Vert2 = ParseFaceVertex(tokens[2]);
            t.Vert3 = ParseFaceVertex(tokens[3]);

            return t;
        }

        /// <summary>
        /// Parse a face's vertex data (eg: 2/5/3 -> vert/texC/norm).
        /// </summary>
        /// <param name="data">The data containing the </param>
        /// <returns>The parsed vertex information</returns>
        private static Triangle.Vertex ParseFaceVertex(string data)
        {
            Triangle.Vertex vert = new Triangle.Vertex();
            var d = data.Split('/');

            vert.vertex = int.Parse(d[0]);
            if (data.Length > 1)
            {
                // Its possible to not specify a texture coord in order to still supply a normal
                // eg: 1//5 -> Vertex index 1, no tex coord, normal index 5
                if (d[1] != string.Empty)
                {
                    vert.texCoord = int.Parse(d[1]);
                }
            }
            if (data.Length > 2)
            {
                vert.normal = int.Parse(d[2]);
            }

            return vert;
        }

        /// <summary>
        /// Loads all the materials in a material file
        /// </summary>
        /// <param name="file">The path to the file to load and parse</param>
        /// <returns>A list of all <see cref="Material"/> defined in the file</returns>
        private static List<Material> LoadMaterials(string file)
        {
            List<Material> materials = new List<Material>();

            if (!File.Exists(file))
            {
                throw new ArgumentException(string.Format("The path '{0}' does not exist", file));
            }

            Material currentMaterial;

            // Load and parse the file
            using (FileStream stream = File.OpenRead(file))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(' ');

                    switch (tokens[0])
                    {
                        case "newmtl":
                            materials.Add(new Material());
                            currentMaterial = materials.Last();
                            currentMaterial.Name = tokens[1];
                            break;
                        case "Ka":
                            currentMaterial.Ambient = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Kd":
                            currentMaterial.Diffuse = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Ks":
                            currentMaterial.Specular = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Ns":
                            currentMaterial.SpecularCoefficient = float.Parse(tokens[1]);
                            break;
                        case "d":
                            currentMaterial.Transparency = 1.0f - float.Parse(tokens[1]);
                            break;
                        case "Tr":
                            currentMaterial.Transparency = float.Parse(tokens[1]);
                            break;
                        case "map_Ka":
                            currentMaterial.AmbientTextureMap = tokens[1];
                            break;
                        case "map_Kd":
                            currentMaterial.DiffuseTextureMap = tokens[1];
                            break;
                        case "map_Ks":
                            currentMaterial.SpecularTextureMap = tokens[1];
                            break;
                        case "map_Ns":
                            currentMaterial.SpecularCoefficientMap = tokens[1];
                            break;
                        case "map_d":
                            currentMaterial.AlphaTextureMap = tokens[1];
                            break;
                        case "disp":
                            currentMaterial.DisplacementMap = tokens[1];
                            break;
                        case "map_bump":
                        case "bump":
                            currentMaterial.BumpMap = tokens[1];
                            break;
                    }
                }
            }

            return materials;
        }
    }
}
