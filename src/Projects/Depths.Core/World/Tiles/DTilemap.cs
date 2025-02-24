using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTilemap
    {
        internal DSize2 Size => this.size;

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

        internal void Update()
        {
            for (int y = 0; y < this.size.Height; y++)
            {
                for (int x = 0; x < this.size.Width; x++)
                {
                    DTile tile = GetTile(new(x, y));

                    if (tile.Health == 0 && tile.IsDestructible)
                    {
                        SetTile(new(x, y), DTileType.Empty);
                    }
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

            if (tile == null)
            {
                return;
            }

            tile.Type = type;

            switch (type)
            {
                case DTileType.Empty:
                    tile.Health = 0;
                    tile.IsSolid = false;
                    tile.IsDestructible = false;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Ground:
                    tile.Health = 1;
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Stone:
                    tile.Health = (byte)DRandomMath.Range(2, 3);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Ore:
                    tile.Health = (byte)DRandomMath.Range(4, 5);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Stairs:
                    tile.IsSolid = false;
                    tile.IsDestructible = false;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Trap:
                    tile.IsSolid = false;
                    tile.IsDestructible = false;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                case DTileType.Wall:
                    tile.IsSolid = true;
                    tile.IsDestructible = false;
                    tile.Ore = null;
                    tile.Resistance = 0;
                    break;

                default:
                    break;
            }
        }

        internal DTile GetTile(Point position)
        {
            return !IsInsideBounds(position) ? null : this.tiles[position.X, position.Y];
        }

        internal Texture2D GetTileTexture(DTileType tileType)
        {
            return tileType switch
            {
                DTileType.Empty => null,
                DTileType.Ground => this.assetDatabase.GetTexture("texture_tile_1"),
                DTileType.Stone => this.assetDatabase.GetTexture("texture_tile_2"),
                DTileType.Ore => this.assetDatabase.GetTexture("texture_tile_3"),
                DTileType.Stairs => this.assetDatabase.GetTexture("texture_tile_4"),
                DTileType.MovableBlock => this.assetDatabase.GetTexture("texture_tile_5"),
                DTileType.Trap => null,
                DTileType.Wall => this.assetDatabase.GetTexture("texture_tile_6"),
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
