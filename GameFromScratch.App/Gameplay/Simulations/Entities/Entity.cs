using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    internal class Entity
    {
        // types and behaviors
        public EntityFlags Flags;

        // movement
        public Vector2 Position;
        public Vector2 Velocity;
        public float Speed;

        // movement - periodic
        public Vector2 MoveStart;
        public Vector2 MoveEnd;

        // body
        public Vector2 Bounds;

        // graphics
        public Color Color;
    }
}
