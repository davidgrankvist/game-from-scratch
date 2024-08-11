using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class SpawnSystem : ISystem
    {
        private readonly ILevel level;

        public SpawnSystem(ILevel level)
        {
            this.level = level;
        }

        public void Initialize(GameContext context)
        {
            var entities = level.Create();

            var repo = context.State.Repository;
            repo.AddRange(entities);
        }

        public void Update(GameContext context)
        {
        }
    }
}
