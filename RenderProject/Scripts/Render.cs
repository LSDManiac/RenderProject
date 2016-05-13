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
            int width = 1200;
            int height = 1200;

            Bitmap bmp = new Bitmap(width, height);

            Model model = new Model();

            string path = "C:\\Projects\\RenderProject\\RenderProject\\Models\\";
            //string path = "E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\";
            
            model.Load(path + "head.obj", 1);

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
                

                for (int j = 0; j < face.vertexes.Count; j++)
                {
                    Vector3 vect = model.vertexes[face.vertexes[j]];

                    vect.x = (int)((vect.x / 2 + 0.5f) * width);
                    vect.y = (int)((vect.y / 2 + 0.5f) * height);
                    
                    points.Add(vect);

                    Vector3 text = model.textureVertexes[face.textures[j]];
                    text.x = (int)(text.x * texture.Width);
                    text.y = (int)(text.y * texture.Height);
                    textures.Add(text);
                }

                Drawing.Polygon(points, bmp, colorDel, zBuffer);
            }
            
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light3.bmp", ImageFormat.Bmp);
        }

    }
}
