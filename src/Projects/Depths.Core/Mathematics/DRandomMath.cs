using System;

namespace Depths.Core.Mathematics
{
    internal static class DRandomMath
    {
        private static readonly Random _random = new();

        internal static int Range(int max)
        {
            return _random.Next(max + 1);
        }

        internal static int Range(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        internal static bool Chance(int chance)
        {
            return Chance(chance, 100);
        }

        internal static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }
    }
}
