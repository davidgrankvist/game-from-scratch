using GameFromScratch.App.Gameplay.Simulations.Entities;

namespace GameFromScratch.App.Gameplay.Simulations.Levels
{
    internal interface ILevel
    {
        public string Name { get; }
        public IEnumerable<Entity> Create();
    }
}
