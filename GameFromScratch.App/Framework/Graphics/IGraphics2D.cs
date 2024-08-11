using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Framework.Graphics
{
    internal interface IGraphics2D
    {
        /// <summary>
        /// Resize graphics buffers for the given screen resolution.
        /// </summary>
        public void Resize(int width, int height);

        /// <summary>
        /// Paint pending graphics.
        /// </summary>
        public void Commit();

        public void Fill(Color color);

        public void DrawRectangle(Vector2 position, float width, float height, Color color);

        public void DrawCircle(Vector2 position, float radius, Color color);

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color);

        public void DrawRectangleRotated(Vector2 position, float width, float height, Color color, float angle, Vector2 origin);

        public void DrawText(string text, int fontSize, Color color, Vector2 position);

        /// <summary>
        /// Treat coordinates as pixels rather then world coordinates. Convenience accessor to set
        /// the pixel mode of the underlying camera.
        /// </summary>
        public bool PixelMode { get; set; }

        /// <summary>
        /// Draws a texture in its original dimensions.
        /// </summary>
        public void DrawTexture(string textureName, Vector2 position);

        /// <summary>
        /// Draws a texture, scaling it to the specified dimensions.
        /// </summary>
        public void DrawTexture(string textureName, Vector2 position, float width, float height);
    }
}
