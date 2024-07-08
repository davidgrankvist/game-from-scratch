using GameFromScratch.App.Gameplay.Simulations.Systems;
using System.Drawing;

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

        public void Initialize()
        {
            systems.Add(new DemoSystem());

            foreach (var system in systems)
            {
                system.Initialize();
            }
        }

        public void Update(float deltaTimeSeconds)
        {
            context.State.DeltaTime = deltaTimeSeconds;
            context.Tools.Graphics.Fill(Color.White);

            foreach (var system in systems)
            {
                system.Update(context);
            }
        }
    }
}
