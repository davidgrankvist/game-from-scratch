using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Framework
{
    internal interface IGraphics2D
    {
        public void Resize(int width, int height);

        public void Commit();

        public void Fill(Color color);

        public void DrawRectangle(Vector2 position, float width, float height, Color color);
    }
}
