using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Framework.Maths;
using GameFromScratch.App.Platform.Common.Text;
using GameFromScratch.App.Platform.Common.Textures;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Platform.Common
{
    internal abstract class SoftwareRenderer2D : IGraphics2D
    {
        protected int[] bitmap;
        protected int Width;
        protected int Height;

        private readonly Camera2D camera;
        public bool PixelMode { get => camera.PixelMode; set => camera.PixelMode = value; }

        private readonly TextRenderer textRenderer;
        private bool didInitText;
        private int textBaselineY;
        private int textOffsetLeft;

        private readonly TextureManager textureManager;

        public SoftwareRenderer2D(Camera2D camera)
        {
            this.camera = camera;
            bitmap = Array.Empty<int>();
            textRenderer = new TextRenderer();
            textureManager = new TextureManager();
        }

        public virtual void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            bitmap = new int[width * height];
        }

        // implement in platform code
        public abstract void Commit();

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

        private Color GetPixel(int x, int y)
        {
            return Color.FromArgb(bitmap[ToIndex(x, y)]);
        }

        public void DrawRectangle(Vector2 position, float width, float height, Color color)
        {
            var topLeftPixel = camera.ToPixel(position);
            var bottomRightPixel = camera.ToPixel(position + new Vector2(width, height));

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
            var boundingBoxTopLeft = camera.ToPixel(position - new Vector2(radius, radius));
            var boundingBoxBottomRight = camera.ToPixel(position + new Vector2(radius, radius));

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
                    var delta = camera.FromPixel(new Vector2Int(ix, iy)) - position;

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
            var pixelA = camera.ToPixel(a);
            var pixelB = camera.ToPixel(b);
            var pixelC = camera.ToPixel(c);

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
                    var p = camera.FromPixel(new Vector2Int(ix, iy));
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
            var pixelA = camera.ToPixel(a);
            var pixelB = camera.ToPixel(b);
            var pixelC = camera.ToPixel(c);
            var pixelD = camera.ToPixel(d);

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
                    var p = camera.FromPixel(new Vector2Int(ix, iy));
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

        public void DrawText(string text, int fontSize, Color color, Vector2 position)
        {
            if (!didInitText)
            {
                textRenderer.LoadFont();
                didInitText = true;
            }

            if (textRenderer.FontSize != fontSize)
            {
                textRenderer.SetFontSize(fontSize);
            }

            var characterTopLeft = camera.ToPixel(position);
            var isFirst = true;

            foreach (var character in text)
            {
                var glyph = textRenderer.DrawCharacter(character);
                DrawCharacterGlyph(glyph, characterTopLeft, color, isFirst);

                var spacing = glyph.AdvanceX;
                characterTopLeft += new Vector2Int(spacing, 0);

                isFirst = false;
            }
        }

        private void DrawCharacterGlyph(GlyphBitmap glyph, Vector2Int firstCharacterTopLeftPixel, Color color, bool isFirst)
        {
            if (isFirst)
            {
                // make sure subsequent characters are positioned relative to the first character top left
                textBaselineY = firstCharacterTopLeftPixel.Y + glyph.Top;
                textOffsetLeft = glyph.Left;
            }

            var glyphTopLeftX = firstCharacterTopLeftPixel.X + glyph.Left - textOffsetLeft;
            var glyphTopLeftY = textBaselineY - glyph.Top;

            var pxStart = MathExtensions.Clamp(glyphTopLeftX, 0, Width);
            var pyStart = MathExtensions.Clamp(glyphTopLeftY, 0, Height);
            var pxEnd = MathExtensions.Clamp(pxStart + glyph.Width, 0, Width);
            var pyEnd = MathExtensions.Clamp(pyStart + glyph.Height, 0, Height);

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    // position within the glyph's own bitmap
                    var glyphX = ix - pxStart;
                    var glyphY = iy - pyStart;

                    var alpha = glyph.Buffer[glyphY * glyph.Width + glyphX];
                    if (alpha > 0)
                    {
                        // Handle alpha values by blending with the background color.
                        var backgroundColor = GetPixel(ix, iy);
                        var blendedColor = Blend(backgroundColor, color, alpha);
                        SetPixel(ix, iy, blendedColor);
                    }
                }
            }
        }

        private static Color Blend(Color source, Color dest, byte alpha)
        {
            var alphaf = alpha / 255f;

            var r = LerpByte(source.R, dest.R, alphaf);
            var g = LerpByte(source.G, dest.G, alphaf);
            var b = LerpByte(source.B, dest.B, alphaf);

            return Color.FromArgb(r, g, b);
        }

        private static byte LerpByte(byte x, byte y, float t)
        {
            return (byte)MathF.Round(MathExtensions.Lerp(x, y, t));
        }

        public void DrawTexture(string textureName, Vector2 position)
        {
            var texture = textureManager.GetTexture(textureName);
            DrawTexture(texture, position, texture.Width, texture.Height);
        }

        public void DrawTexture(string textureName, Vector2 position, float width, float height)
        {
            var texture = textureManager.GetTexture(textureName);
            DrawTexture(texture, position, width, height);
        }

        private void DrawTexture(Texture texture, Vector2 position, float width, float height)
        {
            var widthScale = width / texture.Width;
            var heightScale = height / texture.Height;

            var topLeftPixel = camera.ToPixel(position);
            var bottomRightPixel = camera.ToPixel(position + new Vector2(width, height));

            // visible part of rectangle
            var pxStart = Math.Max(topLeftPixel.X, 0);
            var pxEnd = Math.Min(bottomRightPixel.X, Width);
            var pyStart = Math.Max(topLeftPixel.Y, 0);
            var pyEnd = Math.Min(bottomRightPixel.Y, Height);

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    // position within the scaled texture
                    var scaledTextureX = ix - pxStart;
                    var scaledTextureY = iy - pyStart;

                    // nearest actual pixel within the original texture
                    var originalTextureX = MathExtensions.Clamp((int)MathF.Round(scaledTextureX / widthScale), 0, texture.Width - 1);
                    var originalTextureY = MathExtensions.Clamp((int)MathF.Round(scaledTextureY / heightScale), 0, texture.Height - 1);

                    var color = Color.FromArgb(texture.Buffer[originalTextureY * texture.Width + originalTextureX]);
                    if (color.A > 0)
                    {
                        // Handle alpha values by blending with the background color.
                        var backgroundColor = GetPixel(ix, iy);
                        var blendedColor = Blend(backgroundColor, color, color.A);
                        SetPixel(ix, iy, blendedColor);
                    }
                }
            }
        }

    }
}
