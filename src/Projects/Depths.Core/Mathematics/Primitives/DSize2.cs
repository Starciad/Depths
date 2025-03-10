using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.Mathematics.Primitives
{
    internal struct DSize2 : IEquatable<DSize2>
    {
        internal static readonly DSize2 Empty = new();
        internal static readonly DSize2 Zero = new(0, 0);
        internal static readonly DSize2 One = new(1, 1);

        internal readonly bool IsEmpty => this.Width == 0 && this.Height == 0;
        internal readonly float AspectRatio => this.Height == 0 ? 0 : (float)this.Width / this.Height;

        internal int Width;
        internal int Height;

        internal DSize2(int value)
        {
            this.Width = value;
            this.Height = value;
        }
        internal DSize2(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator DSize2(DPoint value)
        {
            return new(value.X, value.Y);
        }

        public static implicit operator DPoint(DSize2 value)
        {
            return new(value.Width, value.Height);
        }

        public static DSize2 operator +(DSize2 a, DSize2 b)
        {
            return new(a.Width + b.Width, a.Height + b.Height);
        }

        public static DSize2 operator -(DSize2 a, DSize2 b)
        {
            return new(a.Width - b.Width, a.Height - b.Height);
        }

        public static DSize2 operator *(DSize2 a, int b)
        {
            return new(a.Width * b, a.Height * b);
        }

        public static DSize2 operator /(DSize2 a, int b)
        {
            return new(a.Width / b, a.Height / b);
        }

        public static bool operator ==(DSize2 a, DSize2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DSize2 a, DSize2 b)
        {
            return !a.Equals(b);
        }

        internal static DSize2 Add(DSize2 a, DSize2 b)
        {
            return new(a.Width + b.Width, a.Height + b.Height);
        }

        internal static DSize2 Subtract(DSize2 a, DSize2 b)
        {
            return new(a.Width - b.Width, a.Height - b.Height);
        }

        internal readonly DPoint ToPoint()
        {
            return new(this.Width, this.Height);
        }

        internal readonly Vector2 ToVector2()
        {
            return new(this.Width, this.Height);
        }

        public override readonly string ToString()
        {
            return $"{this.Width}x{this.Height}";
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Width, this.Height);
        }

        public override readonly bool Equals(object obj)
        {
            return obj is DSize2 other && Equals(other);
        }

        public readonly bool Equals(DSize2 other)
        {
            return this.Width == other.Width && this.Height == other.Height;
        }

        internal readonly DSize2 Abs()
        {
            return new(Math.Abs(this.Width), Math.Abs(this.Height));
        }

        internal static DSize2 Min(DSize2 a, DSize2 b)
        {
            return new(Math.Min(a.Width, b.Width), Math.Min(a.Height, b.Height));
        }

        internal static DSize2 Max(DSize2 a, DSize2 b)
        {
            return new(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
        }

        internal static DSize2 Clamp(DSize2 value, DSize2 min, DSize2 max)
        {
            return new DSize2(
                Math.Clamp(value.Width, min.Width, max.Width),
                Math.Clamp(value.Height, min.Height, max.Height)
            );
        }

        internal static DSize2 Lerp(DSize2 a, DSize2 b, float t)
        {
            return new DSize2(
                (int)MathF.Round(a.Width + ((b.Width - a.Width) * t)),
                (int)MathF.Round(a.Height + ((b.Height - a.Height) * t))
            );
        }
    }
}
