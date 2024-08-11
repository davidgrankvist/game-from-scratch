using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class GravitySystem : ISystem
    {
        private const float accelerationY = 1350;

        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var deltaTime = context.State.DeltaTime;
            var player = context.State.Repository.Player;

            player.Velocity += new Vector2(0, accelerationY * deltaTime * context.State.GravitySign);
        }
    }
}
