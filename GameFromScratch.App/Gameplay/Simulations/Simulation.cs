using GameFromScratch.App.Gameplay.Simulations.Levels;
using GameFromScratch.App.Gameplay.Simulations.Systems;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class Simulation
    {
        private readonly SimulationTools tools;
        private SimulationContext context;
        private List<ISystem> systems;

        public bool IsDone { get => context.State.CompletedLevel; }

        public Simulation(SimulationTools tools)
        {
            this.tools = tools;
            context = new SimulationContext(tools);
            systems = new List<ISystem>();
        }

        private void Reset()
        {
            context = new SimulationContext(tools);
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
                new ShrinkDeviceSystem(),
                new MovementControlsSystem(),
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
