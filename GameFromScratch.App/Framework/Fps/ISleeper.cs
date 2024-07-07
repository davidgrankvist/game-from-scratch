namespace GameFromScratch.App.Framework.Fps
{
    /// <summary>
    /// The duration of Thread.Sleep depends on the OS scheduling, which may cause it to sleep for too long.
    /// The purpose of this interface is to support millisecond precision by using platform specific sleep functions.
    /// </summary>
    internal interface ISleeper
    {
        public void Sleep(int delayMs);
    }
}
