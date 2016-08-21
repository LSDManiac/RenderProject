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

            Bitmap bmp = new Bitmap(width + gap, height + gap);

            Model model = new Model();

            //             D:\\Projects\\RenderProject\\RenderProject\\Models
            string path = "D:\\Projects\\RenderProject\\RenderProject\\Models\\";
            //string path = "E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\";

            model.Load(path + "head.obj");

            //Bitmap texture = new Bitmap(path + "head_diffuse.tga");
            Bitmap texture = new Bitmap(path + "african_head_SSS.jpg");
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

                Drawing.ColorDelegate colorDel = delegate (Vector3 pos)
                {
                    /*
                    float a1 = 0, a2 = 0, a3 = 0;
                    float b1 = 0, b2 = 0, b3 = 0;


                    double u = a1 * pos.x + a2 * pos.y + a3 * pos.z;
                    double v = b1 * pos.x + b2 * pos.y + b3 * pos.z;

                    texture.GetPixel((int)(u * texture.Width), (int)(v * texture.Height));
                    */
                    return Color.FromArgb(255, (int)(intence * 255 / 2), (int)(intence * 255), (int)(intence * 255));
                };
                
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
                
                Drawing.Polygon(drFace, bmp, colorDel, zBuffer);
            }

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light.bmp", ImageFormat.Bmp);
        }

    }
}
