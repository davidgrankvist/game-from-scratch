using GameFromScratch.App.Gameplay;
using GameFromScratch.App.Platform;

namespace GameFromScratch.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var graphics = PlatformManager.GetGraphics2D(RendererType.SoftwareRenderer);
            var windowManager = PlatformManager.GetWindowManager(graphics);

            var game = new Game(windowManager, graphics);
            game.Run();
        }
    }
}
