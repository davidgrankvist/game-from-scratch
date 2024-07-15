using GameFromScratch.App.Framework.Input;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class GravityInverterDeviceSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            var input = context.Tools.Input;
            if (input.IsPressed(KeyCode.S))
            {
                context.State.GravitySign *= -1;
            }
        }
    }
}
