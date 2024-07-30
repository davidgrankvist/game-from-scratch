namespace GameFromScratch.App.Framework.Maths
{
    internal static class MathExtensions
    {
        public static int Min(params int[] values)
        {
            int m = int.MaxValue;
            foreach (var v in values)
            {
                m = Math.Min(m, v);
            }
            return m;
        }

        public static int Max(params int[] values)
        {
            int m = int.MinValue;
            foreach (var v in values)
            {
                m = Math.Max(m, v);
            }
            return m;
        }

        public static float Lerp(float x, float y, float t)
        {
            return (1 - t) * x + t * y;
        }

        public static int Clamp(int x, int min, int max)
        {
            return Math.Min(Math.Max(x, min), max);
        }
    }
}
