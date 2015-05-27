using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using RenderProject.Graphics;

namespace RenderProject
{
    class Render
    {


        public static void Main()
        {
            int width = 800;
            int height = 800;

            Bitmap bmp = new Bitmap(width, height);

            Model model = new Model();

            model.Load("E:\\Projects\\RenderProject\\RenderProject\\RenderProject\\Models\\head.obj", 1);

            for (int i = 1; i < model.faces.Count + 1; i++)
            {
                Model.Face face = model.faces[i];
                Model.VertexNormal normal = Model.VertexNormal.CountNormal(model.vertexes[face.vertexes[0]],
                    model.vertexes[face.vertexes[1]],
                    model.vertexes[face.vertexes[face.vertexes.Count - 1]]);

                normal.Normalize();

                float intence = normal.z;

                if (intence <= 0 || float.IsNaN(intence))
                {
                    continue;
                }
                List<KeyValuePair<int, int>> points = new List<KeyValuePair<int, int>>();
                
                for (int j = 0; j < face.vertexes.Count; j++)
                {
                    
                    float x0f = model.vertexes[face.vertexes[j]].x;
                    float y0f = model.vertexes[face.vertexes[j]].y;

                    int x0 = (int)((x0f / 2 + 0.5f) * width);
                    int y0 = (int)((y0f / 2 + 0.5f) * height);
                    
                    points.Add(new KeyValuePair<int, int>(x0, y0));
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

                Drawing.Polygon(points, bmp, color);
            }
            
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("image_light3.bmp", ImageFormat.Bmp);
        }

    }
}
