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
<<<<<<< HEAD
            int width = 2000;
            int height = 2000;
            int gap = 100;
            int depth = 500;
=======
            int width = 1200;
            int height = 1200;
>>>>>>> origin/master

            Bitmap bmp = new Bitmap(width + gap, height + gap);

            Material texture = new Material();
            texture.LoadTexture("D:\\Projects\\RenderProject\\RenderProject\\Models\\african_head_SSS.jpg");

            Model model = new Model();

<<<<<<< HEAD
            model.Load("D:\\Projects\\RenderProject\\RenderProject\\Models\\head.obj", 1);
=======
            string path = "C:\\Projects\\RenderProject\\RenderProject\\Models\\";
            //string path = "E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\";
            
            model.Load(path + "head.obj", 1);
>>>>>>> origin/master

            Bitmap texture = new Bitmap(path + "head_diffuse.tga");
            Dictionary<Vector2i, double> zBuffer = new Dictionary<Vector2i, double>();

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
<<<<<<< HEAD
                
                Drawing.DrawFace drFace = new Drawing.DrawFace();
=======

                Drawing.ColorDelegate colorDel = delegate (Vector3 pos)
                {

                    float a1 = 0, a2 = 0, a3 = 0;
                    float b1 = 0, b2 = 0, b3 = 0;


                    double u = a1 * pos.x + a2 * pos.y + a3 * pos.z;
                    double v = b1 * pos.x + b2 * pos.y + b3 * pos.z;
                    
                    texture.GetPixel((int)(u * texture.Width),(int)(v * texture.Height));
                    return Color.FromArgb(255, (int) (intence*255 / 2), (int) (intence*255), (int) (intence*255)); 
                };

                List<Vector3> points = new List<Vector3>();
                List<Vector3> textures = new List<Vector3>();
>>>>>>> origin/master
                

                for (int j = 0; j < face.vertexes.Count; j++)
                {
                    Vector3 vect = model.vertexes[face.vertexes[j]];

<<<<<<< HEAD
                    vect.x = ((vect.x / 2 + 0.5f) * width + gap/2);
                    vect.y = ((vect.y / 2 + 0.5f) * height + gap/2);

                    // Depth experiment here, trying to fix z buffer problem
                    vect.z = (vect.z * depth);

                    drFace.points.Add(vect);
                    drFace.normals.Add(model.normalsVertexes[face.vertexes[j]]);
                    drFace.textures.Add(model.textureVertexes[face.vertexes[j]]);
                }
                
                Color color = Color.FromArgb(255, (int) (intence*255), (int) (intence*255), (int) (intence*255));

                Drawing.Polygon(drFace, bmp, color, texture, zBuffer);
=======
                    vect.x = (int)((vect.x / 2 + 0.5f) * width);
                    vect.y = (int)((vect.y / 2 + 0.5f) * height);
                    
                    points.Add(vect);

                    Vector3 text = model.textureVertexes[face.textures[j]];
                    text.x = (int)(text.x * texture.Width);
                    text.y = (int)(text.y * texture.Height);
                    textures.Add(text);
                }

                Drawing.Polygon(points, bmp, colorDel, zBuffer);
>>>>>>> origin/master
            }
            
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light.bmp", ImageFormat.Bmp);
        }

    }
}
