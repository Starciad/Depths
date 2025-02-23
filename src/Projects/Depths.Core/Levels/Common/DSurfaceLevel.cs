using Depths.Core.Databases;
using Depths.Core.Enums.Tiles;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Levels.Common
{
    internal sealed class DSurfaceLevel : DLevel
    {
        private readonly DTilemap tilemap;

        internal DSurfaceLevel(DAssetDatabase assetDatabase) : base(assetDatabase)
        {
            this.tilemap = new(assetDatabase, new(12, 7));
            this.tilemap.SetTile(new(0, 0), DTileType.Ground);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            this.tilemap.Draw(spriteBatch);
        }
    }
}
