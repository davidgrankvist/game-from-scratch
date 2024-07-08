using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Platform.Win32Platform;

namespace GameFromScratch.App.Platform
{
    internal static class PlatformManager
    {
        public static IGraphics2D GetGraphics2D(RendererType rendererType, Camera2D camera)
        {
            if (OperatingSystem.IsWindowsVersionAtLeast(7))
            {
                switch (rendererType)
                {
                    case RendererType.SoftwareRenderer:
                        return new Win32BitmapDrawer(camera);
                    default:
                        throw new PlatformNotSupportedException($"Unsupported renderer type {rendererType}");
                }
            }
            throw new PlatformNotSupportedException($"Unsupported OS {Environment.OSVersion}");
        }

        public static IWindowManager GetWindowManager(IGraphics2D graphics, Camera2D camera)
        {
            if (OperatingSystem.IsWindowsVersionAtLeast(7))
            {
                switch (graphics)
                {
                    case IWin32Graphics2D win32Graphics2D:
                        return new Win32WindowManager(win32Graphics2D, camera);
                    default:
                        throw new PlatformNotSupportedException("Unsupported graphics for this platform");
                }
            }

            throw new PlatformNotSupportedException($"Unsupported OS {Environment.OSVersion}");
        }
    }
}
