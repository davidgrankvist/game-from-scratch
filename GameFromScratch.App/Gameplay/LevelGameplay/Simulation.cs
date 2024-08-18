using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using GameFromScratch.App.Gameplay.LevelGameplay.Systems;
using GameFromScratch.App.Gameplay.LevelGameplay.Systems.Devices;

namespace GameFromScratch.App.Gameplay.LevelGameplay
{
    internal class Simulation
    {
        private readonly GameTools tools;
        private GameContext context;
        private List<ISystem> systems;

        public bool IsDone { get => context.State.CompletedLevel; }

        public Simulation(GameTools tools)
        {
            this.tools = tools;
            context = new GameContext(tools);
            systems = new List<ISystem>();
        }

        private void Reset()
        {
            context = new GameContext(tools);
            systems = new List<ISystem>();
        }

        public void Initialize(ILevel level)
        {
            if (IsDone)
            {
                Reset();
            }

            systems.AddRange([
                new SpawnSystem(level),
                new PlayerControlSystem(),
                new DeviceSwitcherSystem(),
                new ShrinkDeviceSystem(),
                new MovementControlSystem(),
                new GrapplingHookDeviceSystem(),
                new MapInverterDeviceSystem(),
                new GravityInverterDeviceSystem(),
                new GravitySystem(),
                new MovementSystem(),
                new RenderSystem(),
            ]);

            foreach (var system in systems)
            {
                system.Initialize(context);
            }
        }

        public void Update(float deltaTimeSeconds)
        {
            context.State.DeltaTime = deltaTimeSeconds;
            var prevIsDone = IsDone;

            foreach (var system in systems)
            {
                system.Update(context);
            }

            if (IsDone && !prevIsDone)
            {
                Console.WriteLine("Level completed!");
            }
        }
    }
}
