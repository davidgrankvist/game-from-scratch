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
                var (overlapX, overlapY) = CheckOverlap(player.Position, player.Bounds, entity.Position, entity.Bounds);
                var didCollide = overlapX && overlapY;

                // RESOLVE
                if (didCollide)
                {
                    var playerPrevPos = player.Position - player.Velocity * context.State.DeltaTime;
                    var (prevOverlapX, prevOverlapY) = CheckOverlap(playerPrevPos, player.Bounds, entity.Position, entity.Bounds);

                    // collided from X
                    if (!prevOverlapX)
                    {
                        // moved from left to right
                        if (player.Velocity.X >= 0)
                        {
                            // stop at entity left edge
                            player.Position.X = entity.Position.X - player.Bounds.X;
                        }
                        else // moved from right to left
                        {
                            // stop at entity right edge
                            player.Position.X = entity.Position.X + entity.Bounds.X;
                        }
                    }

                    // collided from Y
                    if (!prevOverlapY)
                    {
                        // moved downwards
                        if (player.Velocity.Y >= 0)
                        {
                            // stop at entity top edge
                            player.Position.Y = entity.Position.Y - player.Bounds.Y;
                        }
                        else // moved upwards
                        {
                            // stop at entity bottom edge
                            player.Position.Y = entity.Position.Y + entity.Bounds.Y;
                        }
                    }
                }
            }
        }

        private static (bool OverlapX, bool OverlapY) CheckOverlap(Vector2 PositionA, Vector2 BoundsA, Vector2 PositionB, Vector2 BoundsB)
        {
            var aMinX = PositionA.X;
            var aMaxX = PositionA.X + BoundsA.X;
            var aMinY = PositionA.Y;
            var aMaxY = PositionA.Y + BoundsA.Y;

            var bMinX = PositionB.X;
            var bMaxX = PositionB.X + BoundsB.X;
            var bMinY = PositionB.Y;
            var bMaxY = PositionB.Y + BoundsB.Y;

            var overlapX = aMaxX > bMinX && aMinX < bMaxX;
            var overlapY = aMaxY > bMinY && aMinY < bMaxY;

            return (overlapX, overlapY);
        }
    }
}
