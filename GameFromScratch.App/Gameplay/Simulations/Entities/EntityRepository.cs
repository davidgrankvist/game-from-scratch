namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    internal class EntityRepository
    {
        public readonly Entity Player;
        private readonly List<Entity> entities;
        public IEnumerable<Entity> Entities { get => entities; }
        public IEnumerable<Entity> NonPlayerEntities { get => entities.Skip(1); }

        public EntityRepository()
        {
            Player = EntityCreator.CreatePlayer();
            entities = new List<Entity>() { Player };
        }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }

        public void AddRange(IEnumerable<Entity> entities)
        {
            this.entities.AddRange(entities);
        }

        /// <summary>
        /// Query for entities where all specified flags are set.
        /// </summary>
        public IEnumerable<Entity> Query(EntityFlags flags)
        {
            return entities.Where(entity => (entity.Flags & flags) == flags);
        }
    }
}
