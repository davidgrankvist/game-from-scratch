using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;

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

        public Game(IWindowManager windowManager, IGraphics2D graphics)
        {
            this.windowManager = windowManager;
            this.graphics = graphics;
            fpsThrottler = new FpsThrottler();

            testAnimation = new TestAnimation(graphics, windowManager.Input);
            fpsSampler = new FpsSampler(100);
        }

        public void Run()
        {
            windowManager.CreateWindow();

            while (windowManager.IsRunning)
            {
                windowManager.ProcessMessage();

                if (!fpsThrottler.PollIsReady())
                {
                    continue;
                }

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
