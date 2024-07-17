using GameFromScratch.App.Gameplay.Simulations.Levels;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class SpawnSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
            var level = new ConceptLevel();

            var repo = context.State.Repository;
            repo.AddRange(level.Create());
        }

        public void Update(SimulationContext context)
        {
        }
    }
}
