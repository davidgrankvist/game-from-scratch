using GameFromScratch.App.Framework.Input;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class PlayerInputSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            var player = context.State.Repository.Player;
            var playerSpeed = player.Speed;
            var playerVx = 0f;
            var playerVy = 0f;
            if (context.Tools.Input.IsDown(KeyCode.W))
            {
                playerVy = -playerSpeed;
            }
            if (context.Tools.Input.IsDown(KeyCode.A))
            {
                playerVx = -playerSpeed;
            }
            if (context.Tools.Input.IsDown(KeyCode.S))
            {
                playerVy = playerSpeed;
            }
            if (context.Tools.Input.IsDown(KeyCode.D))
            {
                playerVx = playerSpeed;
            }

            player.Velocity = new Vector2(playerVx, playerVy);
        }
    }
}
