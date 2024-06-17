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
        private byte[] bitmap;

        private int biWidth;
        private int biHeight;

        public HWND Hwnd { get; set; }

        public unsafe void Resize(int width, int height)
        {
            var numColors = 3;
            var bitsPerColor = 8;

            var biHeader = new BITMAPINFOHEADER
            {
                biSize = (uint)sizeof(BITMAPINFOHEADER),
                biWidth = width,
                biHeight = -height, // If biHeight is negative, the bitmap is a top-down DIB with the origin at the upper left corner
                biBitCount = (ushort)(numColors * bitsPerColor),
                biPlanes = 1,
                biCompression = (uint)BI_COMPRESSION.BI_RGB,
            };
            bitmapInfo = new BITMAPINFO
            {
                bmiHeader = biHeader,
            };
            biWidth = width;
            biHeight = height;

            bitmap = new byte[width * height * numColors];
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
                        0, 0, biWidth, biHeight,
                        pBitmap,
                        pBi,
                        DIB_USAGE.DIB_RGB_COLORS,
                        ROP_CODE.SRCCOPY
                    );
                }
            }
        }

        public void Fill(byte r, byte g, byte b)
        {
            for (int i = 0; i < bitmap.Length; i += 3)
            {
                // On Windows, the color order is reversed
                bitmap[i] = b;
                bitmap[i + 1] = g;
                bitmap[i + 2] = r;
            }
        }
    }
}
