using GameFromScratch.App.Gameplay.Simulations.Levels;
using GameFromScratch.App.Gameplay.Simulations.Systems;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class Simulation
    {
        private readonly SimulationContext context;
        private readonly List<ISystem> systems;

        public Simulation(SimulationTools tools)
        {
            context = new SimulationContext(tools);
            systems = new List<ISystem>();
        }

        public void Initialize(ILevel level)
        {
            systems.AddRange([
                new SpawnSystem(level),
                new ShrinkDeviceSystem(),
                new MovementControlsSystem(),
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

            foreach (var system in systems)
            {
                system.Update(context);
            }
        }
    }
}
