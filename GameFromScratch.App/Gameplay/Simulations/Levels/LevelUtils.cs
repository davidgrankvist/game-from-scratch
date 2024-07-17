﻿using GameFromScratch.App.Gameplay.Simulations.Entities;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal static class LevelUtils
    {
        // TODO(feature): consider screen resolution
        public static Vector2 MAP_SIZE = new Vector2(900, 400);

        public static Entity CreatePlayer()
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

        public static Entity CreateGoal(Vector2 position)
        {
            return new Entity
            {
                Flags = EntityFlags.Goal | EntityFlags.Solid | EntityFlags.Render,
                Position = position,
                Bounds = new Vector2(50, 50),
                Color = Color.Gold,
            };
        }
    }
}
