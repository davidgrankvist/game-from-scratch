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

        public void DrawCircle(Vector2 position, float radius, Color color);

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color);

        public void DrawRectangleRotated(Vector2 position, float width, float height, Color color, float angle, Vector2 origin);
    }
}
