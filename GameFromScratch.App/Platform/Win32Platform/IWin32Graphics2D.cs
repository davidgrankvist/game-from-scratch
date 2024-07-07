using GameFromScratch.App.Framework.Graphics;
using System.Runtime.Versioning;
using Windows.Win32.Foundation;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal interface IWin32Graphics2D : IGraphics2D
    {
        public HWND Hwnd { get; set; }
    }
}
