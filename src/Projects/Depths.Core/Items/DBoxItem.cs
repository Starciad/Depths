using Depths.Core.Entities.Common;
using Depths.Core.Mathematics;

using System;

namespace Depths.Core.Items
{
    internal sealed class DBoxItem(string name, Range amountRange, Func<DBoxItem, DPlayerEntity, uint> onCollectionCallback)
    {
        internal string Name => name;
        internal Range AmountRange => amountRange;
        internal Func<DBoxItem, DPlayerEntity, uint> OnCollectionCallback => onCollectionCallback;

        internal uint GetRandomCount()
        {
            return (uint)DRandomMath.Range(this.AmountRange.Start.Value, this.AmountRange.End.Value);
        }
    }
}
