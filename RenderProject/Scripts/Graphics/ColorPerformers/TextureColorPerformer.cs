using System;
using System.Drawing;
using RenderProject.MyMath;

namespace RenderProject.Graphics.ColorPerformers
{
    public class TextureColorPerformer : ColorPerformer
    {
        public TextureColorPerformer(Bitmap texture, Vector3 p1, Vector3 p2, Vector3 p3,
            Vector2 t1, Vector2 t2, Vector2 t3, ColorPerformer inner = null) : base(inner)
        {
            _texture = texture;

            if ((int)Math.Round(p1.x) != (int)Math.Round(p2.x) &&
                (int)Math.Round(p1.y) != (int)Math.Round(p2.y) &&
                Math.Abs((int)Math.Round(p1.x) - (int)Math.Round(p2.x)) > 1 &&
                Math.Abs((int)Math.Round(p1.y) - (int)Math.Round(p2.y)) > 1)
            {
                _p1 = p1;
                _p2 = p2;
                _t1 = t1;
                _t2 = t2;
            }
            else if ((int)Math.Round(p1.x) != (int)Math.Round(p3.x) &&
                     (int)Math.Round(p1.y) != (int)Math.Round(p3.y) &&
                     Math.Abs((int)Math.Round(p1.x) - (int)Math.Round(p3.x)) > 1 &&
                     Math.Abs((int)Math.Round(p1.y) - (int)Math.Round(p3.y)) > 1)
            {
                _p1 = p1;
                _p2 = p3;
                _t1 = t1;
                _t2 = t3;
            }
            else
            {
                _p1 = p3;
                _p2 = p2;
                _t1 = t3;
                _t2 = t2;
            }
        }

        private readonly Bitmap _texture;

        private Vector3 _p1, _p2;
        private Vector2 _t1, _t2;

        public override Color GetColor(Vector3 position)
        {
            double xProgress = (_p1.x - position.x) / (_p1.x - _p2.x);
            double yProgress = (_p1.y - position.y) / (_p1.y - _p2.y);

            double u = _t1.x - (_t1.x - _t2.x) * xProgress;
            double v = _t1.y - (_t1.y - _t2.y) * yProgress;

            if (u < 0) u = 0;
            if (u > 1) u = (double)(_texture.Width - 1) / _texture.Width;
            if (v < 0) v = 0;
            if (v > 1) v = (double)(_texture.Height - 1) / _texture.Height;

            Color textureColor = _texture.GetPixel((int)(_texture.Width * u), (int)(_texture.Height * v));

            return textureColor;
        }
    }
}
