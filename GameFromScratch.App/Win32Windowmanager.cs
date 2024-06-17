using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
namespace GameFromScratch.App
{
	internal class Win32WindowManager
	{
		private Win32BitmapDrawer bitmapDrawer;
		private WNDPROC wndProc; // Prevent window procedure delegate from being garbage collected

		public bool IsRunning { get; private set; }

        public Win32WindowManager(Win32BitmapDrawer bitmapDrawer)
		{
			this.bitmapDrawer = bitmapDrawer;
		}

		public unsafe void CreateWindow()
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
				wndProc = wc.lpfnWndProc;
				PInvoke.RegisterClass(wc);

				// Create and show the window
				HWND hwnd = PInvoke.CreateWindowEx(
					WINDOW_EX_STYLE.WS_EX_LEFT,
					classNameStr,
					"Game From Scratch",
					WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
					PInvoke.CW_USEDEFAULT, PInvoke.CW_USEDEFAULT, PInvoke.CW_USEDEFAULT, PInvoke.CW_USEDEFAULT,
					HWND.Null,
					null,
					moduleHandle,
					null
				);

				if (hwnd == HWND.Null)
				{
					return;
				}
				bitmapDrawer.hwnd = hwnd;

				PInvoke.ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_SHOW);
			}

			IsRunning = true;
		}

		public void ProcessMessage()
		{
			var peek = PInvoke.PeekMessage(out MSG msg, HWND.Null, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE);
			if (msg.message == PInvoke.WM_QUIT)
			{
				IsRunning = false;
				return;
			}
			else if (peek != 0)
			{
				PInvoke.TranslateMessage(msg);
				PInvoke.DispatchMessage(msg);
			}
		}

		private LRESULT WindowProcedure(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
		{
			switch (msg)
			{
				case PInvoke.WM_SIZE:
					PInvoke.GetClientRect(hwnd, out RECT rect);
					bitmapDrawer.CreateBitmap(rect.Width, rect.Height);
					break;
				case PInvoke.WM_DESTROY:
					PInvoke.PostQuitMessage(0);
					break;
				case PInvoke.WM_PAINT:
					// Do nothing as painting is handled by the rendering code
					break;
				default:
					return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			return new LRESULT(0);
		}
	}
}
