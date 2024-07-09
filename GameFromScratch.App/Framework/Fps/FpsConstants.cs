namespace GameFromScratch.App.Framework
{
    internal static class FpsConstants
    {
        // 1 tick = 100 ns = ten millionth of a second
        public const int TICKS_PER_SECOND = 10_000_000;
        // 1 tick = 100 ns = 1 / 10 000 ms
        public const int TICKS_PER_MILLISECOND = 10_000;
    }
}
