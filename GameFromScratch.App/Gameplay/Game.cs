using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Graphics;

namespace GameFromScratch.App.Gameplay
{
    internal class Game
    {
        private readonly IWindowManager windowManager;
        private readonly IGraphics2D graphics;
        private readonly FpsThrottler fpsThrottler;

        private readonly TestAnimation testAnimation;
        private readonly FpsSampler fpsSampler;

        private const bool debugMode = false;

        public Game(IWindowManager windowManager, IGraphics2D graphics, Camera2D camera)
        {
            this.windowManager = windowManager;
            this.graphics = graphics;
            fpsThrottler = new FpsThrottler(60, windowManager.Sleeper);

            testAnimation = new TestAnimation(graphics, windowManager.Input, camera);
            fpsSampler = new FpsSampler(100);
        }

        public void Run()
        {
            windowManager.CreateWindow();

            while (windowManager.IsRunning)
            {
                windowManager.ProcessMessage();
                fpsThrottler.SleepUntilNextFrame();

                Update();
                PrintFps();

                windowManager.Input.Refresh();
            }
        }

        private void Update()
        {
            testAnimation.Update();
        }

        private void PrintFps()
        {
            if (debugMode)
            {
                fpsSampler.Sample();
                Console.WriteLine($"FPS: {fpsSampler.Fps}");
            }
        }
    }
}
