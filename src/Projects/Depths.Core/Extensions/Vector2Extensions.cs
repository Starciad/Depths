using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

namespace Depths.Core.Extensions
{
    internal static class Vector2Extensions
    {
        internal static DPoint ToDPoint(this Vector2 value)
        {
            return new((int)value.X, (int)value.Y);
        }
    }
}
