using Depths.Core.Enums.General;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.World.Ores
{
    internal sealed class DOre
    {
        internal Texture2D IconTexture { get; init; }
        internal Range LayerRange { get; init; }
        internal string Name { get; init; }
        internal DRarity Rarity { get; init; }
        internal byte Resistance { get; init; }
        internal byte Value { get; init; }
    }
}
