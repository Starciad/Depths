using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Mathematics;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World
{
    internal sealed class DWorld
    {
        internal DTilemap Tilemap { get; private set; }

        internal DWorld(DAssetDatabase assetDatabase)
        {
            this.Tilemap = new(assetDatabase, DTilemapMath.GetTotalTileCount(DWorldConstants.WORLD_WIDTH, DWorldConstants.WORLD_HEIGHT));
        }

        internal void Update()
        {
            this.Tilemap.Update();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.Tilemap.Draw(spriteBatch);
        }
    }
}
