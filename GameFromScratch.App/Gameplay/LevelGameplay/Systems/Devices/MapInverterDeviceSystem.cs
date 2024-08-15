using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems.Devices
{
    internal class MapInverterDeviceSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var state = context.State;
            if (!(state.IsActiveInput(PlayerInputFlags.UseDevicePress) && state.ActiveDevice == PlayerDevice.MapInverter))
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
