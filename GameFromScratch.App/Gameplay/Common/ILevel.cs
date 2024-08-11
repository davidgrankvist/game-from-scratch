using GameFromScratch.App.Gameplay.Common.Entities;

namespace GameFromScratch.App.Gameplay.Common
{
    internal interface ILevel
    {
        public string Name { get; }
        public IEnumerable<Entity> Create();
    }
}
