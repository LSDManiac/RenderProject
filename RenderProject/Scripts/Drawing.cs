using System;
using System.Collections.Generic;
using System.Drawing;

namespace RenderProject
{
    public static class Drawing
    {

        /// <summary>
        /// Swaps two int vars
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Draws line on given Bitmap
        /// </summary>
        /// <param name="x0">Point 1 x</param>
        /// <param name="y0">Point 1 y</param>
        /// <param name="x1">Point 2 x</param>
        /// <param name="y1">Point 2 y</param>
        /// <param name="image"></param>
        /// <param name="color">Color of the line</param>
        public static void Line(int x0, int y0, int x1, int y1, Bitmap image, Color color)
        {
            // Takes distances between point
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            // Swap stats firstly needed to make line go from zero to I quadrant
            // Secondly swap needed to make line go on tan < 1 (it's eaier to drow line this way)
            bool swapped = false;
            if (dx < dy)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
                swapped = true;
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }


            // Recounting distances and creating shifts and steps variable
            dx = Math.Abs(x1 - x0);
            dy = Math.Abs(y1 - y0);
            int shift = 0;
            int step = 2*dy;
            int y = y0;

            // Main idea of this method not to count trigonometry
            // But to count incrementing of y param in int
            for (int x = x0; x <= x1; x++)
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
                    y += (y0 > y1 ? -1 : 1);
                }
            }
        }

        /// <summary>
        /// Draws polygon
        /// </summary>
        /// <param name="points">Points of polygon</param>
        /// <param name="image"></param>
        /// <param name="color">Color of polygon</param>
        public static void Polygon(List<KeyValuePair<int, int>> points, Bitmap image, Color color)
        {
            // Splits polygon into triangles
            for (int i = 2; i < points.Count; i++)
            {
                Triangle(points[0].Key, points[0].Value,
                    points[i - 1].Key, points[i - 1].Value,
                    points[i].Key, points[i].Value,
                    image, color);
            }
        }

        /// <summary>
        /// Draws triangle on given Bitmap
        /// </summary>
        /// <param name="x0">Point 1 x</param>
        /// <param name="y0">Point 1 y</param>
        /// <param name="x1">Point 2 x</param>
        /// <param name="y1">Point 2 y</param>
        /// <param name="x2">Point 3 x</param>
        /// <param name="y2">Point 3 y</param>
        /// <param name="image"></param>
        /// <param name="color">Color of triangle</param>
        public static void Triangle(int x0, int y0, int x1, int y1, int x2, int y2, Bitmap image, Color color)
        {
            int xTop = 0, yTop = 0, xBot = 0, yBot = 0, xMid = 0, yMid = 0;
            if (y0 >= Math.Max(y1, y2))
            {
                xTop = x0;
                yTop = y0;
                xMid = y1 > y2 ? x1 : x2;
                yMid = y1 > y2 ? y1 : y2;
                xBot = y1 <= y2 ? x1 : x2;
                yBot = y1 <= y2 ? y1 : y2;
            }
            else if (y1 >= Math.Max(y0, y2))
            {
                xTop = x1;
                yTop = y1;
                xMid = y0 > y2 ? x0 : x2;
                yMid = y0 > y2 ? y0 : y2;
                xBot = y0 <= y2 ? x0 : x2;
                yBot = y0 <= y2 ? y0 : y2;
            }
            else if (y2 >= Math.Max(y0, y1))
            {
                xTop = x2;
                yTop = y2;
                xMid = y0 > y1 ? x0 : x1;
                yMid = y0 > y1 ? y0 : y1;
                xBot = y0 <= y1 ? x0 : x1;
                yBot = y0 <= y1 ? y0 : y1;
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
                Line(xs, y, xa, y, image, color);

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
                Line(xs, y, xa, y, image, color);

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
            //Line(x0, y0, x1, y1, image, Color.Red);
            //Line(x0, y0, x2, y2, image, Color.Red);
            //Line(x2, y2, x1, y1, image, Color.Red);
        }

    }
}
