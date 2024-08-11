using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class MovementControlSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            var state = context.State;
            var player = state.Repository.Player;

            var playerVx = player.Velocity.X;
            var playerVy = player.Velocity.Y;

            var isTouchingGround = playerVy == 0;

            // prevent sliding on the floor
            if (isTouchingGround)
            {
                playerVx = 0;
            }

            if (state.IsActiveInput(PlayerInputFlags.MoveJump) && isTouchingGround)
            {
                playerVy = -player.JumpSpeed * context.State.GravitySign;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveLeft))
            {
                playerVx = -player.Speed;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveRight))
            {
                playerVx = player.Speed;
            }

            player.Velocity = new Vector2(playerVx, playerVy);
        }
    }
}
