using System.Drawing;
using System.Numerics;
using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal class ConceptLevel : ILevel
    {
        public IEnumerable<Entity> Create()
        {
            var mapSize = LevelUtils.MAP_SIZE;

            // map
            var towerWidth = 80;
            var tunnelWidth = 30;
            var gapWidth = 2 * tunnelWidth + towerWidth;
            var groundHeight = 50;

            var groundLeft = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - groundHeight),
                Bounds = new Vector2(mapSize.X / 2 - gapWidth / 2, groundHeight),
                Color = Color.Black,
            };
            var groundRight = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(mapSize.X / 2 + gapWidth / 2, mapSize.Y - groundHeight),
                Bounds = new Vector2(mapSize.X / 2 + gapWidth / 2, groundHeight),
                Color = Color.Black,
            };
            var groundMid = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(groundLeft.Bounds.X, groundLeft.Position.Y + tunnelWidth),
                Bounds = new Vector2(gapWidth, groundHeight - tunnelWidth),
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

            var player = LevelUtils.CreatePlayer();

            return [
                groundLeft,
                groundRight,
                groundMid,
                tower,
                invisibleBorderLeft,
                invisibleBorderRight,
                invisibleBorderTop,
                player,
            ];
        }

    }
}
