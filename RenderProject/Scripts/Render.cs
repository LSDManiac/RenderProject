using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using RenderProject.Graphics;
using RenderProject.MyMath;

namespace RenderProject
{
    class Render
    {
        public static void Main()
        {
            int gap = 50;
            int depth = 10;
            int width = 1200;
            int height = 1200;

            double textureShift = 0;
            double textureCountModelShift = 0;

            Bitmap bmp = new Bitmap(width + gap, height + gap);

            Model model = new Model();

            //             D:\\Projects\\RenderProject\\RenderProject\\Models
            string path = "D:\\Projects\\RenderProject\\RenderProject\\Models\\";
            //string path = "E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\";

            model.Load(path + "head.obj");

            Bitmap texture = new Bitmap(path + "head_diffuse.jpg");
            //Bitmap texture = new Bitmap(path + "african_head_SSS.jpg");
            Dictionary<Vector2I, double> zBuffer = new Dictionary<Vector2I, double>();

            Matrix perspectiveMatrix = Matrix.IdentityMatrix(4);
            perspectiveMatrix[3, 2] = -1 / (float)depth;

            for (int i = 1; i < model.faces.Count + 1; i++)
            {
                Model.Face face = model.faces[i];
                Vector3 normal = Vector3.CountNormal(model.vertexes[face.vertexes[0]],
                                                     model.vertexes[face.vertexes[1]],
                                                     model.vertexes[face.vertexes[face.vertexes.Count - 1]]);

                normal.Normalize();

                double intence = normal.z;

                if (intence <= 0 || double.IsNaN(intence))
                {
                    continue;
                }

                Drawing.DrawFace drFace = new Drawing.DrawFace();
                
                foreach (int vertex in face.vertexes)
                {
                    Matrix matrix = new Matrix(4, 1);
                    matrix[0, 0] = ((model.vertexes[vertex].x) * (width/2 - gap));
                    matrix[1, 0] = ((model.vertexes[vertex].y) * (height/2 - gap));
                    matrix[2, 0] = model.vertexes[vertex].z;
                    matrix[3, 0] = 1;

                    Matrix transaction = perspectiveMatrix * matrix;

                    Vector3 vect = new Vector3(
                        transaction[0,0]/transaction[3,0],
                        transaction[1,0]/transaction[3,0],
                        transaction[2,0]/transaction[3,0]);

                    drFace.points.Add(vect);
                    drFace.normals.Add(model.normalsVertexes[vertex]);
                    drFace.textures.Add(model.textureVertexes[vertex]);
                }

                Vector2 texturePoint1 = drFace.textures[0];
                texturePoint1.x += textureShift;
                texturePoint1.y += textureShift;
                Vector2 texturePoint2 = drFace.textures[1];
                texturePoint2.x += textureShift;
                texturePoint2.y += textureShift;
                Vector2 texturePoint3 = drFace.textures[2];
                texturePoint3.x += textureShift;
                texturePoint3.y += textureShift;
                
                Vector3 modelPoint1 = drFace.points[0];
                modelPoint1.x += textureCountModelShift * width;
                modelPoint1.y += textureCountModelShift * height;
                modelPoint1.z += textureCountModelShift;
                Vector3 modelPoint2 = drFace.points[1];
                modelPoint2.x += textureCountModelShift * width;
                modelPoint2.y += textureCountModelShift * height;
                modelPoint2.z += textureCountModelShift;
                Vector3 modelPoint3 = drFace.points[2];
                modelPoint3.x += textureCountModelShift * width;
                modelPoint3.y += textureCountModelShift * height;
                modelPoint3.z += textureCountModelShift;
                
                double tempVal1 = (texturePoint1.x * modelPoint3.x - texturePoint3.x * modelPoint1.x) *
                                  (modelPoint2.y * modelPoint3.x - modelPoint3.y * modelPoint2.x) -
                                  (modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x) *
                                  (texturePoint2.x * modelPoint3.x - texturePoint3.x * modelPoint2.x);

                double tempVal2 = (modelPoint1.z * modelPoint3.x - modelPoint3.z * modelPoint1.x) *
                                  (modelPoint2.y * modelPoint3.x - modelPoint3.y * modelPoint2.x) - 
                                  (modelPoint2.z * modelPoint3.x - modelPoint3.z * modelPoint2.x) *
                                  (modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x);

                double a3 = tempVal1 / tempVal2;

                tempVal1 = texturePoint1.x * modelPoint3.x - modelPoint1.x * texturePoint3.x -
                           a3 * (modelPoint1.z * modelPoint3.x - modelPoint3.z * modelPoint1.x);

                tempVal2 = modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x;

                double a2 = tempVal1 / tempVal2;

                double a1 = (texturePoint1.x - modelPoint1.y * a2 - modelPoint1.z * a3) / modelPoint1.x;

                tempVal1 = (texturePoint1.y * modelPoint3.x - texturePoint3.y * modelPoint1.x) *
                           (modelPoint2.y * modelPoint3.x - modelPoint3.y * modelPoint2.x) -
                           (modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x) *
                           (texturePoint2.y * modelPoint3.x - texturePoint3.y * modelPoint2.x);

                tempVal2 = (modelPoint1.z * modelPoint3.x - modelPoint3.z * modelPoint1.x) *
                           (modelPoint2.y * modelPoint3.x - modelPoint3.y * modelPoint2.x) -
                           (modelPoint2.z * modelPoint3.x - modelPoint3.z * modelPoint2.x) *
                           (modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x);
                
                double b3 = tempVal1 / tempVal2;
                
                tempVal1 = texturePoint1.y * modelPoint3.x - modelPoint1.x * texturePoint3.y -
                           b3 * (modelPoint1.z * modelPoint3.x - modelPoint3.z * modelPoint1.x);

                tempVal2 = modelPoint1.y * modelPoint3.x - modelPoint3.y * modelPoint1.x;

                double b2 = tempVal1 / tempVal2;
                
                double b1 = (texturePoint1.y - modelPoint1.y * b2 - modelPoint1.z * b3) / modelPoint1.x;

                Drawing.ColorDelegate colorDel = delegate (Vector3 pos)
                {
                    double v = a1 * (pos.x + textureCountModelShift * width) +
                               a2 * (pos.y + textureCountModelShift * height) +
                               a3 * (pos.z + textureCountModelShift) -
                               textureShift;
                    double u = b1 * (pos.x + textureCountModelShift * width) +
                               b2 * (pos.y + textureCountModelShift * height) +
                               b3 * (pos.z + textureCountModelShift) -
                               textureShift;

                    if (u > 1 || u < 0 || v > 1 || v < 0) return Color.Black;

                    return texture.GetPixel((int)(u * texture.Width), (int)(v * texture.Height));
                    
                    //return Color.FromArgb(255, (int)(intence * 255 / 2), (int)(intence * 255), (int)(intence * 255));
                };

                Drawing.Polygon(drFace, bmp, colorDel, zBuffer);
            }

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light.bmp", ImageFormat.Bmp);
        }

    }
}
