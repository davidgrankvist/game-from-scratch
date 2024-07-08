using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Gameplay.Simulations;
using System.Diagnostics;

namespace GameFromScratch.App.Gameplay
{
    internal class Game
    {
        private readonly IWindowManager windowManager;
        private readonly IGraphics2D graphics;
        private readonly FpsThrottler fpsThrottler;
        private readonly FpsSampler fpsSampler;

        private const int targetFps = 60;
        private const bool debugMode = false;
        private const int fpsSampleWindow = 100;

        private readonly Simulation simulation;

        public Game(IWindowManager windowManager, IGraphics2D graphics, Camera2D camera)
        {
            this.windowManager = windowManager;
            this.graphics = graphics;

            fpsThrottler = new FpsThrottler(targetFps, windowManager.Sleeper);
            fpsSampler = new FpsSampler(fpsSampleWindow);

            simulation = new Simulation(new SimulationTools(graphics, windowManager.Input, camera));
        }

        public void Run()
        {
            windowManager.CreateWindow();
            simulation.Initialize();

            var frameTimer = new Stopwatch();

            while (windowManager.IsRunning)
            {
                windowManager.ProcessMessage();
                fpsThrottler.SleepUntilNextFrame();

                simulation.Update((float)frameTimer.Elapsed.TotalSeconds);
                graphics.Commit();
                windowManager.Input.Refresh();

                PrintFps();

                frameTimer.Restart();
            }
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
