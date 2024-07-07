using System.Diagnostics;

namespace GameFromScratch.App.Framework.Fps
{
    internal class FpsThrottler
    {
        private readonly double targetMsPerFrame;
        private readonly ISleeper sleeper;
        private readonly Stopwatch stopWatch;

        public FpsThrottler(int fps, ISleeper sleeper)
        {
            targetMsPerFrame = GetTargetMsPerFrame(fps);
            this.sleeper = sleeper;
            stopWatch = new Stopwatch();
        }

        private static double GetTargetMsPerFrame(int fps)
        {
            var targetTicksPerFrame = FpsConstants.TICKS_PER_SECOND / fps;
            return targetTicksPerFrame / FpsConstants.MILLISECONDS_PER_TICK;
        }

        public void SleepUntilNextFrame()
        {
            var sleepDelay = (int)(targetMsPerFrame - stopWatch.ElapsedMilliseconds);
            if (sleepDelay > 0)
            {
                sleeper.Sleep(sleepDelay);
            }

            stopWatch.Restart();
        }
    }
}
