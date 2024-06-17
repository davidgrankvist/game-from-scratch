using System.Runtime.Versioning;
using GameFromScratch.App.Framework;
using Windows.Win32.Foundation;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal interface IWin32Graphics2D : IGraphics2D
    {
        public HWND Hwnd { get; set; }
    }
}
