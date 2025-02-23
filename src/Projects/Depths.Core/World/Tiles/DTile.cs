using Depths.Core.Enums.Tiles;
using Depths.Core.World.Ores;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTile
    {
        internal byte Health { get; set; }
        internal DOre Ore { get; set; }
        internal byte Resistance { get; set; }
        internal DTileType Type { get; set; }
    }
}
