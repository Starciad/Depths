using Depths.Core.Enums.General;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.World.Ores
{
    internal sealed class DOre
    {
        internal required Texture2D IconTexture { get; init; }
        internal required Range LayerRange { get; init; }
        internal required string Name { get; init; }
        internal required DRarity Rarity { get; init; }
        internal required byte Resistance { get; init; }
        internal required byte Value { get; init; }
    }
}
