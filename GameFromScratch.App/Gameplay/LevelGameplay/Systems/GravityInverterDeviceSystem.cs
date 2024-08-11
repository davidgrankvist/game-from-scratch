using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class GravityInverterDeviceSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var input = context.Tools.Input;
            if (input.IsPressed(KeyCode.S))
            {
                context.State.GravitySign *= -1;
            }
        }
    }
}
