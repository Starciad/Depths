using Depths.Core.Enums.World.Tiles;
using Depths.Core.World.Ores;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTile
    {
        internal byte Health { get; set; }
        internal bool IsDestructible { get; set; }
        internal bool IsSolid { get; set; }
        internal DOre Ore { get; set; }
        internal byte Resistance { get; set; }
        internal DTileType Type { get; set; }
    }
}
