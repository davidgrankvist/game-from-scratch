using GameFromScratch.App.Gameplay.Common.Entities;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Context
{
    internal class EntityRepository
    {

        // all entities
        private readonly List<Entity> entities;
        public IEnumerable<Entity> Entities { get => entities; }

        // player convenience accessor
        private readonly Entity dummyPlayer;
        private Entity player;
        public Entity Player { get => player; }

        public EntityRepository()
        {
            entities = new List<Entity>();
            dummyPlayer = new Entity
            {
                Flags = EntityFlags.Player,
            };
            player = dummyPlayer;
        }

        public void Add(Entity entity)
        {
            UpdatePlayerCache(entity, true);
            entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            UpdatePlayerCache(entity, false);
            entities.Remove(entity);
        }

        private void UpdatePlayerCache(Entity entity, bool add)
        {
            var isPlayer = entity.Flags.HasFlag(EntityFlags.Player);
            if (!isPlayer)
            {
                return;
            }
            player = add ? entity : dummyPlayer;
        }

        public void AddRange(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <summary>
        /// Query for entities where all specified flags are set. Optionally, skip entities matching any of the skip flags.
        /// </summary>
        public IEnumerable<Entity> Query(EntityFlags flags, EntityFlags skip = EntityFlags.None)
        {
            return entities.Where(
                entity =>
                    (entity.Flags & flags) == flags
                    && (entity.Flags & skip) == EntityFlags.None
            );
        }
    }
}
