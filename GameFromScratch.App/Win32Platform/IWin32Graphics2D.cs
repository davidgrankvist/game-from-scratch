using Windows.Win32.Foundation;

namespace GameFromScratch.App.Win32Platform
{
    internal interface IWin32Graphics2D : IGraphics2D
    {
        public HWND hwnd { get; set; }
    }
}
