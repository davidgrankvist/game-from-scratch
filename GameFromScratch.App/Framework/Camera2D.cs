using GameFromScratch.App.Framework.Maths;
using System.Numerics;

namespace GameFromScratch.App.Framework
{
    internal class Camera2D
    {
        private Vector2 viewportTopLeft;
        public Vector2 ViewportTopLeft { get => viewportTopLeft; }
        // TODO(feature): add scaling factor so that world units can differ from pixels

        public Camera2D()
        {
            viewportTopLeft = Vector2.Zero;
        }

        public void SetViewport(float x, float y)
        {
            viewportTopLeft = new Vector2(x, y);
        }

        public Vector2Int ToPixel(Vector2 worldPosition)
        {
            // translate so that the top left pixel matches the viewport
            var pixelPosition = worldPosition - viewportTopLeft;
            // round to nearest pixel
            var px = (int)(pixelPosition.X + 0.5);
            var py = (int)(pixelPosition.Y + 0.5);
            return new Vector2Int(px, py);
        }

        public Vector2 FromPixel(Vector2Int pixelPosition)
        {
            var worldPosition = pixelPosition.ToVector2() + viewportTopLeft;
            return worldPosition;
        }
    }
}
