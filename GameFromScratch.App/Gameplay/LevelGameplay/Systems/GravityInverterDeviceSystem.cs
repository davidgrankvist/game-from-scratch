using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class GravityInverterDeviceSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var state = context.State;
            if (state.IsActiveInput(PlayerInputFlags.UseDevicePress) && state.ActiveDevice == PlayerDevice.GravityInverter)
            {
                context.State.GravitySign *= -1;
            }
        }
    }
}
