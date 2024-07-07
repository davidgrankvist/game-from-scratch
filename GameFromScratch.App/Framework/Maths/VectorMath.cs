using System.Numerics;

namespace GameFromScratch.App.Framework.Maths
{
    internal static class VectorMath
    {
        public static Vector2 RotatePoint(Vector2 point, float angle, Vector2 origin)
        {
            var sin = MathF.Sin(angle);
            var cos = MathF.Cos(angle);

            var translated = point - origin;
            var rotated = new Vector2(translated.X * cos - translated.Y * sin, translated.X * sin + translated.Y * cos);
            var result = rotated + origin;

            return result;
        }

        public static float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
