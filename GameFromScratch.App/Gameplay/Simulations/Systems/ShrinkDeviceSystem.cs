using GameFromScratch.App.Framework.Input;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class ShrinkDeviceSystem : ISystem
    {
        private const float minScale = 0.5f;
        private const float maxScale = 1;
        private float scale = 1;
        private const float scaleStep = 0.005f;

        private Vector2 boundsStart;
        private float speedStart;
        private float jumpSpeedStart;

        public void Initialize(SimulationContext context)
        {
            var player = context.State.Repository.Player;
            boundsStart = player.Bounds;
            speedStart = player.Speed;
            jumpSpeedStart = player.JumpSpeed;
        }

        public void Update(SimulationContext context)
        {
            var input = context.Tools.Input;
            var didScale = false;

            if (input.IsDown(KeyCode.MouseLeft))
            {
                scale = MathF.Max(scale - scaleStep, minScale);
                didScale = true;
            }

            if (input.IsDown(KeyCode.MouseRight))
            {
                scale = MathF.Min(scale + scaleStep, maxScale);
                didScale = true;
            }

            if (!didScale)
            {
                return;
            }

            var player = context.State.Repository.Player;
            player.Speed = scale * speedStart;
            player.JumpSpeed = scale * jumpSpeedStart;
            player.Bounds = scale * boundsStart;

            // TODO(feature): adjust camera scale
        }
    }
}
