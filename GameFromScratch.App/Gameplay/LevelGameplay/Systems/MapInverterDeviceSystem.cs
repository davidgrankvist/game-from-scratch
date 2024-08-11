using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class MapInverterDeviceSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            if (!input.IsPressed(KeyCode.MouseRight))
            {
                return;
            }

            var repo = context.State.Repository;
            var entitiesToInvert = repo.Query(EntityFlags.Invert);
            foreach (var entity in entitiesToInvert)
            {
                entity.Flags = entity.Flags ^ (EntityFlags.Solid | EntityFlags.Render);
            }
        }
    }
}
