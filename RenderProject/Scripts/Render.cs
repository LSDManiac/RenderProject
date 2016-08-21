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
            int width = 2000;
            int height = 2000;
            int gap = 100;
            int depth = 500;

            Bitmap bmp = new Bitmap(width + gap, height + gap);

            Material texture = new Material();
            texture.LoadTexture("D:\\Projects\\RenderProject\\RenderProject\\Models\\african_head_SSS.jpg");

            Model model = new Model();

            model.Load("D:\\Projects\\RenderProject\\RenderProject\\Models\\head.obj", 1);

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
                
                Drawing.DrawFace drFace = new Drawing.DrawFace();
                
                for (int j = 0; j < face.vertexes.Count; j++)
                {
                    Vector3 vect = model.vertexes[face.vertexes[j]];

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
            }
            
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light.bmp", ImageFormat.Bmp);
        }

    }
}
