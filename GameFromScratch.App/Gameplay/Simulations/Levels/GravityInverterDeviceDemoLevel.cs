using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal class GravityInverterDeviceDemoLevel : ILevel
    {
        public IEnumerable<Entity> Create()
        {
            var mapSize = LevelUtils.MAP_SIZE;

            var groundHeight = 50;
            var groundBottomLeft = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - groundHeight),
                Bounds = new Vector2(mapSize.X / 2, groundHeight),
                Color = Color.Black,
            };
            var groundTopRight = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(mapSize.X / 2, 0),
                Bounds = new Vector2(mapSize.X / 2, groundHeight),
                Color = Color.Black,
            };

            var player = LevelUtils.CreatePlayer();

            return [
                groundBottomLeft,
                groundTopRight,
                player,
            ];
        }
    }
}
