using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    internal static class EntityCreator
    {

        public static IEnumerable<Entity> CreateConceptLevel(Vector2 mapSize)
        {
            var player = CreatePlayer();
            var map = CreateMap(mapSize);

            yield return player;
            foreach (var entity in map)
            {
                yield return entity;
            }
        }

        private static Entity CreatePlayer()
        {
            return new Entity
            {
                Flags = EntityFlags.Player | EntityFlags.Solid | EntityFlags.Render | EntityFlags.Move,
                Speed = 300,
                JumpSpeed = 4 * 120,
                Position = new Vector2(200, 200),
                Bounds = new Vector2(50, 50),
                Color = Color.Blue,
            };
        }

        private static IEnumerable<Entity> CreateMap(Vector2 mapSize)
        {
            // map
            var towerWidth = 80;
            var tunnelWidth = 30;
            var gapWidth = 2 * tunnelWidth + towerWidth;
            var groundWidth = 50;

            var groundLeft = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - groundWidth),
                Bounds = new Vector2(mapSize.X / 2 - gapWidth / 2, groundWidth),
                Color = Color.Black,
            };
            var groundRight = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(mapSize.X / 2 + gapWidth / 2, mapSize.Y - groundWidth),
                Bounds = new Vector2(mapSize.X / 2 + gapWidth / 2, groundWidth),
                Color = Color.Black,
            };
            var groundMid = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(groundLeft.Bounds.X, groundLeft.Position.Y + tunnelWidth),
                Bounds = new Vector2(gapWidth, groundWidth - tunnelWidth),
                Color = Color.Black,
            };
            var tower = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(groundLeft.Bounds.X + tunnelWidth, groundLeft.Position.Y - 100),
                Bounds = new Vector2(towerWidth, 100),
                Color = Color.Green,
            };

            // borders
            var invisibleBorderLeft = new Entity
            {
                Flags = EntityFlags.Solid,
                Position = new Vector2(-10, 0),
                Bounds = new Vector2(10, mapSize.Y),
            };
            var invisibleBorderRight = new Entity
            {
                Flags = EntityFlags.Solid,
                Position = new Vector2(mapSize.X, 0),
                Bounds = new Vector2(10, mapSize.Y),
            };
            var invisibleBorderTop = new Entity
            {
                Flags = EntityFlags.Solid,
                Position = new Vector2(0, -10),
                Bounds = new Vector2(mapSize.X, 10),
            };

            return [
                groundLeft,
                groundRight,
                groundMid,
                tower,
                invisibleBorderLeft,
                invisibleBorderRight,
                invisibleBorderTop
            ];
        }
    }
}
