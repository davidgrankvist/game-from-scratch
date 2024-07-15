using GameFromScratch.App.Framework.Input;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class GravitySystem : ISystem
    {
        private const float accelerationY = 1350;

        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            var deltaTime = context.State.DeltaTime;
            var player = context.State.Repository.Player;

            player.Velocity += new Vector2(0, accelerationY * deltaTime * context.State.GravitySign);
        }
    }
}
