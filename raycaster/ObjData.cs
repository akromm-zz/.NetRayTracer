using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raycaster
{
    public class ObjData
    {
        public struct Triangle
        {
            public struct Vertex
            {
                public int v, n, t;
            }

            public Vertex vert1, vert2, vert3;

            // used to specify which object this face is a part of.
            public string objectName;

            // used to specify which group this face is a part of.
            public List<string> groupNames;

            // used to specify which smoothing group this face is a part of
            public string smoothingGroupName;

            // used to specify which material is being used
            public string material;
        }

        public struct Material
        {
            public string name;
            public Vector3 ambient, diffuse, specular;
            public float specularCoefficient;
            public float transparency;
            public string ambientTextureMap;
            public string diffuseTextureMap;
            public string specularTetureMap;
            public string specularCoefficientMap;
            public string alphaTextureMap;
            public string bumpMap;
            public string displacementMap;
        }

        private List<Vector3> _vertices;
        private List<Vector3> _normals;
        private List<Vector3> _texCoords;
        private List<Triangle> _faces;
        private List<Material> _materials;

        public ObjData()
        {
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _texCoords = new List<Vector3>();
            _faces = new List<Triangle>();
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
                throw new ArgumentException("The path '{0}' does not exist", filepath);
            }

            List<string> currentGroups = null;
            string currentObject = string.Empty;
            string currentSmoothingGroup = string.Empty;
            string currentMaterial = string.Empty;
            List<string> materialFiles = null;

            using (FileStream stream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                // Read through the whole file
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(' ');

                    switch (tokens[0])
                    {
                        case "v": // Vertex
                            {
                                float x = float.Parse(tokens[1]);
                                float y = float.Parse(tokens[2]);
                                float z = float.Parse(tokens[3]);

                                data._vertices.Add(new Vector3(x, y, z));
                            }
                            break;
                        case "vn": // Vertex Normal
                            {
                                float x = float.Parse(tokens[1]);
                                float y = float.Parse(tokens[2]);
                                float z = float.Parse(tokens[3]);

                                data._normals.Add(new Vector3(x, y, z));
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
                                data._texCoords.Add(new Vector3(u, v, w));
                            }
                            break;
                        case "f": // Face
                            {
                                if (tokens.Length > 4)
                                {
                                    throw new FormatException("Only support loading .obj files with triangle faces");
                                }

                                Triangle t = ParseFaceData(tokens);
                                t.groupNames = currentGroups;
                                t.objectName = currentObject;
                                t.smoothingGroupName = currentSmoothingGroup;
                                t.material = currentMaterial;
                                data._faces.Add(t);
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

            foreach (var m in materialFiles)
            {
                data._materials.AddRange(LoadMaterials(m));
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

            t.vert1 = ParseFaceVertex(tokens[1]);
            t.vert2 = ParseFaceVertex(tokens[2]);
            t.vert3 = ParseFaceVertex(tokens[3]);

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

            vert.v = int.Parse(d[0]);
            if (data.Length > 1)
            {
                // Its possible to not specify a texture coord in order to still supply a normal
                // eg: 1//5 -> Vertex index 1, no tex coord, normal index 5
                if (d[1] != string.Empty)
                {
                    vert.t = int.Parse(d[1]);
                }
            }
            if (data.Length > 2)
            {
                vert.n = int.Parse(d[2]);
            }

            return vert;
        }

        private static List<Material> LoadMaterials(string file)
        {
            List<Material> materials = new List<Material>();

            if (!File.Exists(file))
            {
                throw new ArgumentException("The path '{0}' does not exist", file);
            }

            Material currentMaterial;

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
                            currentMaterial.name = tokens[1];
                            break;
                        case "Ka":
                            currentMaterial.ambient = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Kd":
                            currentMaterial.diffuse = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Ks":
                            currentMaterial.specular = new Vector3(
                                float.Parse(tokens[1]),
                                float.Parse(tokens[2]),
                                float.Parse(tokens[3]));
                            break;
                        case "Ns":
                            currentMaterial.specularCoefficient = float.Parse(tokens[1]);
                            break;
                        case "d":
                            currentMaterial.transparency = 1.0f - float.Parse(tokens[1]);
                            break;
                        case "Tr":
                            currentMaterial.transparency = float.Parse(tokens[1]);
                            break;
                        case "map_Ka":
                            currentMaterial.ambientTextureMap = tokens[1];
                            break;
                        case "map_Kd":
                            currentMaterial.diffuseTextureMap = tokens[1];
                            break;
                        case "map_Ks":
                            currentMaterial.specularTetureMap = tokens[1];
                            break;
                        case "map_Ns":
                            currentMaterial.specularCoefficientMap = tokens[1];
                            break;
                        case "map_d":
                            currentMaterial.alphaTextureMap = tokens[1];
                            break;
                        case "disp":
                            currentMaterial.displacementMap = tokens[1];
                            break;
                        case "map_bump":
                        case "bump":
                            currentMaterial.bumpMap = tokens[1];
                            break;
                    }
                }
            }

            return materials;
        }
    }
}
