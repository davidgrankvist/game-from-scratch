namespace GameFromScratch.App.Gameplay.LevelGameplay.Context
{
    internal class GameState
    {
        public readonly EntityRepository Repository;
        public float DeltaTime;
        public float GravitySign;
        public bool CompletedLevel;

        public GameState()
        {
            Repository = new EntityRepository();
            DeltaTime = 0;
            GravitySign = 1;
            CompletedLevel = false;
        }
    }
}
