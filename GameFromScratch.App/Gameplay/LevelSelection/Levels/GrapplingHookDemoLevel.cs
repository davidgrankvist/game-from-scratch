using GameFromScratch.App.Gameplay.Common;
using GameFromScratch.App.Gameplay.Common.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelSelection.Levels
{
    internal class GrapplingHookDemoLevel : ILevel
    {
        public string Name => "Grappling hook demo";

        public IEnumerable<Entity> Create()
        {
            var mapSize = LevelUtils.MAP_SIZE;

            var groundHeight = 50;
            var platformWidth = 250;

            var leftPlatform = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - groundHeight),
                Bounds = new Vector2(platformWidth, groundHeight),
                Color = Color.Black,
            };

            var rightPlatform = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(mapSize.X - platformWidth, mapSize.Y - groundHeight),
                Bounds = new Vector2(platformWidth, groundHeight),
                Color = Color.Black,
            };

            var flyingPlatformWidth = 80;
            var flyingPlatform = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(mapSize.X / 2 - flyingPlatformWidth / 2f, 80),
                Bounds = new Vector2(flyingPlatformWidth, 50),
                Color = Color.Blue,
            };

            var player = LevelUtils.CreatePlayer();

            var goal = LevelUtils.CreateGoal(rightPlatform.Position + new Vector2(platformWidth / 2f - 25, -50));

            return [
                leftPlatform,
                rightPlatform,
                flyingPlatform,
                player,
                goal,
            ];
        }
    }
}
