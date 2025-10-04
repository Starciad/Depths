using Depths.Core.Entities.Common;
using Depths.Core.Mathematics;

using System;

namespace Depths.Core.Items
{
    internal sealed class BoxItem(string name, Range amountRange, Func<BoxItem, PlayerEntity, uint> onCollectionCallback)
    {
        internal string Name => name;
        internal Range AmountRange => amountRange;
        internal Func<BoxItem, PlayerEntity, uint> OnCollectionCallback => onCollectionCallback;

        internal uint GetRandomCount()
        {
            return (uint)RandomMath.Range(this.AmountRange.Start.Value, this.AmountRange.End.Value);
        }
    }
}
