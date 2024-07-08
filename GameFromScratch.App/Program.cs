using GameFromScratch.App.Framework;
using GameFromScratch.App.Gameplay;
using GameFromScratch.App.Platform;

namespace GameFromScratch.App
{
    internal class Program
    {
        static void Main()
        {
            var camera = new Camera2D();
            var graphics = PlatformManager.GetGraphics2D(RendererType.SoftwareRenderer, camera);
            var windowManager = PlatformManager.GetWindowManager(graphics, camera);

            var game = new Game(windowManager, graphics, camera);
            game.Run();
        }
    }
}
