using GameFromScratch.App.Gameplay.Common;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Context
{
    internal class GameContext
    {
        public readonly GameTools Tools;
        public readonly GameState State;

        public GameContext(GameTools tools)
        {
            Tools = tools;
            State = new GameState();
        }
    }
}
