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
            ResolveCollisions(context);
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

        // TODO(cleanup): yikes
        private static void ResolveCollisions(SimulationContext context)
        {
            var repo = context.State.Repository;
            var solidNonPlayerEntities = repo.Query(EntityFlags.Solid, EntityFlags.Player);
            var player = repo.Player;

            /*
             * Approach:
             *  1. Assume that all shapes are rectangles along the same axis
             *  2. Use Aligned Axis Bounding Box (AABB) collision detection
             *  3. Check previous overlap in X or Y
             *  4. Adjust to a non-overlapping X or Y
             */
            // TODO(bug): handle elevators
            foreach (var entity in solidNonPlayerEntities)
            {
                // DETECT
                var playerMinX = player.Position.X;
                var playerMaxX = player.Position.X + player.Bounds.X;
                var playerMinY = player.Position.Y;
                var playerMaxY = player.Position.Y + player.Bounds.Y;

                var entityMinX = entity.Position.X;
                var entityMaxX = entity.Position.X + entity.Bounds.X;
                var entityMinY = entity.Position.Y;
                var entityMaxY = entity.Position.Y + entity.Bounds.Y;

                var overlapX = playerMaxX > entityMinX && playerMinX < entityMaxX;
                var overlapY = playerMaxY > entityMinY && playerMinY < entityMaxY;
                var didCollide = overlapX && overlapY;

                // RESOLVE
                if (didCollide)
                {
                    var playerPrevPos = player.Position - player.Velocity * context.State.DeltaTime;
                    var playerPrevMinX = playerPrevPos.X;
                    var playerPrevMaxX = playerPrevPos.X + player.Bounds.X;
                    var playerPrevMinY = playerPrevPos.Y;
                    var playerPrevMaxY = playerPrevPos.Y + player.Bounds.Y;

                    var prevOverlapX = playerPrevMaxX > entityMinX && playerPrevMinX < entityMaxX;
                    var prevOverlapY = playerPrevMaxY > entityMinY && playerPrevMinY < entityMaxY;

                    // TODO(improvement): pick nearest OK position rather than just the previous position
                    if (!prevOverlapX)
                    {
                        // collided from X
                        player.Position.X = playerPrevPos.X;
                    }
                    if (!prevOverlapY)
                    {
                        // collided from Y
                        player.Position.Y = playerPrevPos.Y;
                    }
                }
            }
        }
    }
}
