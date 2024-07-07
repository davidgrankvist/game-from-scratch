using System.Numerics;

namespace GameFromScratch.App.Framework.Maths
{
    public readonly struct Vector2Int
    {
        public readonly int X;
        public readonly int Y;

        public Vector2Int()
        {
            X = 0;
            Y = 0;
        }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X - right.X, left.Y - right.Y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
