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

        private static void ResolveCollisions(SimulationContext context)
        {
            /*
             * Approach:
             *  1. Assume that all shapes are rectangles along the same axis
             *  2. Check current overlap in X or Y to detect collision (AABB algorithm)
             *  3. Check previous overlap in X or Y to determine movement direction
             *  4. Adjust to a non-overlapping X or Y
             */
            var repo = context.State.Repository;
            var player = repo.Player;

            /*
             * Phase 1 - Check if the player moves into something stationary.
             *
             * This may alter both the player position and velocity.
             */
            var stationaryEntities = repo.Query(EntityFlags.Solid, EntityFlags.Player)
                // Exclude moving entities, except elevators since they are X axis stationary.
                .Where(entity => !entity.Flags.HasFlag(EntityFlags.Move) || entity.Flags.HasFlag(EntityFlags.Elevator));
            foreach (var entity in stationaryEntities)
            {
                var (overlapX, overlapY) = CheckOverlap(player.Position, player.Bounds, entity.Position, entity.Bounds);
                var didCollide = overlapX && overlapY;

                if (didCollide)
                {
                    ResolveMoveIntoStationary(player, entity, context.State.DeltaTime);
                }
            }

            // TODO(bug): check that elevator collision works well with gravity
            /*
             * Phase 2 - Check if something moves into the player.
             *
             * This may alter either of the entities, depending on who gets "pushed".
             *
             * Only checks elevators for now.
             */
            var elevators = repo.Query(EntityFlags.Solid | EntityFlags.Move | EntityFlags.Elevator, EntityFlags.Player);
            foreach (var elevator in elevators)
            {
                var (overlapX, overlapY) = CheckOverlap(player.Position, player.Bounds, elevator.Position, elevator.Bounds);
                var didCollide = overlapX && overlapY;

                if (didCollide)
                {
                    ResolveElevatorIntoPlayer(elevator, player);
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

        private static void ResolveMoveIntoStationary(Entity moving, Entity stationary, float deltaTime)
        {
            var movingPrevPos = moving.Position - moving.Velocity * deltaTime;
            var (prevOverlapX, prevOverlapY) = CheckOverlap(movingPrevPos, moving.Bounds, stationary.Position, stationary.Bounds);

            // collided from X
            if (!prevOverlapX)
            {
                ResolveMoveIntoStationaryX(moving, stationary);
            }

            // collided from Y
            if (!prevOverlapY)
            {
                ResolveMoveIntoStationaryY(moving, stationary);
            }
        }
        private static void ResolveMoveIntoStationaryX(Entity moving, Entity stationary)
        {
            // moved from left to right
            if (moving.Velocity.X >= 0)
            {
                // stop at stationary left edge
                moving.Position.X = stationary.Position.X - moving.Bounds.X;
            }
            else // moved from right to left
            {
                // stop at stationary right edge
                moving.Position.X = stationary.Position.X + stationary.Bounds.X;
            }

            // stop further X movement
            moving.Velocity = new Vector2(0, moving.Velocity.Y);
        }

        private static void ResolveMoveIntoStationaryY(Entity moving, Entity stationary)
        {
            // moved downwards
            if (moving.Velocity.Y >= 0)
            {
                // stop at stationary top edge
                moving.Position.Y = stationary.Position.Y - moving.Bounds.Y;
            }
            else // moved upwards
            {
                // stop at stationary bottom edge
                moving.Position.Y = stationary.Position.Y + stationary.Bounds.Y;
            }

            // stop further Y movement
            moving.Velocity = new Vector2(moving.Velocity.X, 0);
        }

        private static void ResolveElevatorIntoPlayer(Entity elevator, Entity player)
        {
            /*
             * X axis - this part is stationary and handled in earlier checks
             * Y axis - if there is still a collision, then it must be along this axis
             */

            // moved downwards
            if (elevator.Velocity.Y >= 0)
            {
                // stop elevator at player top edge
                elevator.Position.Y = player.Position.Y - elevator.Bounds.Y;
            }
            else // moved upwards
            {
                // push player to elevator top edge
                player.Position.Y = elevator.Position.Y - player.Bounds.Y;
            }
        }
    }
}
