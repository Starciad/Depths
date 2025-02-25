using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.Mathematics
{
    internal static class DPointMath
    {
        internal static float Distance(Point value1, Point value2)
        {
            float dx = value1.X - value2.X;
            float dy = value1.Y - value2.Y;

            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }
    }
}
