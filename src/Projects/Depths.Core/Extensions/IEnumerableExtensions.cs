using Depths.Core.Mathematics;

using System;
using System.Collections.Generic;

namespace Depths.Core.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static T GetRandomItem<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentException("The collection cannot be null.");
            }

            using IEnumerator<T> enumerator = enumerable.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                throw new ArgumentException("The collection cannot be empty.");
            }

            T current = enumerator.Current;
            int index = 1;

            while (enumerator.MoveNext())
            {
                if (RandomMath.Range(index + 1) == 0)
                {
                    current = enumerator.Current;
                }

                index++;
            }

            return current;
        }
    }
}
