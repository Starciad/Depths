using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTilemap
    {
        internal DSize2 Size => this.size;

        private readonly DSize2 size;
        private readonly DTile[,] tiles;

        private readonly DAssetDatabase assetDatabase;

        private readonly Dictionary<DTileType, Action<DTile>> tileTypes = new()
        {
            [DTileType.Empty] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 0;
                tile.IsSolid = false;
                tile.IsDestructible = false;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Ground] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 1;
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Stone] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = (byte)DRandomMath.Range(2, 3);
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Ore] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = (byte)DRandomMath.Range(4, 5);
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Stairs] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 0;
                tile.IsSolid = false;
                tile.IsDestructible = false;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Box] = (DTile tile) =>
            {
                tile.HasGravity = true;
                tile.Health = 2;
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.SpikeTrap] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 0;
                tile.IsSolid = false;
                tile.IsDestructible = false;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.ArrowTrap] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 1;
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.Wall] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 0;
                tile.IsSolid = true;
                tile.IsDestructible = false;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.BoulderTrap] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 5;
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },

            [DTileType.ExplosiveTrap] = (DTile tile) =>
            {
                tile.HasGravity = false;
                tile.Health = 1;
                tile.IsSolid = true;
                tile.IsDestructible = true;
                tile.Ore = null;
                tile.Resistance = 0;
            },
        };

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
                    Point position = new(x, y);
                    DTile tile = GetTile(position);

                    UpdateTileHealth(tile, position);
                    UpdateTileGravity(tile, position);
                }
            }
        }

        private void UpdateTileHealth(DTile tile, Point position)
        {
            if (tile.Health == 0 && tile.IsDestructible)
            {
                SetTile(position, DTileType.Empty);
            }
        }

        private void UpdateTileGravity(DTile tile, Point position)
        {
            if (!tile.HasGravity)
            {
                return;
            }

            DTile tileBelow = this.tiles[position.X, position.Y + 1];

            if (tileBelow == null)
            {
                return;
            }

            if (tileBelow.Type == DTileType.Empty)
            {
                tileBelow.Copy(tile);
                SetTile(position, DTileType.Empty);
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
            this.tileTypes[type]?.Invoke(tile);
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
                DTileType.Box => this.assetDatabase.GetTexture("texture_tile_5"),
                DTileType.SpikeTrap => this.assetDatabase.GetTexture("texture_tile_6"),
                DTileType.ArrowTrap => this.assetDatabase.GetTexture("texture_tile_7"),
                DTileType.Wall => this.assetDatabase.GetTexture("texture_tile_8"),
                DTileType.BoulderTrap => this.assetDatabase.GetTexture("texture_tile_9"),
                DTileType.ExplosiveTrap => this.assetDatabase.GetTexture("texture_tile_10"),
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
