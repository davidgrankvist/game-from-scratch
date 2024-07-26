using GameFromScratch.App.Framework;
using GameFromScratch.App.Platform.Common;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32BitmapDrawer : SoftwareRenderer2D, IWin32Graphics2D
    {
        private BITMAPINFO bitmapInfo;
        public HWND Hwnd { get; set; }

        public Win32BitmapDrawer(Camera2D camera) : base(camera)
        {
        }

        public override unsafe void Resize(int width, int height)
        {
            base.Resize(width, height);

            var biHeader = new BITMAPINFOHEADER
            {
                biSize = (uint)sizeof(BITMAPINFOHEADER),
                biWidth = width,
                biHeight = -height, // If biHeight is negative, the bitmap is a top-down DIB with the origin at the upper left corner
                biBitCount = 32, // 3 colors + 1 byte padding = 32 bits
                biPlanes = 1,
                biCompression = (uint)BI_COMPRESSION.BI_RGB,
            };
            bitmapInfo = new BITMAPINFO
            {
                bmiHeader = biHeader,
            };
        }

        // Draw the current bitmap right away, without waiting for a WM_PAINT message.
        public override void Commit()
        {
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
                    _ = PInvoke.StretchDIBits(
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
    }
}
