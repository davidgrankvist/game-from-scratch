using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class MapInverterDeviceSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            var input = context.Tools.Input;
            if (!input.IsPressed(KeyCode.MouseRight))
            {
                return;
            }

            var repo = context.State.Repository;
            var entitiesToInvert = repo.Query(EntityFlags.Invert);
            foreach (var entity in entitiesToInvert)
            {
                entity.Flags = entity.Flags ^ (EntityFlags.Solid | EntityFlags.Render);
            }
        }
    }
}
