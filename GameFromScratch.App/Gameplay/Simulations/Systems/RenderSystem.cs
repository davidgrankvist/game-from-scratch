using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class RenderSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            var repo = context.State.Repository;

            // render non-player entities first so they are put in the background
            var nonPlayerRenderEntities = repo.Query(EntityFlags.Render, EntityFlags.Player);
            foreach (var entity in nonPlayerRenderEntities)
            {
                context.Tools.Graphics.DrawRectangle(entity.Position, entity.Bounds.X, entity.Bounds.Y, entity.Color);
            }
            var player = repo.Player;
            context.Tools.Graphics.DrawRectangle(player.Position, player.Bounds.X, player.Bounds.Y, player.Color);
        }
    }
}
