namespace GameFromScratch.App.Framework.Fps
{
    internal class FpsThrottler
    {
        private const int DEFAULT_FPS = 60;

        private long lastTick;
        private readonly double targetTicksPerFrame;

        public FpsThrottler(int fps)
        {
            targetTicksPerFrame = GetTargetTicksPerFrame(fps);
            lastTick = DateTime.MinValue.Ticks;
        }

        public FpsThrottler() : this(DEFAULT_FPS)
        {
        }

        private static double GetTargetTicksPerFrame(int fps)
        {
            return FpsConstants.TICKS_PER_SECOND / fps;
        }

        public bool PollIsReady()
        {
            var ellapsed = DateTime.UtcNow.Ticks - lastTick;
            if (ellapsed < targetTicksPerFrame)
            {
                return false;
            }

            lastTick = DateTime.UtcNow.Ticks;
            return true;
        }
    }
}
