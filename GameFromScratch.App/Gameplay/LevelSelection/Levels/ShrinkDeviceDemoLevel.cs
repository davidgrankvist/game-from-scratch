using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.Common.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelSelection.Levels
{
    internal class ShrinkDeviceDemoLevel : ILevel
    {
        public string Name => "Shrinker demo";

        public IEnumerable<Entity> Create()
        {
            var mapSize = LevelUtils.MAP_SIZE;

            var groundHeight = 50;
            var ground = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - groundHeight),
                Bounds = new Vector2(mapSize.X, groundHeight),
                Color = Color.Black,
            };

            var towerWidth = 75;
            var towerHeight = 100;
            var tunnelWidth = 25;

            var flyingTower = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(ground.Bounds.X / 2 - towerWidth / 2, ground.Position.Y - towerHeight - tunnelWidth),
                Bounds = new Vector2(towerWidth, towerHeight),
                Color = Color.Green,
            };

            var player = LevelUtils.CreatePlayer();

            var goal = LevelUtils.CreateGoal(new Vector2(mapSize.X - 75, ground.Position.Y - 50));

            return [
                ground,
                flyingTower,
                player,
                goal,
            ];
        }
    }
}
