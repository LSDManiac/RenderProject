using System.Drawing;

namespace RenderProject.Graphics
{
    public class Material
    {
        public Bitmap texture;

        public void LoadTexture(string path)
        {
            texture = new Bitmap(Image.FromFile(path));
        }


    }
}
