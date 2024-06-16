using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameFromScratch.App
{
	internal class Win32WindowManager
	{
		public static unsafe void CreateWindow()
		{
			var moduleHandle = PInvoke.GetModuleHandle((string?)null);
			var hInstance = new HINSTANCE(moduleHandle.DangerousGetHandle());

			var classNameStr = "Sample Window Class";
			fixed (char* pClassName = classNameStr)
			{
				// Set up and register window class
				var className = new PCWSTR(pClassName);
				var wc = new WNDCLASSW
				{
					lpfnWndProc = WindowProcedure,
					hInstance = hInstance,
					lpszClassName = className,
				};
				PInvoke.RegisterClass(wc);

				// Create and show the window
				HWND hwnd = PInvoke.CreateWindowEx(
					WINDOW_EX_STYLE.WS_EX_LEFT,
					classNameStr,
					"Game From Scratch",
					WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
					PInvoke.CW_USEDEFAULT,
					PInvoke.CW_USEDEFAULT,
					PInvoke.CW_USEDEFAULT,
					PInvoke.CW_USEDEFAULT,
					HWND.Null,
					null,
					moduleHandle,
					null
				);

				if (hwnd == HWND.Null)
				{
					return;
				}
				PInvoke.ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_SHOW);

				// Message loop
				// On exit -1 is received here, which is why we check for > 0
				while(PInvoke.GetMessage(out MSG msg, hwnd, 0, 0) > 0)
				{
					PInvoke.TranslateMessage(msg);
					PInvoke.DispatchMessage(msg);
				}
            }
		}

		private static LRESULT WindowProcedure(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
		{
			switch (msg)
			{
				case PInvoke.WM_DESTROY:
					PInvoke.PostQuitMessage(0);
					break;
				case PInvoke.WM_PAINT:
					OnPaint(hwnd);
					break;
				default:
					return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			return new LRESULT(0);
		}

		private static void OnPaint(HWND hwnd)
		{
			// make sure client area is refilled when resizing the window
			HDC hdc = PInvoke.BeginPaint(hwnd, out PAINTSTRUCT ps);

			var whiteColorRef = new COLORREF(0x00ffffff); // TODO: RGB macro is not generated for some reason. Create a helper
			var brush = PInvoke.CreateSolidBrush_SafeHandle(whiteColorRef);
			PInvoke.FillRect(hdc, ps.rcPaint, brush);

			PInvoke.EndPaint(hwnd, ps);
		}
	}
}
