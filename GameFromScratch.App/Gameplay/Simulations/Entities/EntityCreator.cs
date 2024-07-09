using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    internal class EntityCreator
    {
        public static Entity CreatePlayer()
        {
            return new Entity
            {
                Speed = 100,
                Bounds = new Vector2(50, 50),
                Color = Color.Blue,
            };
        }
    }
}
