using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Fps;
using System.Runtime.Versioning;
using Windows.Win32;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32Sleeper : ISleeper
    {
        private uint resolutionStatus;

        public Win32Sleeper()
        {
            resolutionStatus = PInvoke.TIMERR_NOCANDO;
        }

        public void Initialize(int resolutionMs)
        {
            // set resolution for Thread.Sleep calls
            resolutionStatus = PInvoke.timeBeginPeriod((uint)resolutionMs);
        }

        public void Sleep(int delayMs)
        {
            // if supported, use low resolution sleep
            if (resolutionStatus == PInvoke.TIMERR_NOERROR)
            {
                Thread.Sleep(delayMs);
                return;
            }

            // otherwise, fall back to busy-wait loop
            var start = DateTime.UtcNow.Ticks;
            while (DateTime.UtcNow.Ticks - start < delayMs * FpsConstants.MILLISECONDS_PER_TICK)
            {
            }
        }
    }
}
