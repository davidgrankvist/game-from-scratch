namespace GameFromScratch.App
{
    internal class GameConfiguration
    {
        public readonly bool DebugMode;

        private GameConfiguration(bool debugMode)
        {
            DebugMode = debugMode;
        }

        // load config files, environment variables, etc. here
        public static GameConfiguration Load()
        {
            var debugMode = LoadDebugMode();
            return new GameConfiguration(debugMode);
        }

        private static bool LoadDebugMode()
        {
            // TODO(investigate): Decide how this should be set. Should it change during runtime? Build flag is a simple way for now.
#if CUSTOM_DEBUG
            return true;
#else
            return false;
#endif
        }

    }
}
