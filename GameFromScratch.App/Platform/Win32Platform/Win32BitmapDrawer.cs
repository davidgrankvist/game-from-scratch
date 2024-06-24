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
            // Assumes that world coordinates are pixels
            var px = ToNearestPixel(position.X);
            var py = ToNearestPixel(position.Y);
            var pw = ToNearestPixel(position.X + width);
            var ph = ToNearestPixel(position.Y + height);

            // Only draw pixels within the viewbox (which is the entire bitmap for now)
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
    }
}
