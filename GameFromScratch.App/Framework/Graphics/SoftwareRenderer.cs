using GameFromScratch.App.Framework.Maths;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Framework.Graphics
{
    internal abstract class SoftwareRenderer2D : IGraphics2D
    {
        protected int[] bitmap;
        protected int Width;
        protected int Height;
        private Vector2 viewportTopLeft;
        // TODO(feature): add scaling factor so that world units can differ from pixels

        public SoftwareRenderer2D()
        {
            viewportTopLeft = Vector2.Zero;
        }

        public virtual void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            bitmap = new int[width * height];
        }

        // implement in platform code
        public abstract void Commit();

        public void SetViewport(float x, float y)
        {
            viewportTopLeft = new Vector2(x, y);
        }

        public void Fill(Color color)
        {
            Array.Fill(bitmap, color.ToArgb());
        }

        private void SetPixel(int x, int y, Color color)
        {
            bitmap[ToIndex(x, y)] = color.ToArgb();
        }

        private int ToIndex(int x, int y)
        {
            return y * Width + x;
        }

        private Vector2Int ToPixel(Vector2 worldPosition)
        {
            // translate so that the top left pixel matches the viewport
            var pixelPosition = worldPosition - viewportTopLeft;
            // round to nearest pixel
            var px = (int)(pixelPosition.X + 0.5);
            var py = (int)(pixelPosition.Y + 0.5);
            return new Vector2Int(px, py);
        }

        private Vector2 FromPixel(Vector2Int pixelPosition)
        {
            var worldPosition = pixelPosition.ToVector2() + viewportTopLeft;
            return worldPosition;
        }

        public void DrawRectangle(Vector2 position, float width, float height, Color color)
        {
            var topLeftPixel = ToPixel(position);
            var bottomRightPixel = ToPixel(position + new Vector2(width, height));

            // visible part of rectangle
            var pxStart = Math.Max(topLeftPixel.X, 0);
            var pxEnd = Math.Min(bottomRightPixel.X, Width);
            var pyStart = Math.Max(topLeftPixel.Y, 0);
            var pyEnd = Math.Min(bottomRightPixel.Y, Height);

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    SetPixel(ix, iy, color);
                }
            }
        }

        public void DrawCircle(Vector2 position, float radius, Color color)
        {
            var boundingBoxTopLeft = ToPixel(position - new Vector2(radius, radius));
            var boundingBoxBottomRight = ToPixel(position + new Vector2(radius, radius));

            // visible part of bounding box
            var pxStart = Math.Max(boundingBoxTopLeft.X, 0);
            var pxEnd = Math.Min(boundingBoxBottomRight.X, Width);
            var pyStart = Math.Max(boundingBoxTopLeft.Y, 0);
            var pyEnd = Math.Min(boundingBoxBottomRight.Y, Height);

            // color pixels where the distance from the center is at most the radius
            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    var delta = FromPixel(new Vector2Int(ix, iy)) - position;

                    var squaredDistance = delta.X * delta.X + delta.Y * delta.Y;
                    if (squaredDistance <= radius * radius)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            var pixelA = ToPixel(a);
            var pixelB = ToPixel(b);
            var pixelC = ToPixel(c);

            // bounding box
            var pxMin = MathExtensions.Min(pixelA.X, pixelB.X, pixelC.X);
            var pxMax = MathExtensions.Max(pixelA.X, pixelB.X, pixelC.X);
            var pyMin = MathExtensions.Min(pixelA.Y, pixelB.Y, pixelC.Y);
            var pyMax = MathExtensions.Max(pixelA.Y, pixelB.Y, pixelC.Y);

            // visible part of bounding box
            var pxStart = Math.Max(pxMin, 0);
            var pxEnd = Math.Min(pxMax, Width);
            var pyStart = Math.Max(pyMin, 0);
            var pyEnd = Math.Min(pyMax, Height);

            // triangle edges
            var ab = b - a;
            var bc = c - b;
            var ca = a - c;

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    var p = FromPixel(new Vector2Int(ix, iy));
                    /*
                     * Walking around the triangle, p should be either to your left or right
                     * the entire time. If it switches back and forth, it can't be in the triangle.
                     *
                     * The cross product determines the orientation for the "to the left/right" checks.
                     */
                    var isInTriangle = VectorMath.CrossProduct(ab, p - a) >= 0
                        && VectorMath.CrossProduct(bc, p - b) >= 0
                        && VectorMath.CrossProduct(ca, p - c) >= 0;

                    if (isInTriangle)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }

        public void DrawRectangleRotated(Vector2 position, float width, float height, Color color, float angle, Vector2 origin)
        {
            // corners
            var topLeft = position;
            var topRight = position + new Vector2(width, 0);
            var bottomLeft = position + new Vector2(0, height);
            var bottomRight = position + new Vector2(width, height);

            // rotated corners
            var topLeftRotated = VectorMath.RotatePoint(topLeft, angle, origin);
            var topRightRotated = VectorMath.RotatePoint(topRight, angle, origin);
            var bottomLeftRotated = VectorMath.RotatePoint(bottomLeft, angle, origin);
            var bottomRightRotated = VectorMath.RotatePoint(bottomRight, angle, origin);

            // draw the rectangle
            DrawRectangleByPoints(topLeftRotated, topRightRotated, bottomRightRotated, bottomLeftRotated, color);
        }

        // TODO(improvement): very similar to triangle code - generalize?
        private void DrawRectangleByPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
        {
            var pixelA = ToPixel(a);
            var pixelB = ToPixel(b);
            var pixelC = ToPixel(c);
            var pixelD = ToPixel(d);

            // bounding box
            var pxMin = MathExtensions.Min(pixelA.X, pixelB.X, pixelC.X, pixelD.X);
            var pxMax = MathExtensions.Max(pixelA.X, pixelB.X, pixelC.X, pixelD.X);
            var pyMin = MathExtensions.Min(pixelA.Y, pixelB.Y, pixelC.Y, pixelD.Y);
            var pyMax = MathExtensions.Max(pixelA.Y, pixelB.Y, pixelC.Y, pixelD.Y);

            // visible part of bounding box
            var pxStart = Math.Max(pxMin, 0);
            var pxEnd = Math.Min(pxMax, Width);
            var pyStart = Math.Max(pyMin, 0);
            var pyEnd = Math.Min(pyMax, Height);

            // edges
            var ab = b - a;
            var bc = c - b;
            var cd = d - c;
            var da = a - d;

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    var p = FromPixel(new Vector2Int(ix, iy));
                    /*
                     * Walking around the edges, p should be either to your left or right
                     * the entire time. If it switches back and forth, it can't be in the area.
                     *
                     * The cross product determines the orientation for the "to the left/right" checks.
                     */
                    var isInRectangle = VectorMath.CrossProduct(ab, p - a) >= 0
                        && VectorMath.CrossProduct(bc, p - b) >= 0
                        && VectorMath.CrossProduct(cd, p - c) >= 0
                        && VectorMath.CrossProduct(da, p - d) >= 0;

                    if (isInRectangle)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }
    }
}
