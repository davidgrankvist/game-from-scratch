using GameFromScratch.App.Gameplay.Simulations.Levels;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class SpawnSystem : ISystem
    {
        private readonly ILevel level;

        public SpawnSystem(ILevel level)
        {
            this.level = level;
        }

        public void Initialize(SimulationContext context)
        {
            var entities = level.Create();

            var repo = context.State.Repository;
            repo.AddRange(entities);
        }

        public void Update(SimulationContext context)
        {
        }
    }
}
