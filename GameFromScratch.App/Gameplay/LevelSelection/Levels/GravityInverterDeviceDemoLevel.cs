using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.Common.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelSelection.Levels
{
    internal class GravityInverterDeviceDemoLevel : ILevel
    {
        public string Name => "Gravity inverter demo";

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

            var goal = LevelUtils.CreateGoal(new Vector2(mapSize.X - 75, 50));

            return [
                groundBottomLeft,
                groundTopRight,
                player,
                goal,
            ];
        }
    }
}
