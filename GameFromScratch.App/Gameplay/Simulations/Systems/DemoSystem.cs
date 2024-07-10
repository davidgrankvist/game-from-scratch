using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class DemoSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
            var repo = context.State.Repository;

            var player = repo.Player;
            player.Position = new Vector2(200, 200);
            player.Speed = 120f;

            var mapSize = new Vector2(900, 400);
            repo.AddRange(EntityCreator.CreateMap(mapSize));
        }

        public void Update(SimulationContext context)
        {
            MovePlayer(context);
            MovePeriodicEntities(context);
            Render(context);
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
        }

        private static void MovePeriodicEntities(SimulationContext context)
        {
            var repo = context.State.Repository;
            var periodicEntities = repo.Query(EntityFlags.MovePeriodic);

            foreach (var entity in periodicEntities)
            {
                /*
                 * Only consider elevator up and down movement for now
                 */
                var py = entity.Position.Y;
                var vy = entity.Velocity.Y;
                // switch direction if reached start/end
                if (py >= entity.MoveStart.Y && vy > 0 || py <= entity.MoveEnd.Y && vy < 0)
                {
                    vy = -vy;
                }

                entity.Velocity = new Vector2(0, vy);
                entity.Position = entity.Position + entity.Velocity * context.State.DeltaTime;
            }
        }

        private static void Render(SimulationContext context)
        {
            var repo = context.State.Repository;
            var entitiesToRender = repo.Query(EntityFlags.Render);
            foreach (var entity in entitiesToRender)
            {
                if (entity == repo.Player)
                {
                    continue;
                }
                context.Tools.Graphics.DrawRectangle(entity.Position, entity.Bounds.X, entity.Bounds.Y, entity.Color);
            }
            var player = repo.Player;
            context.Tools.Graphics.DrawRectangle(player.Position, player.Bounds.X, player.Bounds.Y, player.Color);
        }
    }
}
