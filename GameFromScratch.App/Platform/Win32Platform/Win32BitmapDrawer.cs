using System.Drawing;
using System.Numerics;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32BitmapDrawer : IWin32Graphics2D
    {
        private BITMAPINFO bitmapInfo;
        private int[] bitmap;

        private int Width;
        private int Height;

        public HWND Hwnd { get; set; }

        public unsafe void Resize(int width, int height)
        {
            // 3 colors + 1 byte padding = 32 bits
            var pixelSize = 32;

            var biHeader = new BITMAPINFOHEADER
            {
                biSize = (uint)sizeof(BITMAPINFOHEADER),
                biWidth = width,
                biHeight = -height, // If biHeight is negative, the bitmap is a top-down DIB with the origin at the upper left corner
                biBitCount = (ushort)pixelSize,
                biPlanes = 1,
                biCompression = (uint)BI_COMPRESSION.BI_RGB,
            };
            bitmapInfo = new BITMAPINFO
            {
                bmiHeader = biHeader,
            };
            Width = width;
            Height = height;

            bitmap = new int[width * height];
        }

        public void Commit()
        {
            /*
			 * Instead of invalidating, waiting for a WM_PAINT and then drawing from the window procedure,
			 * we do it all in one place.
			 *
			 * InvalidateRect invalidates and BeginPaint validates it again, which means WM_PAINT is not triggered.
			 */
            PInvoke.InvalidateRect(Hwnd, (RECT?)null, false);
            HDC hdc = PInvoke.BeginPaint(Hwnd, out PAINTSTRUCT ps);

            DrawCurrentBitmap(hdc, ps.rcPaint.Width, ps.rcPaint.Height);

            PInvoke.EndPaint(Hwnd, ps);
        }

        private unsafe void DrawCurrentBitmap(HDC hdc, int width, int height)
        {
            fixed (BITMAPINFO* pBi = &bitmapInfo)
            {
                fixed (void* pBitmap = bitmap)
                {
                    PInvoke.StretchDIBits(
                        hdc,
                        0, 0, width, height,
                        0, 0, Width, Height,
                        pBitmap,
                        pBi,
                        DIB_USAGE.DIB_RGB_COLORS,
                        ROP_CODE.SRCCOPY
                    );
                }
            }
        }

        /*
         * TODO(improvement): Separate rendering parts below from the Win32 bitmap specifics
         */

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

        private static int ToNearestPixel(float f)
        {
            return (int)(f + 0.5);
        }

        public void DrawRectangle(Vector2 position, float width, float height, Color color)
        {
            // x/y components of corners
            var px = ToNearestPixel(position.X);
            var py = ToNearestPixel(position.Y);
            var pw = ToNearestPixel(position.X + width);
            var ph = ToNearestPixel(position.Y + height);

            // visible part of rectangle
            var pxStart = Math.Max(px, 0);
            var pxEnd = Math.Min(pw, Width);
            var pyStart = Math.Max(py, 0);
            var pyEnd = Math.Min(ph, Height);

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
            // center
            var cx = ToNearestPixel(position.X);
            var cy = ToNearestPixel(position.Y);

            // bounding box
            var px = ToNearestPixel(position.X - radius);
            var py = ToNearestPixel(position.Y - radius);
            var pw = ToNearestPixel(px + 2 * radius);
            var ph = ToNearestPixel(py + 2 * radius);

            // visible part of bounding box
            var pxStart = Math.Max(px, 0);
            var pxEnd = Math.Min(pw, Width);
            var pyStart = Math.Max(py, 0);
            var pyEnd = Math.Min(ph, Height);

            // color pixels where the distance from the center is at most the radius
            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                var dx = (ix - cx);
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    var dy = (iy - cy);
                    var squaredDistance = dx * dx + dy * dy;

                    if (squaredDistance <= radius * radius)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            // pixel coordinates
            var apx = ToNearestPixel(a.X);
            var apy = ToNearestPixel(a.Y);

            var bpx = ToNearestPixel(b.X);
            var bpy = ToNearestPixel(b.Y);

            var cpx = ToNearestPixel(c.X);
            var cpy = ToNearestPixel(c.Y);

            // bounding box
            var pxMin = Math.Min(Math.Min(apx, bpx), cpx);
            var pxMax = Math.Max(Math.Max(apx, bpx), cpx);
            var pyMin = Math.Min(Math.Min(apy, bpy), cpy);
            var pyMax = Math.Max(Math.Max(apy, bpy), cpy);

            // visible part of bounding box
            var pxStart = Math.Min(pxMin, 0);
            var pxEnd = Math.Max(pxMax, Width);
            var pyStart = Math.Min(pyMin, 0);
            var pyEnd = Math.Min(pyMax, Height);

            // triangle edges
            var ab = b - a;
            var bc = c - b;
            var ca = a - c;

            for (var ix = pxStart; ix < pxEnd; ix++)
            {
                for (var iy = pyStart; iy < pyEnd; iy++)
                {
                    var p = new Vector2(ix, iy);
                    /*
                     * Walking around the triangle, p should be either to your left or right
                     * the entire time. If it switches back and forth, it can't be in the triangle.
                     *
                     * The cross product determines the orientation for the "to the left/right" checks.
                     */
                    var isInTriangle = CrossProduct(ab, p - a) >= 0
                        && CrossProduct(bc, p - b) >= 0
                        && CrossProduct(ca, p - c) >= 0;

                    if (isInTriangle)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }

        private static float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public void DrawRectangleRotated(Vector2 position, float width, float height,  Color color, float angle, Vector2 origin)
        {
            // corners
            var topLeft = position;
            var topRight = position + new Vector2(width, 0);
            var bottomLeft = position + new Vector2(0, height);
            var bottomRight = position + new Vector2(width, height);

            // rotated corners
            var topLeftRotated = RotatePoint(topLeft, angle, origin);
            var topRightRotated = RotatePoint(topRight, angle, origin);
            var bottomLeftRotated = RotatePoint(bottomLeft, angle, origin);
            var bottomRightRotated = RotatePoint(bottomRight, angle, origin);

            // draw the rectangle
            DrawRectangleByPoints(topLeftRotated, topRightRotated, bottomRightRotated, bottomLeftRotated, color);
        }

        private static Vector2 RotatePoint(Vector2 point, float angle, Vector2 origin)
        {
            var sin = MathF.Sin(angle);
            var cos = MathF.Cos(angle);

            var translated = point - origin;
            var rotated = new Vector2(translated.X * cos - translated.Y * sin, translated.X * sin + translated.Y * cos);
            var result = rotated + origin;

            return result;
        }

        // TODO(improvement): very similar to triangle code - generalize?
        private void DrawRectangleByPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
        {
            // pixel coordinates
            var apx = ToNearestPixel(a.X);
            var apy = ToNearestPixel(a.Y);

            var bpx = ToNearestPixel(b.X);
            var bpy = ToNearestPixel(b.Y);

            var cpx = ToNearestPixel(c.X);
            var cpy = ToNearestPixel(c.Y);

            var dpx = ToNearestPixel(d.X);
            var dpy = ToNearestPixel(d.Y);

            // bounding box
            var pxMin = Math.Min(Math.Min(apx, bpx), Math.Min(cpx, dpx));
            var pxMax = Math.Max(Math.Max(apx, bpx), Math.Max(cpx, dpx));
            var pyMin = Math.Min(Math.Min(apy, bpy), Math.Min(cpy, dpy));
            var pyMax = Math.Max(Math.Max(apy, bpy), Math.Max(cpy, dpy));

            // visible part of bounding box
            var pxStart = Math.Min(pxMin, 0);
            var pxEnd = Math.Max(pxMax, Width);
            var pyStart = Math.Min(pyMin, 0);
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
                    var p = new Vector2(ix, iy);
                    /*
                     * Walking around the edges, p should be either to your left or right
                     * the entire time. If it switches back and forth, it can't be in the area.
                     *
                     * The cross product determines the orientation for the "to the left/right" checks.
                     */
                    var isInRectangle = CrossProduct(ab, p - a) >= 0
                        && CrossProduct(bc, p - b) >= 0
                        && CrossProduct(cd, p - c) >= 0
                        && CrossProduct(da, p - d) >= 0;

                    if (isInRectangle)
                    {
                        SetPixel(ix, iy, color);
                    }
                }
            }
        }
    }
}
