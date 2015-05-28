using System;
using System.Collections.Generic;
using System.Drawing;
using RenderProject.MyMath;

namespace RenderProject
{
    public static class Drawing
    {

        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static void Line(Vector3 p0, Vector3 p1, Bitmap image, Color color, Dictionary<Vector2i, int> zBuffer)
        {
            Vector2i p0i = p0;
            Vector2i p1i = p1;
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
            }


            // Recounting distances and creating shifts and steps variable
            dx = Math.Abs(p1i.x - p0i.x);
            dy = Math.Abs(p1i.y - p0i.y);
            int shift = 0;
            int step = 2*dy;
            int y = p0i.y;

            // Main idea of this method not to count trigonometry
            // But to count incrementing of y param in int
            for (int x = p0i.x; x <= p1i.x; x++)
            {
                if (swapped)
                {
                    if (!(y >= image.Width || y < 0 || x >= image.Height || x < 0))
                    {
                        image.SetPixel(y, x, color);
                    }
                }
                else
                {
                    if (!(x >= image.Width || x < 0 || y >= image.Height || y < 0))
                    {
                        image.SetPixel(x, y, color);
                    }
                }

                shift += step;

                if (shift >= dx)
                {
                    shift -= 2*dx;
                    y += (p0i.y > p1i.y ? -1 : 1);
                }
            }
        }

        public static void Polygon(List<Vector3i> points, Bitmap image, Color color, Dictionary<int, int> zBuffer)
        {
            // Splits polygon into triangles
            for (int i = 2; i < points.Count; i++)
            {
                Triangle(points[0], points[i - 1], points[i], image, color, zBuffer);
            }
        }

        public static void Triangle(Vector3 p0, Vector3 p1, Vector3 p2, Bitmap image, Color color, Dictionary<int, int> zBuffer)
        {
            Vector2i p0i = p0;
            Vector2i p1i = p1;
            Vector2i p2i = p2;
            int xTop = 0, yTop = 0, xBot = 0, yBot = 0, xMid = 0, yMid = 0;
            if (p0i.y >= Math.Max(p1i.y, p2i.y))
            {
                xTop = p0i.x;
                yTop = p0i.y;
                xMid = p1i.y > p2i.y ? p1i.y : p2i.y;
                yMid = p1i.y > p2i.y ? p1i.y : p2i.y;
                xBot = p1i.y <= p2i.y ? p1i.y : p2i.y;
                yBot = p1i.y <= p2i.y ? p1i.y : p2i.y;
            }
            else if (p1i.y >= Math.Max(p0i.y, p2i.y))
            {
                xTop = p1i.x;
                yTop = p1i.y;
                xMid = p0i.y > p2i.y ? p0i.x : p2i.x;
                yMid = p0i.y > p2i.y ? p0i.y : p2i.y;
                xBot = p0i.y <= p2i.y ? p0i.x : p2i.x;
                yBot = p0i.y <= p2i.y ? p0i.y : p2i.y;
            }
            else if (p2i.y >= Math.Max(p0i.y, p1i.y))
            {
                xTop = p2i.x;
                yTop = p2i.y;
                xMid = p0i.y > p1i.y ? p0i.x : p1i.x;
                yMid = p0i.y > p1i.y ? p0i.y : p1i.y;
                xBot = p0i.y <= p1i.y ? p0i.x : p1i.x;
                yBot = p0i.y <= p1i.y ? p0i.y : p1i.y;
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

            for (int y = yBot; y < yMid; y++)
            {
                Line(xs, y, xa, y, image, color, zBuffer);

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

            for (int y = yMid; y <= yTop; y++)
            {
                Line(xs, y, xa, y, image, color, zBuffer);

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
            //Line(p0.x, p0.y, p1.x, p1.y, image, Color.Red);
            //Line(p0.x, p0.y, p2.x, p2.y, image, Color.Red);
            //Line(p2.x, p2.y, p1.x, p1.y, image, Color.Red);
        }

    }
}
