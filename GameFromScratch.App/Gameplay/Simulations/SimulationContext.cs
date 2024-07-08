using GameFromScratch.App.Gameplay.Simulations.Systems;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class SimulationContext
    {
        public readonly SimulationTools Tools;
        public SimulationState State;

        public SimulationContext(SimulationTools tools)
        {
            Tools = tools;
            State = new SimulationState();
        }
    }
}
