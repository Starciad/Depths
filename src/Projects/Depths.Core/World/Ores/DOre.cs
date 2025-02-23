using Depths.Core.Enums.General;

using System;

namespace Depths.Core.World.Ores
{
    internal sealed class DOre
    {
        internal Range CaveSpawnLevel { get; init; }
        internal string DisplayName { get; init; }
        internal DRarity Rarity { get; init; }
        internal byte Value { get; init; }
    }
}
