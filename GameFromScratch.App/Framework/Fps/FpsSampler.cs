namespace GameFromScratch.App.Framework.Fps
{
    internal class FpsSampler
    {
        private long[] ellapsedTimeSamples;
        private int iWindow;
        private float windowSum;
        private float averageFps;
        private long lastTick;

        public float Fps { get => averageFps; }

        public FpsSampler(int windowSize)
        {
            ellapsedTimeSamples = new long[windowSize];
            iWindow = 0;
            windowSum = 0;
            lastTick = DateTime.UtcNow.Ticks;
            averageFps = 0;
        }

        public void Sample()
        {
            // measure time
            var ellapsed = DateTime.UtcNow.Ticks - lastTick;
            lastTick = DateTime.UtcNow.Ticks;

            // update rolling sum
            ellapsedTimeSamples[iWindow] = ellapsed;
            var iOldestEllapsed = (iWindow + 1) % ellapsedTimeSamples.Length;
            var oldestEllapsed = ellapsedTimeSamples[iOldestEllapsed];
            windowSum = windowSum + ellapsed - oldestEllapsed;
            iWindow = (iWindow + 1) % ellapsedTimeSamples.Length;

            // convert to rolling mean of FPS
            var averageTicksPerFrame = windowSum / ellapsedTimeSamples.Length;
            var averageSecondsPerFrame = averageTicksPerFrame / FpsConstants.TICKS_PER_SECOND;
            averageFps = 1 / averageSecondsPerFrame;
        }
    }
}
