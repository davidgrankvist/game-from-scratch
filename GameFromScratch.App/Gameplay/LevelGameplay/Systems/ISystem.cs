using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal interface ISystem
    {
        public void Initialize(GameContext context);

        public void Update(GameContext context);
    }
}
