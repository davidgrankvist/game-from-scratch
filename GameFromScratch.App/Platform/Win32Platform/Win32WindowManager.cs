using System.Runtime.Versioning;
using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Input;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32WindowManager : IWindowManager
    {
        private readonly IWin32Graphics2D graphics;
        private readonly Win32InputHandler inputHandler;
        private readonly InputBuffer inputBuffer;
        private readonly Win32Sleeper sleeper;
        private bool isRunning;
        private WNDPROC? wndProc; // Prevent window procedure delegate from being garbage collected

        public bool IsRunning { get => isRunning; }
        public IInputBuffer Input { get => inputBuffer; }
        public ISleeper Sleeper { get => sleeper; }

        public Win32WindowManager(IWin32Graphics2D graphics, Camera2D camera)
        {
            this.graphics = graphics;
            inputBuffer = new InputBuffer(camera);
            inputHandler = new Win32InputHandler(inputBuffer);
            sleeper = new Win32Sleeper();
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
                graphics.Hwnd = hwnd;

                PInvoke.ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_SHOW);
            }

            isRunning = true;

            // make sure Thread.Sleep can handle 1ms delays
            sleeper.Initialize(1);

            /*
             * TODO(hack): This is to handle a WM_PAINT edge case.
             *
             * Problem:
             * We skip WM_PAINT messages so that we can paint at a controlled frame rate.
             * Initially the client area is invalidated, which triggers WM_PAINT messages.
             * Since we skip those, further WM_PAINT messages are triggered and we get stuck
             * in the PeekMessage loop.
             *
             * The workaround here is to paint one time in advance so that the client area is validated
             * and the WM_PAINT messages stop getting triggered.
             */
            graphics.Commit();
        }

        public void ProcessMessages()
        {
            /*
             * Remove all pending input messages from the message queue and trigger the window procedure for each one.
             *
             * The WM_QUIT even is not associated with the current window, so in order to receive that event
             * a null HWND is used here.
             */
            while (PInvoke.PeekMessage(out MSG msg, HWND.Null, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE) != 0)
            {
                if (msg.message == PInvoke.WM_QUIT)
                {
                    isRunning = false;
                    return;
                }
                PInvoke.TranslateMessage(msg);
                PInvoke.DispatchMessage(msg);
            }
        }

        private LRESULT WindowProcedure(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
        {
            var wParamUint = (uint)wParam;
            var lParamInt = (int)lParam;
            switch (msg)
            {
                case PInvoke.WM_SIZE:
                    PInvoke.GetClientRect(hwnd, out RECT rect);
                    graphics.Resize(rect.Width, rect.Height);
                    break;
                case PInvoke.WM_DESTROY:
                    PInvoke.PostQuitMessage(0); // triggers WM_QUIT
                    break;
                case PInvoke.WM_PAINT:
                    // Do nothing as painting is handled by the rendering code
                    break;
                case PInvoke.WM_KEYDOWN:
                    inputHandler.HandleKeyDown(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_KEYUP:
                    inputHandler.HandleKeyUp(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_LBUTTONDOWN:
                    inputHandler.HandleMouseLeftDown(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_LBUTTONUP:
                    inputHandler.HandleMouseLeftUp(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_RBUTTONDOWN:
                    inputHandler.HandleMouseRightDown(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_RBUTTONUP:
                    inputHandler.HandleMouseRightUp(wParamUint, lParamInt);
                    break;
                case PInvoke.WM_MOUSEMOVE:
                    inputHandler.HandleMouseMove(wParamUint, lParamInt);
                    break;
                default:
                    return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
            }
            return new LRESULT(0);
        }
    }
}
