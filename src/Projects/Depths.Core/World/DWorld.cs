using Depths.Core.Databases;
using Depths.Core.Enums.Tiles;
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
            this.Tilemap = new(assetDatabase, DTilemapMath.GetTotalTileCount(5, 5));

            this.Tilemap.SetTile(new(0, 0), DTileType.Ground);

            for (byte i = 0; i < 3; i++)
            {
                this.Tilemap.SetTile(new(i, 5), DTileType.Ground);
            }

            for (byte i = 0; i < 5; i++)
            {
                this.Tilemap.SetTile(new(i, 6), DTileType.Ground);
            }

            this.Tilemap.SetTile(new(11, 4), DTileType.Ground);
            this.Tilemap.SetTile(new(08, 19), DTileType.Ground);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.Tilemap.Draw(spriteBatch);
        }
    }
}
