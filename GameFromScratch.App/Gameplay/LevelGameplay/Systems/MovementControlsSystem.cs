using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class MovementControlsSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            var player = context.State.Repository.Player;

            var playerVx = player.Velocity.X;
            var playerVy = player.Velocity.Y;

            var isTouchingGround = playerVy == 0;

            // prevent sliding on the floor
            if (isTouchingGround)
            {
                playerVx = 0;
            }

            if (input.IsPressed(KeyCode.W) && isTouchingGround)
            {
                playerVy = -player.JumpSpeed * context.State.GravitySign;
            }
            if (input.IsDown(KeyCode.A))
            {
                playerVx = -player.Speed;
            }
            if (input.IsDown(KeyCode.D))
            {
                playerVx = player.Speed;
            }

            player.Velocity = new Vector2(playerVx, playerVy);
        }
    }
}
