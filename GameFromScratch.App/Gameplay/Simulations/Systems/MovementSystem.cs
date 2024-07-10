using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal class MovementSystem : ISystem
    {
        public void Initialize(SimulationContext context)
        {
        }

        public void Update(SimulationContext context)
        {
            UpdateElevatorVelocities(context);
            MoveEntities(context);
        }

        private static void UpdateElevatorVelocities(SimulationContext context)
        {
            var repo = context.State.Repository;
            var elevators = repo.Query(EntityFlags.Elevator);

            foreach (var entity in elevators)
            {
                /*
                 * Elevator up and down movement
                 */
                var py = entity.Position.Y;
                var vy = entity.Velocity.Y;
                // switch direction if reached start/end
                if (py >= entity.MoveStart.Y && vy > 0 || py <= entity.MoveEnd.Y && vy < 0)
                {
                    vy = -vy;
                }

                entity.Velocity = new Vector2(0, vy);
            }
        }

        private static void MoveEntities(SimulationContext context)
        {
            var repo = context.State.Repository;
            var entitiesToMove = repo.Query(EntityFlags.Move);

            foreach (var entity in entitiesToMove)
            {
                entity.Position = entity.Position + entity.Velocity * context.State.DeltaTime;
            }
        }
    }
}
