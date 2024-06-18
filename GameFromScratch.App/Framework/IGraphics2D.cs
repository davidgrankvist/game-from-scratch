using System.Drawing;

namespace GameFromScratch.App.Framework
{
    internal interface IGraphics2D
    {
        public void Fill(Color color);

        public void Resize(int width, int height);

        public void Commit();
    }
}
