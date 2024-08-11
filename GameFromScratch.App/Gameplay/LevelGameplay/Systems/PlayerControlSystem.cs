using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class PlayerControlSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            var state = context.State;

            state.InputFlags = PlayerInputFlags.None;

            /* Movement */
            if (input.IsPressed(KeyCode.W))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.MoveJump;
            }
            if (input.IsDown(KeyCode.A))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.MoveLeft;
            }
            if (input.IsDown(KeyCode.D))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.MoveRight;
            }

            /* Devices */
            if (input.IsPressed(KeyCode.MouseLeft))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.UseDevicePress;
            }
            if (input.IsDown(KeyCode.MouseLeft))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.UseDeviceHold;
            }
            if (input.IsPressed(KeyCode.E))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.NextDevice;
            }
            if (input.IsPressed(KeyCode.Q))
            {
                state.InputFlags = state.InputFlags | PlayerInputFlags.PrevDevice;
            }
        }
    }
}
