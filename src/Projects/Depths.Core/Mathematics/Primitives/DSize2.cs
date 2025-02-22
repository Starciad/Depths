using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.Mathematics.Primitives
{
    [Serializable]
    internal struct DSize2 : IEquatable<DSize2>
    {
        internal static readonly DSize2 Empty = new();
        internal readonly bool IsEmpty => this.Width == 0 && this.Height == 0;

        internal static DSize2 Zero => new(0, 0);
        internal static DSize2 One => new(1, 1);

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

        public static implicit operator DSize2(Point value)
        {
            return new DSize2(value.X, value.Y);
        }
        public static implicit operator Point(DSize2 value)
        {
            return new Point(value.Width, value.Height);
        }

        public static DSize2 operator +(DSize2 first, DSize2 second)
        {
            return Add(first, second);
        }
        public static DSize2 operator -(DSize2 first, DSize2 second)
        {
            return Subtract(first, second);
        }
        public static DSize2 operator *(DSize2 size, int value)
        {
            return new DSize2(size.Width * value, size.Height * value);
        }
        public static DSize2 operator /(DSize2 size, int value)
        {
            return new DSize2(size.Width / value, size.Height / value);
        }

        public static bool operator ==(DSize2 first, DSize2 second)
        {
            return first.Equals(ref second);
        }
        public static bool operator !=(DSize2 first, DSize2 second)
        {
            return !(first == second);
        }

        internal static DSize2 Add(DSize2 first, DSize2 second)
        {
            DSize2 size;
            size.Width = first.Width + second.Width;
            size.Height = first.Height + second.Height;
            return size;
        }
        internal static DSize2 Subtract(DSize2 first, DSize2 second)
        {
            DSize2 size;
            size.Width = first.Width - second.Width;
            size.Height = first.Height - second.Height;
            return size;
        }

        internal readonly Point ToPoint()
        {
            return new Point(this.Width, this.Height);
        }
        internal readonly Vector2 ToVector2()
        {
            return new Vector2(this.Width, this.Height);
        }

        public override readonly string ToString()
        {
            return string.Concat(this.Width, 'x', this.Height);
        }

        public override readonly int GetHashCode()
        {
            unchecked
            {
                return (this.Width.GetHashCode() * 397) ^ this.Height.GetHashCode();
            }
        }

        internal readonly bool Equals(DSize2 size)
        {
            return Equals(ref size);
        }
        internal readonly bool Equals(ref DSize2 size)
        {
            return this.Width == size.Width && this.Height == size.Height;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is DSize2 @int && Equals(@int);
        }

        bool IEquatable<DSize2>.Equals(DSize2 other)
        {
            throw new NotImplementedException();
        }
    }
}