using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.Mathematics.Primitives
{
    internal struct DPoint : IEquatable<DPoint>
    {
        internal static readonly DPoint Origin = new(0);
        internal static readonly DPoint Up = new(0, -1);
        internal static readonly DPoint Down = new(0, 1);
        internal static readonly DPoint Left = new(-1, 0);
        internal static readonly DPoint Right = new(1, 0);

        public int X;
        public int Y;

        public DPoint()
        {
            this.X = 0;
            this.Y = 0;
        }
        public DPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public DPoint(int value)
        {
            this.X = value;
            this.Y = value;
        }

        public static DPoint operator +(DPoint a, DPoint b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public static DPoint operator -(DPoint a, DPoint b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public static DPoint operator *(DPoint a, DPoint b)
        {
            return new(a.X * b.X, a.Y * b.Y);
        }

        public static DPoint operator /(DPoint a, DPoint b)
        {
            return new(a.X / b.X, a.Y / b.Y);
        }

        public static DPoint operator +(DPoint a, int b)
        {
            return new(a.X + b, a.Y + b);
        }

        public static DPoint operator -(DPoint a, int b)
        {
            return new(a.X - b, a.Y - b);
        }

        public static DPoint operator *(DPoint a, int b)
        {
            return new(a.X * b, a.Y * b);
        }

        public static DPoint operator /(DPoint a, int b)
        {
            return new(a.X / b, a.Y / b);
        }

        public static bool operator ==(DPoint a, DPoint b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DPoint a, DPoint b)
        {
            return !a.Equals(b);
        }

        internal readonly DSize2 ToSize2()
        {
            return new(this.X, this.Y);
        }

        internal readonly Point ToPoint()
        {
            return new(this.X, this.Y);
        }

        internal readonly Vector2 ToVector2()
        {
            return new(this.X, this.Y);
        }

        public override readonly string ToString()
        {
            return $"{{X:{this.X} Y:{this.Y}}}";
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }

        public override readonly bool Equals(object obj)
        {
            return obj is DPoint point && Equals(point);
        }

        public readonly bool Equals(DPoint other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public readonly float DistanceTo(DPoint other)
        {
            int dx = this.X - other.X;
            int dy = this.Y - other.Y;
            return MathF.Sqrt((dx * dx) + (dy * dy));
        }

        public readonly int ManhattanDistanceTo(DPoint other)
        {
            return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
        }

        public readonly DPoint Abs()
        {
            return new DPoint(Math.Abs(this.X), Math.Abs(this.Y));
        }

        public static DPoint Min(DPoint a, DPoint b)
        {
            return new DPoint(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }

        public static DPoint Max(DPoint a, DPoint b)
        {
            return new DPoint(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        public static DPoint Clamp(DPoint value, DPoint min, DPoint max)
        {
            return new DPoint(
                Math.Clamp(value.X, min.X, max.X),
                Math.Clamp(value.Y, min.Y, max.Y)
            );
        }

        public static DPoint Lerp(DPoint a, DPoint b, float t)
        {
            return new DPoint(
                (int)MathF.Round(a.X + ((b.X - a.X) * t)),
                (int)MathF.Round(a.Y + ((b.Y - a.Y) * t))
            );
        }
    }
}
