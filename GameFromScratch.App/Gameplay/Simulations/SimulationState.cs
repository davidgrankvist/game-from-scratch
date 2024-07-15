using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class SimulationState
    {
        public readonly EntityRepository Repository;
        public float DeltaTime;
        public float GravitySign;

        public SimulationState()
        {
            Repository = new EntityRepository();
            DeltaTime = 0;
            GravitySign = 1;
        }
    }
}
