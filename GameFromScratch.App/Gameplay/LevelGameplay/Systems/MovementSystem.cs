using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class MovementSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            MoveEntities(context);
            ResolveCollisions(context);
        }

        private static void MoveEntities(GameContext context)
        {
            var repo = context.State.Repository;
            var entitiesToMove = repo.Query(EntityFlags.Move);

            foreach (var entity in entitiesToMove)
            {
                entity.Position = entity.Position + entity.Velocity * context.State.DeltaTime;
            }
        }

        private static void ResolveCollisions(GameContext context)
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

            // Check if the player moves into something stationary
            var stationaryEntities = repo.Query(EntityFlags.Solid, EntityFlags.Player);
            foreach (var entity in stationaryEntities)
            {
                var (overlapX, overlapY) = CheckOverlap(player.Position, player.Bounds, entity.Position, entity.Bounds);
                var didCollide = overlapX && overlapY;

                if (didCollide)
                {
                    ResolveMoveIntoStationary(player, entity, context.State.DeltaTime);

                    if (entity.Flags.HasFlag(EntityFlags.Goal))
                    {
                        context.State.CompletedLevel = true;
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
    }
}
