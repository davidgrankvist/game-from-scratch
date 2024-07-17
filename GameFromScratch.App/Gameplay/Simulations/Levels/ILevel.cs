using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal interface ILevel
    {
        public IEnumerable<Entity> Create();
    }
}
