using System;
using System.Collections.Generic;
using RenderProject.MyMath;

namespace RenderProject.Graphics
{
    class Model
    {

        #region Inner classes
        
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

        public Dictionary<int, Quaternion> vertexes;
        public Dictionary<int, Vector3> textureVertexes;
        public Dictionary<int, Vector3> normalsVertexes;
        public Dictionary<int, Face> faces;

        #endregion

        #region Public interfaces

        public void Load(string path, float scale = 1)
        {
            vertexes = new Dictionary<int, Quaternion>();
            textureVertexes = new Dictionary<int, Vector3>();
            normalsVertexes = new Dictionary<int, Vector3>();
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
                        x = float.Parse(splitted[1].Replace(".", ",")) * scale;
                        y = float.Parse(splitted[2].Replace(".", ",")) * scale;
                        z = float.Parse(splitted[3].Replace(".", ",")) * scale;
                        w = splitted.Length > 4 ? float.Parse(splitted[4]) : 1;

                        vertexes.Add(vertexes.Count + 1,
                            new Quaternion(x, y, z, w));
                        break;

                    case "vt":
                        x = float.Parse(splitted[1].Replace(".", ","));
                        y = float.Parse(splitted[2].Replace(".", ","));
                        w = splitted.Length > 3 ? float.Parse(splitted[3].Replace(".", ",")) : 0;

                        textureVertexes.Add(textureVertexes.Count + 1, new Vector3(x, y, w));
                        break;

                    case "vn":
                        x = float.Parse(splitted[1].Replace(".", ","));
                        y = float.Parse(splitted[2].Replace(".", ","));
                        z = float.Parse(splitted[3].Replace(".", ","));

                        normalsVertexes.Add(normalsVertexes.Count + 1, new Vector3(x, y, z));
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

        public void LoadTexture(string path)
        {

        }

        #endregion

    }
}
