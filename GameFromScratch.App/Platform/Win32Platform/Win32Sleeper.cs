using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using System.Diagnostics;
using System.Runtime.Versioning;
using Windows.Win32;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32Sleeper : ISleeper
    {
        private uint resolutionStatus;

        /*
         * The Windows sleep doesn't guarantee millisecond precision.
         *
         * Setting the sleep resolution to 1 ms helps, but the actual delay depends on
         * how soon the thread is scheduled to run again. We can correct for this by combining
         * with busy-wait loops that have higher precision (but use more CPU time).
         */
        private readonly Stopwatch sleepCorrectionTimer;
        private const int sleepPredictedErrorMs = 1; // TODO(hack): Handwaved error based on some basic experiments

        public Win32Sleeper()
        {
            resolutionStatus = PInvoke.TIMERR_NOCANDO;
            sleepCorrectionTimer = new Stopwatch();
        }

        public void Initialize(int resolutionMs)
        {
            // set resolution for Thread.Sleep calls
            resolutionStatus = PInvoke.timeBeginPeriod((uint)resolutionMs);
        }

        public void Sleep(int delayMs)
        {
            var targetTicks = delayMs * FpsConstants.TICKS_PER_MILLISECOND;

            // if supported, use low resolution sleep
            if (resolutionStatus == PInvoke.TIMERR_NOERROR)
            {
                sleepCorrectionTimer.Restart();

                // assume that Thread.Sleep usually sleeps for too long and use busy-wait for the remainder
                Thread.Sleep(delayMs - sleepPredictedErrorMs);
                BusyWait(targetTicks - sleepCorrectionTimer.ElapsedTicks);

                return;
            }

            // otherwise, fall back to busy-wait loop
            BusyWait(targetTicks);
        }

        private static void BusyWait(long ticks)
        {
            var start = DateTime.UtcNow.Ticks;
            while (DateTime.UtcNow.Ticks - start < ticks)
            {
            }
        }
    }
}
