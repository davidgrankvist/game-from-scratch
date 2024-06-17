using GameFromScratch.App.Framework;

namespace GameFromScratch.App.Gameplay
{
    internal class Game
    {
        private readonly IWindowManager windowManager;
        private readonly IGraphics2D graphics;
        private readonly FpsThrottler fpsThrottler;

        private readonly TestAnimation testAnimation;

        public Game(IWindowManager windowManager, IGraphics2D graphics)
        {
            this.windowManager = windowManager;
            this.graphics = graphics;
            fpsThrottler = new FpsThrottler();

            testAnimation = new TestAnimation(graphics);
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
            }
        }

        public void Update()
        {
            testAnimation.Update();
        }
    }
}
