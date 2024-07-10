using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class SpawnSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
            var mapSize = new Vector2(900, 400);
            var level = EntityCreator.CreateConceptLevel(mapSize);

            var repo = context.State.Repository;
            repo.AddRange(level);
        }

        public void Update(SimulationContext context)
        {
        }
    }
}
