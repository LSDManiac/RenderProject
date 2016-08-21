using System;
using System.Collections.Generic;
using System.Drawing;
using RenderProject.MyMath;

namespace RenderProject
{
    public static class Drawing
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
        }

        public delegate Color ColorDelegate(Vector3 pos);
        public delegate Color PixelColorDelegate(Vector2I pos);

        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static void Swap(ref double a, ref double b)
        {
            double temp = a;
            a = b;
            b = temp;
        }

        private static void SetPixel(int x, int y, double z, Color color, Bitmap image, Dictionary<Vector2I, double> zBuffer)
        {
            if (!(x >= image.Width / 2 || x <= -image.Width / 2 || y >= image.Height / 2 || y <= -image.Height / 2))
            {
                Vector2I curPoint = new Vector2I(x, y);
                if (!zBuffer.ContainsKey(curPoint) || zBuffer[curPoint] <= z)
                {
                    image.SetPixel(x + image.Width/2, y + image.Height/2, color);
                    if (zBuffer.ContainsKey(curPoint)) zBuffer[curPoint] = z;
                    else zBuffer.Add(curPoint, z);
                }
            }
        }

        public static void Line(Vector3 p0, Vector3 p1,
                                Bitmap image, ColorDelegate colorD,
                                Dictionary<Vector2I, double> zBuffer)
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
                    SetPixel(y, x, curZ, colorD(new Vector3(y, x, curZ)), image, zBuffer);
                }
                else
                {
                    SetPixel(x, y, curZ, colorD(new Vector3(x, y, curZ)), image, zBuffer);
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
        
        public static void Polygon(DrawFace face,
                                   Bitmap image, ColorDelegate colorD,
                                   Dictionary<Vector2I, double> zBuffer)
        {
            // Splits polygon into triangles
            for (int i = 2; i < face.points.Count; i++)
            {
                Triangle(face.points[0], face.points[i - 1], face.points[i], image, colorD, zBuffer);
            }
        }

        public static void Triangle(Vector3 p0, Vector3 p1, Vector3 p2,
                                    Bitmap image, ColorDelegate colorD,
                                    Dictionary<Vector2I, double> zBuffer)
        {
            Vector2I p0I = new Vector2I((int)Math.Round(p0.x), (int)Math.Round(p0.y));
            Vector2I p1I = new Vector2I((int)Math.Round(p1.x), (int)Math.Round(p1.y));
            Vector2I p2I = new Vector2I((int)Math.Round(p2.x), (int)Math.Round(p2.y));
            int xTop = 0, yTop = 0, xBot = 0, yBot = 0, xMid = 0, yMid = 0;
            double zTop = 0, zMid = 0, zBot = 0;
            if (p0I.y >= Math.Max(p1I.y, p2I.y))
            {
                xTop = p0I.x;
                yTop = p0I.y;
                zTop = p0.z;
                
                bool minBotRatio = p1I.y > p2I.y;
                xMid = minBotRatio ? p1I.x : p2I.x;
                yMid = minBotRatio ? p1I.y : p2I.y;
                zMid = minBotRatio ? p1.z : p2.z;
                xBot = !minBotRatio ? p1I.x : p2I.x;
                yBot = !minBotRatio ? p1I.y : p2I.y;
                zBot = !minBotRatio ? p1.z : p2.z;
            }
            else if (p1I.y >= Math.Max(p0I.y, p2I.y))
            {
                xTop = p1I.x;
                yTop = p1I.y;
                zTop = p1.z;

                bool minBotRatio = p0I.y > p2I.y;
                xMid = minBotRatio ? p0I.x : p2I.x;
                yMid = minBotRatio ? p0I.y : p2I.y;
                zMid = minBotRatio ? p0.z : p2.z;
                xBot = !minBotRatio ? p0I.x : p2I.x;
                yBot = !minBotRatio ? p0I.y : p2I.y;
                zBot = !minBotRatio ? p0.z : p2.z;
            }
            else if (p2I.y >= Math.Max(p0I.y, p1I.y))
            {
                xTop = p2I.x;
                yTop = p2I.y;
                zTop = p2.z;
                
                bool minBotRatio = p0I.y > p1I.y;
                xMid = minBotRatio ? p0I.x : p1I.x;
                yMid = minBotRatio ? p0I.y : p1I.y;
                zMid = minBotRatio ? p0.z : p1.z;
                xBot = !minBotRatio ? p0I.x : p1I.x;
                yBot = !minBotRatio ? p0I.y : p1I.y;
                zBot = !minBotRatio ? p0.z : p1.z;
            }

            int dxs = Math.Abs(xBot - xTop);
            int dys = Math.Abs(yBot - yTop);
            int dxa = Math.Abs(xBot - xMid);
            int dya = Math.Abs(yBot - yMid);

            int shiftS = 0;
            int stepS = 2 * dxs;

            int shiftA = 0;
            int stepA = 2 * dxa;

            int xs = xBot; // Strait x
            int xa = xBot; // Angled x

            int xMoveS = xTop > xBot ? 1 : -1;
            int xMoveA = xMid > xBot ? 1 : -1;

            double zs = zBot;
            double zsStep = (zTop - zBot) / Math.Abs(yBot - yTop);

            double za = zBot;
            double zaStep = (zMid - zBot) / Math.Abs(yBot - yMid);
            
            for (int y = yBot; y < yMid; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
                Line(s, a, image, colorD, zBuffer);

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

            dxa = Math.Abs(xTop - xMid);
            dya = Math.Abs(yTop - yMid);
            xa = xMid;

            shiftA = 0;
            stepA = 2 * dxa;
            
            xMoveA = xTop > xMid ? 1 : -1;
            
            za = zMid;
            zaStep = (zTop - zMid) / Math.Abs(yTop - yMid);

            for (int y = yMid; y <= yTop; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
                Line(s, a, image, colorD, zBuffer);

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
            //Line(p0, p1, image, delegate { return Color.Red; }, new Dictionary<Vector2i, double>());
            //Line(p0, p2, image, delegate { return Color.Red; }, new Dictionary<Vector2i, double>());
            //Line(p1, p2, image, delegate { return Color.Red; }, new Dictionary<Vector2i, double>());
        }

    }
}
