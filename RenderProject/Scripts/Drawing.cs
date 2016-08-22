using System;
using System.Collections.Generic;
using System.Drawing;
using RenderProject.Graphics.ColorPerformers;
using RenderProject.MyMath;

namespace RenderProject
{
    public class Drawing
    {



        public class DrawFace
        {
            public DrawFace()
            {
                points = new List<Vector3>();
                textures = new List<Vector3>();
                normals = new List<Vector3>();
            }

            public List<Vector3> points;
            public List<Vector3> textures;
            public List<Vector3> normals;

            public Bitmap texture;
        }

        public delegate Color ColorDelegate(Vector3 pos);
        public delegate Color PixelColorDelegate(Vector2I pos);

        private Bitmap _image;
        private Dictionary<Vector2I, double> _zBuffer;

        public void Init(Bitmap image, Dictionary<Vector2I, double> zBuffer)
        {
            _image = image;
            _zBuffer = zBuffer;
        }



        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private void Swap(ref double a, ref double b)
        {
            double temp = a;
            a = b;
            b = temp;
        }

        private void SetPixel(int x, int y, double z, Color color)
        {
            if (!(x >= _image.Width / 2 || x <= -_image.Width / 2 || y >= _image.Height / 2 || y <= -_image.Height / 2))
            {
                Vector2I curPoint = new Vector2I(x, y);
                if (!_zBuffer.ContainsKey(curPoint) || _zBuffer[curPoint] <= z)
                {
                    _image.SetPixel(x + _image.Width/2, y + _image.Height/2, color);
                    if (_zBuffer.ContainsKey(curPoint)) _zBuffer[curPoint] = z;
                    else _zBuffer.Add(curPoint, z);
                }
            }
        }

        public void Line(Vector3 p0, Vector3 p1,
                                ColorPerformer colorP)
        {
            Vector2I p0I = p0;
            Vector2I p1I = p1;
            double p0Z = p0.z;
            double p1Z = p1.z;

            // Takes distances between point
            int dx = Math.Abs(p1I.x - p0I.x);
            int dy = Math.Abs(p1I.y - p0I.y);

            // Swap stats firstly needed to make line go from zero to I quadrant
            // Secondly swap needed to make line go on tan < 1 (it's eaier to drow line this way)
            bool swapped = false;
            if (dx < dy)
            {
                Swap(ref p0I.x, ref p0I.y);
                Swap(ref p1I.x, ref p1I.y);
                swapped = true;
            }

            if (p0I.x > p1I.x)
            {
                Swap(ref p0I.x, ref p1I.x);
                Swap(ref p0I.y, ref p1I.y);
                Swap(ref p0Z, ref p1Z);
            }
            
            // Recounting distances and creating shifts and steps variable
            dx = Math.Abs(p1I.x - p0I.x);
            dy = Math.Abs(p1I.y - p0I.y);
            int shift = 0;
            int step = 2*dy;
            int y = p0I.y;

            double curZ = p0Z;
            double zStep = (p1Z - p0Z) / Math.Abs(p1I.x - p0I.x);

            // Main idea of this method not to count trigonometry
            // But to count incrementing of y param in int
            // Whenever here is z buffer also we count z step in double
            for (int x = p0I.x; x <= p1I.x; x++)
            {
                if (swapped)
                {
                    SetPixel(y, x, curZ, colorP.GetColor(new Vector3(y, x, curZ)));
                }
                else
                {
                    SetPixel(x, y, curZ, colorP.GetColor(new Vector3(x, y, curZ)));
                }

                shift += step;

                if (shift >= dx)
                {
                    shift -= 2*dx;
                    y += (p0I.y > p1I.y ? -1 : 1);
                }

                curZ += zStep;
            }
        }
        
        public void Polygon(DrawFace face,
                                   ColorPerformer colorP)
        {
            // Splits polygon into triangles
            for (int i = 2; i < face.points.Count; i++)
            {
                Triangle(face.points[0], face.points[i - 1], face.points[i], colorP);
            }
        }

        public void Triangle(Vector3 p0, Vector3 p1, Vector3 p2,
                                    ColorPerformer colorP)
        {
            Vector2I p0I = new Vector2I((int)Math.Round(p0.x), (int)Math.Round(p0.y));
            Vector2I p1I = new Vector2I((int)Math.Round(p1.x), (int)Math.Round(p1.y));
            Vector2I p2I = new Vector2I((int)Math.Round(p2.x), (int)Math.Round(p2.y));

            Vector2I mid, bot;
            Vector2I top = mid = bot = new Vector2I(0,0);
            double zTop = 0, zMid = 0, zBot = 0;
            if (p0I.y >= Math.Max(p1I.y, p2I.y))
            {
                top = p0I;
                zTop = p0.z;
                
                bool minBotRatio = p1I.y > p2I.y;
                mid = minBotRatio ? p1I : p2I;
                zMid = minBotRatio ? p1.z : p2.z;
                bot = !minBotRatio ? p1I : p2I;
                zBot = !minBotRatio ? p1.z : p2.z;
            }
            else if (p1I.y >= Math.Max(p0I.y, p2I.y))
            {
                top = p1I;
                zTop = p1.z;

                bool minBotRatio = p0I.y > p2I.y;
                mid = minBotRatio ? p0I : p2I;
                zMid = minBotRatio ? p0.z : p2.z;
                bot = !minBotRatio ? p0I : p2I;
                zBot = !minBotRatio ? p0.z : p2.z;
            }
            else if (p2I.y >= Math.Max(p0I.y, p1I.y))
            {
                top = p2I;
                zTop = p2.z;
                
                bool minBotRatio = p0I.y > p1I.y;
                mid = minBotRatio ? p0I : p1I;
                zMid = minBotRatio ? p0.z : p1.z;
                bot = !minBotRatio ? p0I : p1I;
                zBot = !minBotRatio ? p0.z : p1.z;
            }

            int dxs = Math.Abs(bot.x - top.x);
            int dys = Math.Abs(bot.y - top.y);
            int dxa = Math.Abs(bot.x - mid.x);
            int dya = Math.Abs(bot.y - mid.y);

            int shiftS = 0;
            int stepS = 2 * dxs;

            int shiftA = 0;
            int stepA = 2 * dxa;

            int xs = bot.x; // Strait x
            int xa = bot.x; // Angled x

            int xMoveS = top.x > bot.x ? 1 : -1;
            int xMoveA = mid.x > bot.x ? 1 : -1;

            double zs = zBot;
            double zsStep = (zTop - zBot) / Math.Abs(bot.y - top.y);

            double za = zBot;
            double zaStep = (zMid - zBot) / Math.Abs(bot.y - mid.y);
            
            for (int y = bot.y; y < mid.y; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
                Line(s, a, colorP);

                zs += zsStep;
                za += zaStep;

                shiftS += stepS;
                shiftA += stepA;

                while (shiftS >= dys && dys != 0)
                {
                    shiftS -= 2 * dys;
                    xs += xMoveS;
                }

                while (shiftA >= dya && dya != 0)
                {
                    shiftA -= 2 * dya;
                    xa += xMoveA;
                }
            }

            dxa = Math.Abs(top.x - mid.x);
            dya = Math.Abs(top.y - mid.y);
            xa = mid.x;

            shiftA = 0;
            stepA = 2 * dxa;
            
            xMoveA = top.x > mid.x ? 1 : -1;
            
            za = zMid;
            zaStep = (zTop - zMid) / Math.Abs(top.y - mid.y);

            for (int y = mid.y; y <= top.y; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
                Line(s, a, colorP);

                zs += zsStep;
                za += zaStep;

                shiftS += stepS;
                shiftA += stepA;

                while (shiftS >= dys && dys != 0)
                {
                    shiftS -= 2 * dys;
                    xs += xMoveS;
                }

                while (shiftA >= dya && dya != 0)
                {
                    shiftA -= 2 * dya;
                    xa += xMoveA;
                }
            }
        }
    }
}
