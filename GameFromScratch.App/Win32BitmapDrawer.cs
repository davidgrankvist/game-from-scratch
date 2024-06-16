using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace GameFromScratch.App
{
	internal class Win32BitmapDrawer
	{
		private BITMAPINFO bitmapInfo;
		private byte[] bitmap;

		public unsafe void CreateBitmap(int width, int height)
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

			bitmap = new byte[width * height * numColors];
		}

		public void Draw(HWND hwnd)
		{
			HDC hdc = PInvoke.BeginPaint(hwnd, out PAINTSTRUCT ps);

			DrawCurrentBitmap(hdc, ps.rcPaint.Width, ps.rcPaint.Height);

			PInvoke.EndPaint(hwnd, ps);
		}

		private unsafe void DrawCurrentBitmap(HDC hdc, int width, int height)
		{
			var biWidth = bitmapInfo.bmiHeader.biWidth;
			var biHeight = -bitmapInfo.bmiHeader.biHeight; // undo top-down DIB minus sign

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
	}
}
