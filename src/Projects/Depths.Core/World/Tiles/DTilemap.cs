using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Tiles;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTilemap
    {
        private readonly DSize2 size;
        private readonly DTile[,] tiles;

        private readonly DAssetDatabase assetDatabase;

        internal DTilemap(DAssetDatabase assetDatabase, DSize2 size)
        {
            this.assetDatabase = assetDatabase;

            this.size = size;
            this.tiles = new DTile[size.Width, size.Height];

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    this.tiles[x, y] = new DTile();
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < this.size.Height; y++)
            {
                for (int x = 0; x < this.size.Width; x++)
                {
                    DTile tile = GetTile(new(x, y));
                    Texture2D texture = GetTileTexture(tile.Type);

                    if (texture == null)
                    {
                        continue;
                    }

                    spriteBatch.Draw(texture, new(x * DWorldConstants.TILE_SIZE, y * DWorldConstants.TILE_SIZE), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        internal void SetTile(Point position, DTileType type)
        {
            DTile tile = GetTile(position);
            tile.Type = type;

            switch (type)
            {
                case DTileType.Empty:
                    break;

                case DTileType.Ground:
                    tile.Health = 1;
                    break;

                case DTileType.Stone:
                    tile.Health = (byte)DRandomMath.Range(2, 3);
                    break;

                case DTileType.Ore:
                    tile.Health = (byte)DRandomMath.Range(4, 5);
                    break;

                default:
                    break;
            }
        }

        internal DTile GetTile(Point position)
        {
            if (!IsInsideBounds(position))
            {
                return null;
            }

            return this.tiles[position.X, position.Y];
        }

        internal Texture2D GetTileTexture(DTileType tileType)
        {
            return tileType switch
            {
                DTileType.Empty => null,
                DTileType.Ground => this.assetDatabase.GetTexture("texture_tile_1"),
                DTileType.Stone => this.assetDatabase.GetTexture("texture_tile_2"),
                DTileType.Ore => this.assetDatabase.GetTexture("texture_tile_3"),
                _ => null,
            };
        }

        internal bool IsInsideBounds(Point position)
        {
            return position.X >= 0 && position.X < this.size.Width &&
                   position.Y >= 0 && position.Y < this.size.Height;
        }
    }
}
