﻿using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    internal static class EntityCreator
    {
        public static Entity CreatePlayer()
        {
            return new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render | EntityFlags.Move,
                Bounds = new Vector2(50, 50),
                Color = Color.Blue,
            };
        }

        public static IEnumerable<Entity> CreateMap(Vector2 mapSize)
        {
            // map
            var ground = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(0, mapSize.Y - 50),
                Bounds = new Vector2(mapSize.X, 50),
                Color = Color.Black,
            };
            var tower = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render,
                Position = new Vector2(ground.Position.X + ground.Bounds.X - 80, ground.Position.Y - 100),
                Bounds = new Vector2(80, 100),
                Color = Color.Green,
            };
            var elevator = new Entity
            {
                Flags = EntityFlags.Solid | EntityFlags.Render | EntityFlags.Move,
                Speed = 100,
                Position = new Vector2(tower.Position.X - 100, tower.Position.Y + 50),
                Bounds = new Vector2(70, 25),
                Color = Color.Red,
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
                ground,
                tower,
                elevator,
                invisibleBorderLeft,
                invisibleBorderRight,
                invisibleBorderTop
            ];
        }
    }
}
