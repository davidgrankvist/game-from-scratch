using GameFromScratch.App.Framework.Input;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class DemoSystem : ISystem
    {
        private float animatedObjectPosX = 0;
        private float animatedObjectVx = 100f;

        private Vector2 playerPos = new Vector2(200, 200);
        private float playerSpeed = 120f;

        public void Initialize()
        {
        }

        public void Update(SimulationContext context)
        {
            DrawCursor(context);
            AnimateObject(context);
            MovePlayer(context);
        }

        private void DrawCursor(SimulationContext context)
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

        private void MovePlayer(SimulationContext context)
        {
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

            var playerVelocity = new Vector2(playerVx, playerVy);
            playerPos = playerPos + playerVelocity * context.State.DeltaTime;

            context.Tools.Graphics.DrawCircle(playerPos, 25, Color.Blue);
        }
    }
}
