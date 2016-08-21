using System;
using System.Collections.Generic;
using System.Drawing;
using RenderProject.MyMath;
using RenderProject.Graphics;

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
        public delegate Color PixelColorDelegate(Vector2i pos);

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

        public static void Line(Vector3 p0, Vector3 p1,
                                Bitmap image, ColorDelegate colorD,
                                Dictionary<Vector2i, double> zBuffer)
        {


            Vector2i p0i = p0;
            Vector2i p1i = p1;
            double p0z = p0.z;
            double p1z = p1.z;

            // Takes distances between point
            int dx = Math.Abs(p1i.x - p0i.x);
            int dy = Math.Abs(p1i.y - p0i.y);

            // Swap stats firstly needed to make line go from zero to I quadrant
            // Secondly swap needed to make line go on tan < 1 (it's eaier to drow line this way)
            bool swapped = false;
            if (dx < dy)
            {
                Swap(ref p0i.x, ref p0i.y);
                Swap(ref p1i.x, ref p1i.y);
                swapped = true;
            }

            if (p0i.x > p1i.x)
            {
                Swap(ref p0i.x, ref p1i.x);
                Swap(ref p0i.y, ref p1i.y);
                Swap(ref p0z, ref p1z);
            }
            
            // Recounting distances and creating shifts and steps variable
            dx = Math.Abs(p1i.x - p0i.x);
            dy = Math.Abs(p1i.y - p0i.y);
            int shift = 0;
            int step = 2*dy;
            int y = p0i.y;

            double curZ = p0z;
            double zStep = (p1z - p0z) / Math.Abs(p1i.x - p0i.x);

            // Main idea of this method not to count trigonometry
            // But to count incrementing of y param in int
            // Whenever here is z buffer also we count z step in double
            for (int x = p0i.x; x <= p1i.x; x++)
            {
                if (swapped)
                {
                    if (!(y >= image.Width || y < 0 || x >= image.Height || x < 0))
                    {
                        Vector2i curPoint = new Vector2i(y, x);
                        if (!zBuffer.ContainsKey(curPoint) || zBuffer[curPoint] <= curZ)
                        {
                            image.SetPixel(y, x, colorD(new Vector3(y, x, curZ)));
                            if (zBuffer.ContainsKey(curPoint)) zBuffer[curPoint] = curZ;
                            else zBuffer.Add(curPoint, curZ);
                        }
                    }
                }
                else
                {
                    if (!(x >= image.Width || x < 0 || y >= image.Height || y < 0))
                    {
                        Vector2i curPoint = new Vector2i(x, y);
                        if (!zBuffer.ContainsKey(curPoint) || zBuffer[curPoint] <= curZ)
                        {
                            image.SetPixel(x, y, colorD(new Vector3(x, y, curZ)));
                            if (zBuffer.ContainsKey(curPoint)) zBuffer[curPoint] = curZ;
                            else zBuffer.Add(curPoint, curZ);
                        }
                    }
                }

                shift += step;

                if (shift >= dx)
                {
                    shift -= 2*dx;
                    y += (p0i.y > p1i.y ? -1 : 1);
                }

                curZ += zStep;
            }
        }

<<<<<<< HEAD
        public static void HorisontalLine(Vector3 p0, Vector3 p1,
                                Bitmap image, Color color,
                                Dictionary<Vector2i, double> zBuffer)
        {
            if (p0.x > p1.x)
            {
                Vector3 temp = p0;
                p0 = p1;
                p1 = temp;
            }

            int y = (int)p0.y;
            int startX = (int)p0.x;
            int endX = (int)p1.x;

            double curZ = p0.z;
            double zStep = (p1.z - p0.z) / Math.Abs(endX - startX);

            for(int x = startX; x < endX; x++)
            {
                Vector2i curPoint = new Vector2i(x, y);
                if (!zBuffer.ContainsKey(curPoint) || zBuffer[curPoint] <= curZ)
                {
                    image.SetPixel(x, y, color);
                    if (zBuffer.ContainsKey(curPoint)) zBuffer[curPoint] = curZ;
                    else zBuffer.Add(curPoint, curZ);
                }
                curZ += zStep;
            }

        }

        public static void Polygon(DrawFace face,
                                   Bitmap image, Color color,
                                   Material material,
=======
        public static void Polygon(List<Vector3> points,
                                   Bitmap image, ColorDelegate colorD,
>>>>>>> origin/master
                                   Dictionary<Vector2i, double> zBuffer)
        {
            // Splits polygon into triangles
            for (int i = 2; i < face.points.Count; i++)
            {
<<<<<<< HEAD
                Triangle(face.points[0], face.points[i - 1], face.points[i], image, color, zBuffer);
=======
                Triangle(points[0], points[i - 1], points[i], image, colorD, zBuffer);
>>>>>>> origin/master
            }
        }

        public static void Triangle(Vector3 p0, Vector3 p1, Vector3 p2,
                                    Bitmap image, ColorDelegate colorD,
                                    Dictionary<Vector2i, double> zBuffer)
        {
            Vector2i p0i = new Vector2i((int)Math.Round(p0.x), (int)Math.Round(p0.y));
            Vector2i p1i = new Vector2i((int)Math.Round(p1.x), (int)Math.Round(p1.y));;
            Vector2i p2i = new Vector2i((int)Math.Round(p2.x), (int)Math.Round(p2.y));;
            int xTop = 0, yTop = 0, xBot = 0, yBot = 0, xMid = 0, yMid = 0;
            double zTop = 0, zMid = 0, zBot = 0;
            if (p0i.y >= Math.Max(p1i.y, p2i.y))
            {
                xTop = p0i.x;
                yTop = p0i.y;
                zTop = p0.z;
                
                bool minBotRatio = p1i.y > p2i.y;
                xMid = minBotRatio ? p1i.x : p2i.x;
                yMid = minBotRatio ? p1i.y : p2i.y;
                zMid = minBotRatio ? p1.z : p2.z;
                xBot = !minBotRatio ? p1i.x : p2i.x;
                yBot = !minBotRatio ? p1i.y : p2i.y;
                zBot = !minBotRatio ? p1.z : p2.z;
            }
            else if (p1i.y >= Math.Max(p0i.y, p2i.y))
            {
                xTop = p1i.x;
                yTop = p1i.y;
                zTop = p1.z;

                bool minBotRatio = p0i.y > p2i.y;
                xMid = minBotRatio ? p0i.x : p2i.x;
                yMid = minBotRatio ? p0i.y : p2i.y;
                zMid = minBotRatio ? p0.z : p2.z;
                xBot = !minBotRatio ? p0i.x : p2i.x;
                yBot = !minBotRatio ? p0i.y : p2i.y;
                zBot = !minBotRatio ? p0.z : p2.z;
            }
            else if (p2i.y >= Math.Max(p0i.y, p1i.y))
            {
                xTop = p2i.x;
                yTop = p2i.y;
                zTop = p2.z;
                
                bool minBotRatio = p0i.y > p1i.y;
                xMid = minBotRatio ? p0i.x : p1i.x;
                yMid = minBotRatio ? p0i.y : p1i.y;
                zMid = minBotRatio ? p0.z : p1.z;
                xBot = !minBotRatio ? p0i.x : p1i.x;
                yBot = !minBotRatio ? p0i.y : p1i.y;
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
            int xMoveA = xMid > xBot ? 1 : -1; ;

            double zs = zBot;
            double zsStep = (zTop - zBot) / Math.Abs(yBot - yTop);

            double za = zBot;
            double zaStep = (zMid - zBot) / Math.Abs(yBot - yMid);
            
            for (int y = yBot; y < yMid; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
<<<<<<< HEAD
                HorisontalLine(s, a, image, color, zBuffer);
=======
                Line(s, a, image, colorD, zBuffer);
>>>>>>> origin/master

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
            
            xMoveA = xTop > xMid ? 1 : -1; ;
            
            za = zMid;
            zaStep = (zTop - zMid) / Math.Abs(yTop - yMid);

            for (int y = yMid; y <= yTop; y++)
            {
                Vector3 s = new Vector3(xs, y, zs);
                Vector3 a = new Vector3(xa, y, za);
<<<<<<< HEAD
                HorisontalLine(s, a, image, color, zBuffer);
=======
                Line(s, a, image, colorD, zBuffer);
>>>>>>> origin/master

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
