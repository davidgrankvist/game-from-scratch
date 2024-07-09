using GameFromScratch.App.Framework.Input;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class DemoSystem : ISystem
    {
        private float animatedObjectPosX;
        private float animatedObjectVx;

        public void Initialize(SimulationContext context)
        {
            animatedObjectPosX = 0;
            animatedObjectVx = 100f;

            var player = context.State.Repository.Player;
            player.Position = new Vector2(200, 200);
            player.Speed = 120f;
        }

        public void Update(SimulationContext context)
        {
            DrawCursor(context);
            AnimateObject(context);
            MovePlayer(context);
        }

        private static void DrawCursor(SimulationContext context)
        {
            context.Tools.Graphics.DrawCircle(context.Tools.Input.MousePosition, 25, Color.Green);
        }

        private void AnimateObject(SimulationContext context)
        {
            var posY0 = 100;
            animatedObjectPosX = animatedObjectPosX + context.State.DeltaTime * animatedObjectVx;
            if (animatedObjectPosX < 0 || animatedObjectPosX > 100)
            {
                animatedObjectVx = -animatedObjectVx;
            }
            var side = 50;
            var pos = new Vector2(animatedObjectPosX, posY0);
            context.Tools.Graphics.DrawRectangle(pos, side, side, Color.Black);
        }

        private static void MovePlayer(SimulationContext context)
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
            player.Position = player.Position + player.Velocity * context.State.DeltaTime;

            var topLeft = player.Position - player.Bounds;
            context.Tools.Graphics.DrawRectangle(topLeft, player.Bounds.X, player.Bounds.Y, Color.Blue);
        }
    }
}
