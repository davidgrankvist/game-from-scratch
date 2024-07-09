using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class SimulationContext
    {
        public readonly SimulationTools Tools;
        public readonly SimulationState State;

        public SimulationContext(SimulationTools tools)
        {
            Tools = tools;
            State = new SimulationState();
        }
    }
}
