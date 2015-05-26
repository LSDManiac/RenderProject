using System;
using System.Collections.Generic;

namespace RenderProject.Graphics
{
    class Model
    {

        #region Inner classes

        public class Vertex
        {
            public float x, y, z, w;

            public Vertex(float x, float y, float z, float w = 1)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        }

        public class VertexTexture
        {
            public float u, v, w;

            public VertexTexture(float u, float v, float w = 0)
            {
                this.u = u;
                this.v = v;
                this.w = w;
            }
        }

        public class VertexNormal
        {
            public static VertexNormal CountNormal(Vertex v1, Vertex v2, Vertex v3)
            {
                float x1fn = v2.x - v1.x;
                float y1fn = v2.y - v1.y;
                float z1fn = v2.z - v1.z;

                float x2fn = v3.x - v1.x;
                float y2fn = v3.y - v1.y;
                float z2fn = v3.z - v1.z;
                float nx = y1fn * z2fn - z1fn * y2fn;
                float ny = z1fn * x2fn - x1fn * z2fn;
                float nz = x1fn * y2fn - y1fn * x2fn;
                return new VertexNormal(nx, ny, nz);
            }

            private const double NormalTolerance = 0.1e-37;
            public float x, y, z;

            public VertexNormal(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public void Normalize()
            {
                double module = x*x + y*y + z*z;
                if (Math.Abs(module - 1) < NormalTolerance) return;
                module = Math.Sqrt(module);
                x = (float)(x / module);
                y = (float)(y / module);
                z = (float)(z / module);
            }
        }

        public class Face
        {
            public List<int> vertexes;
            public List<int> textures;
            public List<int> normals;

            public Face()
            {
                vertexes = new List<int>();
                textures = new List<int>();
                normals = new List<int>();
            }
        }

        #endregion

        #region Public fields

        public Dictionary<int, Vertex> vertexes;
        public Dictionary<int, VertexTexture> textureVertexes;
        public Dictionary<int, VertexNormal> normalsVertexes;
        public Dictionary<int, Face> faces;

        #endregion

        #region Public interfaces

        public void Load(string path, float scale = 1)
        {
            vertexes = new Dictionary<int, Vertex>();
            textureVertexes = new Dictionary<int, VertexTexture>();
            normalsVertexes = new Dictionary<int, VertexNormal>();
            faces = new Dictionary<int, Face>();

            string[] lines = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitted = lines[i].Replace("  ", " ").Split(' ');
                if(splitted.Length < 1) continue;
                float x, y, z, w;
                switch (splitted[0])
                {
                    case "v":
                        x = float.Parse(splitted[1]) * scale;
                        y = float.Parse(splitted[2]) * scale;
                        z = float.Parse(splitted[3]) * scale;
                        w = splitted.Length > 4 ? float.Parse(splitted[4]) : 1;

                        vertexes.Add(vertexes.Count + 1,
                            new Vertex(x, y, z, w));
                        break;

                    case "vt":
                        x = float.Parse(splitted[1]);
                        y = float.Parse(splitted[2]);
                        w = splitted.Length > 3 ? float.Parse(splitted[3]) : 0;

                        textureVertexes.Add(textureVertexes.Count + 1, new VertexTexture(x, y, w));
                        break;

                    case "vn":
                        x = float.Parse(splitted[1]);
                        y = float.Parse(splitted[2]);
                        z = float.Parse(splitted[3]);

                        normalsVertexes.Add(normalsVertexes.Count + 1, new VertexNormal(x, y, z));
                        break;

                    case "f":
                        Face nFace = new Face();
                        for (int j = 1; j < splitted.Length; j++)
                        {
                            string[] facePoint = splitted[j].Replace("  ", "").Split('/');
                            if (string.IsNullOrEmpty(facePoint[0])) continue;
                            nFace.vertexes.Add(int.Parse(facePoint[0]));
                            if (facePoint.Length >= 2 && !string.IsNullOrEmpty(facePoint[1]))
                            {
                                nFace.textures.Add(int.Parse(facePoint[1]));
                            }
                            if (facePoint.Length >= 3 && !string.IsNullOrEmpty(facePoint[2]))
                            {
                                nFace.normals.Add(int.Parse(facePoint[2]));
                            }
                        }
                        faces.Add(faces.Count + 1, nFace);
                        break;
                }
            }
        }

        #endregion
    }
}
