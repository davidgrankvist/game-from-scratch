using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal class MapInverterDeviceDemoLevel : ILevel
    {
        public string Name => "Map inverter demo";

        public IEnumerable<Entity> Create()
        {
            var mapSize = LevelUtils.MAP_SIZE;

            var entities = new List<Entity>();

            var platformHeight = 50;
            var numPlatforms = 3f;
            var platformWidth = mapSize.X / numPlatforms;
            for (int i = 0; i < numPlatforms; i++)
            {
                var isActive = i % 2 == 0;

                var flags = isActive ?
                    EntityFlags.Invert | EntityFlags.Solid | EntityFlags.Render
                    : EntityFlags.Invert;

                var platform = new Entity
                {
                    Flags = flags,
                    Position = new Vector2(platformWidth * i, mapSize.Y - platformHeight),
                    Bounds = new Vector2(platformWidth, platformHeight),
                    Color = Color.Black,
                };

                entities.Add(platform);
            }

            var player = LevelUtils.CreatePlayer();
            player.Position -= new Vector2(100, 0);
            entities.Add(player);

            var goal = LevelUtils.CreateGoal(new Vector2(mapSize.X - 50, mapSize.Y - platformHeight - 50));
            entities.Add(goal);

            return entities;
        }
    }
}
