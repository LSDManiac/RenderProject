using System.Drawing;
using RenderProject.MyMath;

namespace RenderProject.Graphics.ColorPerformers
{
    public class ColorPerformer
    {
        public ColorPerformer(ColorPerformer inner = null)
        {
            _innerPerformer = inner;
        }

        private readonly ColorPerformer _innerPerformer;

        public virtual Color GetColor(Vector3 position)
        {
            if(_innerPerformer == null) return Color.Black;
            return _innerPerformer.GetColor(position);
        }
    }
}
