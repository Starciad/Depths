using Depths.Core.Enums.World.Tiles;
using Depths.Core.World.Ores;

using System;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTile
    {
        internal bool HasGravity { get; set; }
        internal byte Health { get; set; }
        internal bool IsDestructible { get; set; }
        internal bool IsSolid { get; set; }
        internal DOre Ore { get; set; }
        internal byte Resistance { get; set; }
        internal DTileType Type { get; set; }

        internal void Copy(DTile tile)
        {
            this.HasGravity = tile.HasGravity;
            this.Health = tile.Health;
            this.IsDestructible = tile.IsDestructible;
            this.IsSolid = tile.IsSolid;
            this.Ore = tile.Ore;
            this.Resistance = tile.Resistance;
            this.Type = tile.Type;
        }
    }
}
