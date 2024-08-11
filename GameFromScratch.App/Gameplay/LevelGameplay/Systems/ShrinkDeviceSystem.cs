using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class ShrinkDeviceSystem : ISystem
    {
        private const float minScale = 0.4f;
        private float scale = 1;
        private const float scaleStep = 0.5f;

        private Vector2 boundsStart;
        private float speedStart;
        private float jumpSpeedStart;

        public void Initialize(GameContext context)
        {
            var player = context.State.Repository.Player;
            boundsStart = player.Bounds;
            speedStart = player.Speed;
            jumpSpeedStart = player.JumpSpeed;
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            var didScale = false;

            if (input.IsDown(KeyCode.MouseLeft))
            {
                scale = MathF.Max(scale - scaleStep * context.State.DeltaTime, minScale);
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
