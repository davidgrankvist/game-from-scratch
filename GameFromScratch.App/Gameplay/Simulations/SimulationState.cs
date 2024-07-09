using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class SimulationState
    {
        public float DeltaTime;
        public readonly EntityRepository Repository;

        public SimulationState()
        {
            DeltaTime = 0;
            Repository = new EntityRepository();
        }
    }
}
