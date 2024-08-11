using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.LevelGameplay;
using GameFromScratch.App.Gameplay.LevelSelection;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay
{
    internal class Game
    {
        private readonly IWindowManager windowManager;
        private readonly IGraphics2D graphics;
        private readonly Camera2D camera;

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
            this.camera = camera;

            fpsThrottler = new FpsThrottler(targetFps, windowManager.Sleeper);
            fpsSampler = new FpsSampler(fpsSampleWindow);

            var tools = new GameTools(graphics, windowManager.Input, camera);
            simulation = new Simulation(tools);
            levelSelector = new LevelSelector(tools);

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
                DrawFpsOverlay();

                graphics.Commit();
                windowManager.Input.Refresh();

                frameTimer.Restart();
            }
        }

        private void DrawFpsOverlay()
        {
            if (debugMode)
            {
                fpsSampler.Sample();
                graphics.PixelMode = true;
                graphics.DrawText($"FPS: {MathF.Round(fpsSampler.Fps, 3)}", 16, Color.Green, new Vector2(10, 10));
                graphics.PixelMode = false;
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
