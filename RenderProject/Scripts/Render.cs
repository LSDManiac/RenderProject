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
            int depth = 500;

            Bitmap bmp = new Bitmap(width, height);

            Model model = new Model();

            //model.Load("E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\head.obj", 1);
            model.Load("C:\\Projects\\RenderProject\\RenderProject\\Models\\head.obj", 1);

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

                List<Vector3> points = new List<Vector3>();
                
                for (int j = 0; j < face.vertexes.Count; j++)
                {
                    Vector3 vect = model.vertexes[face.vertexes[j]];

                    vect.x = (int)((vect.x / 2 + 0.5f) * width);
                    vect.y = (int)((vect.y / 2 + 0.5f) * height);

                    // Depth experiment here, trying to fix z buffer problem
                    vect.z = (int)(vect.z * depth);

                    points.Add(vect);
                }

                //for (int j = 1; j < face.vertexes.Count + 1; j++)
                //{
                //    float x0f = model.vertexes[face.vertexes[(j-1) % face.vertexes.Count]].x;
                //    float y0f = model.vertexes[face.vertexes[(j-1) % face.vertexes.Count]].y;

                //    int x0 = (int)((x0f / 2 + 0.5f) * width);
                //    int y0 = (int)((y0f / 2 + 0.5f) * height);

                //    float x1f = model.vertexes[face.vertexes[j % face.vertexes.Count]].x;
                //    float y1f = model.vertexes[face.vertexes[j % face.vertexes.Count]].y;

                //    int x1 = (int)((x1f / 2 + 0.5f) * width);
                //    int y1 = (int)((y1f / 2 + 0.5f) * height);

                //    Drawing.Line(x0, y0, x1, y1, bmp, Color.White);
                //}

                Color color = Color.FromArgb(255, (int) (intence*255), (int) (intence*255), (int) (intence*255));

                Drawing.Polygon(points, bmp, color, zBuffer);
            }
            
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light3.bmp", ImageFormat.Bmp);
        }

    }
}
