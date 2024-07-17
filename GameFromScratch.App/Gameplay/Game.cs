using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Gameplay.Simulations;
using System.Diagnostics;
using System.Drawing;

namespace GameFromScratch.App.Gameplay
{
    internal class Game
    {
        private readonly IWindowManager windowManager;
        private readonly IGraphics2D graphics;

        private readonly FpsThrottler fpsThrottler;
        private readonly FpsSampler fpsSampler;
        private const int targetFps = 60;
        private const int fpsSampleWindow = 100;

        // game modes
        private readonly Simulation simulation;
        private readonly LevelSelector levelSelector;

        private readonly bool debugMode;

        private bool didInitializeLevel = false;

        public Game(IWindowManager windowManager, IGraphics2D graphics, Camera2D camera, bool debugMode)
        {
            this.windowManager = windowManager;
            this.graphics = graphics;

            fpsThrottler = new FpsThrottler(targetFps, windowManager.Sleeper);
            fpsSampler = new FpsSampler(fpsSampleWindow);

            simulation = new Simulation(new SimulationTools(graphics, windowManager.Input, camera));
            levelSelector = new LevelSelector(windowManager.Input);

            this.debugMode = debugMode;
        }

        public void Run()
        {
            windowManager.CreateWindow();

            var frameTimer = new Stopwatch();

            while (windowManager.IsRunning)
            {
                windowManager.ProcessMessages();
                fpsThrottler.SleepUntilNextFrame();

                graphics.Fill(Color.White);

                RunGameModeFrame((float)frameTimer.Elapsed.TotalSeconds);

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

        private void RunGameModeFrame(float deltaTimeSeconds)
        {
            if (didInitializeLevel)
            {
                simulation.Update(deltaTimeSeconds);
                if (simulation.IsDone)
                {
                    levelSelector.Reset();
                    didInitializeLevel = false;
                }
            }
            else
            {
                levelSelector.Update();
                if (levelSelector.IsReady)
                {
                    simulation.Initialize(levelSelector.SelectedLevel);
                    didInitializeLevel = true;
                }
            }
        }
    }
}
