using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.Extensions;
using Depths.Core.Interfaces.General;
using Depths.Core.Items;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Depths.Core.World.Tiles
{
    internal sealed class Tilemap : IResettable
    {
        internal DSize2 Size => this.size;

        private UpdateCycleFlag updateCycleFlag;
        private byte boulderTrapFrameCounter;

        private readonly byte boulderTrapFrameDelay = 5;
        private readonly int totalDust = 8;

        private readonly DSize2 size;
        private readonly Tile[,] tiles;

        private readonly Dictionary<TileType, Action<Tile, DPoint>> tileTypes;

        private static readonly BoxItem[] items = [
            new("Money", new(3, 12), (item, playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.Money += count;

                return count;
            }),

            new("Stair", new(5, 10), (item, playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.StairCount += count;

                return count;
            }),

            new("Platform", new(4, 8), (item, playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.PlataformCount += count;

                return count;
            }),

            new("Robot", new(1, 3), (item, playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.RobotCount += count;

                return count;
            }),
        ];

        private readonly AssetDatabase assetDatabase;
        private readonly EntityManager entityManager;

        internal Tilemap(DSize2 size, AssetDatabase assetDatabase, EntityManager entityManager, GameInformation gameInformation)
        {
            this.assetDatabase = assetDatabase;
            this.entityManager = entityManager;
            this.size = size;
            this.tiles = new Tile[size.Width, size.Height];

            for (byte y = 0; y < size.Height; y++)
            {
                for (byte x = 0; x < size.Width; x++)
                {
                    this.tiles[x, y] = new Tile();
                }
            }

            void SetDefaults(Tile tile, DPoint position)
            {
                tile.Reset();
            }

            this.tileTypes = new Dictionary<TileType, Action<Tile, DPoint>>
            {
                [TileType.Empty] = SetDefaults,

                [TileType.Dirt] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(1);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = TileType.Dirt;
                },

                [TileType.Stone] = (tile, position) =>
                {
                    int layer = (int)MathF.Floor(position.Y / WorldConstants.TILES_PER_CHUNK_HEIGHT);

                    SetDefaults(tile, position);
                    tile.SetHealth(Convert.ToUInt32(RandomMath.Range(2, 3) + layer));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Resistance = (byte)int.Max(0, layer - 1);
                    tile.Type = TileType.Stone;
                },

                [TileType.Ore] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(Convert.ToUInt32(RandomMath.Range(4, 5) + MathF.Floor(position.Y / WorldConstants.TILES_PER_CHUNK_HEIGHT)));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.OnDestroyed = () => gameInformation.PlayerEntity.CollectOre(tile.Ore);
                    tile.Type = TileType.Ore;
                },

                [TileType.Stair] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.Type = TileType.Stair;
                },

                [TileType.Box] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.SetHealth(2);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.OnDestroyed = () =>
                    {
                        gameInformation.PlayerEntity.CollectBoxItem(items.GetRandomItem());
                    };
                    tile.Type = TileType.Box;
                },

                [TileType.SpikeTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.Type = TileType.SpikeTrap;
                },

                [TileType.Wood] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(4);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = TileType.Wood;
                },

                [TileType.Wall] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.IsSolid = true;
                    tile.Type = TileType.Wall;
                },

                [TileType.BoulderTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = RandomMath.Chance(50, 100) ? Direction.Left : Direction.Right;
                    tile.HasGravity = true;
                    tile.SetHealth(5);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = TileType.BoulderTrap;
                },

                [TileType.Platform] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Type = TileType.Platform;
                },

                [TileType.Ghost] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = Direction.Left;
                    tile.IsSolid = true;
                    tile.Type = TileType.Ghost;
                },
            };
        }

        internal void Update()
        {
            this.boulderTrapFrameCounter++;

            for (byte y = 0; y < this.size.Height; y++)
            {
                for (byte x = 0; x < this.size.Width; x++)
                {
                    DPoint position = new(x, y);
                    Tile tile = GetTile(position);
                    if (tile == null || tile.UpdateCycleFlag == this.updateCycleFlag)
                    {
                        continue;
                    }

                    tile.UpdateCycleFlag = tile.UpdateCycleFlag.GetNextCycle();

                    UpdateTileGravity(tile, position);

                    switch (tile.Type)
                    {
                        case TileType.SpikeTrap:
                            UpdateSpikeTrap(position);
                            break;

                        case TileType.BoulderTrap:
                            if (this.boulderTrapFrameCounter > this.boulderTrapFrameDelay)
                            {
                                UpdateBoulderTrap(tile, position);
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            this.boulderTrapFrameCounter = Convert.ToByte(this.boulderTrapFrameCounter > this.boulderTrapFrameDelay ? 0 : this.boulderTrapFrameCounter);
            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
        }

        internal void DamageTile(DPoint position, uint value)
        {
            Tile tile = GetTile(position);

            if (!tile.TryDamage(value))
            {
                return;
            }

            if (tile.IsDestroyed)
            {
                SetTile(position, TileType.Empty);
                InstantiateDust(TilemapMath.ToGlobalPosition(position));
            }
        }

        private void InstantiateDust(DPoint position)
        {
            float angleIncrement = MathHelper.TwoPi / this.totalDust;
            const float initialSpeed = 2f;

            for (int i = 0; i < this.totalDust; i++)
            {
                float angle = i * angleIncrement;
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * initialSpeed;

                _ = this.entityManager.InstantiateEntity("Dust", entity => { DustEntity dustEntity = entity as DustEntity; dustEntity.Position = new(position.X + ((SpriteConstants.TILE_SPRITE_SIZE - 4) / 2), position.Y + ((SpriteConstants.TILE_SPRITE_SIZE - 4) / 2)); dustEntity.Velocity = velocity; });
            }
        }

        private void UpdateTileGravity(Tile tile, DPoint position)
        {
            if (!tile.HasGravity)
            {
                return;
            }

            DPoint below = new(position.X, position.Y + 1);
            Tile tileBelow = GetTile(below);
            if (tileBelow == null)
            {
                return;
            }

            if (tileBelow.Type == TileType.Empty)
            {
                tileBelow.Copy(tile);
                SetTile(position, TileType.Empty);
            }
        }

        private void UpdateBoulderTrap(Tile tile, DPoint position)
        {
            Tile bottomTile = GetTile(new(position.X, position.Y + 1));

            if (bottomTile != null && !bottomTile.IsSolid)
            {
                bottomTile.Copy(tile);
                SetTile(position, TileType.Empty);
                return;
            }

            switch (tile.Direction)
            {
                case Direction.Right:
                    Tile rightTile = GetTile(new(position.X + 1, position.Y));
                    if (rightTile != null && !rightTile.IsSolid)
                    {
                        rightTile.Copy(tile);
                        SetTile(position, TileType.Empty);
                    }
                    else
                    {
                        tile.Direction = Direction.Left;
                    }

                    break;

                case Direction.Left:
                    Tile leftTile = GetTile(new(position.X - 1, position.Y));
                    if (leftTile != null && !leftTile.IsSolid)
                    {
                        leftTile.Copy(tile);
                        SetTile(position, TileType.Empty);
                    }
                    else
                    {
                        tile.Direction = Direction.Right;
                    }

                    break;
            }
        }

        private void UpdateSpikeTrap(DPoint position)
        {
            Tile tileBelow = this.tiles[position.X, position.Y + 1];

            if (tileBelow != null &&
                (tileBelow.Type == TileType.Dirt ||
                tileBelow.Type == TileType.Stone ||
                tileBelow.Type == TileType.Ore ||
                tileBelow.Type == TileType.Wall))
            {
                return;
            }

            SetTile(position, TileType.Empty);
        }

        internal void Draw(SpriteBatch spriteBatch, CameraManager cameraManager)
        {
            Vector2 topLeftWorld = cameraManager.ScreenToWorld(new Vector2(0, 0));
            Vector2 bottomRightWorld = cameraManager.ScreenToWorld(new Vector2(ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT));

            int minTileX = (int)Math.Floor(topLeftWorld.X / WorldConstants.TILE_SIZE);
            int minTileY = (int)Math.Floor(topLeftWorld.Y / WorldConstants.TILE_SIZE);
            int maxTileX = (int)Math.Ceiling(bottomRightWorld.X / WorldConstants.TILE_SIZE);
            int maxTileY = (int)Math.Ceiling(bottomRightWorld.Y / WorldConstants.TILE_SIZE);

            minTileX = Math.Clamp(minTileX, 0, this.size.Width);
            minTileY = Math.Clamp(minTileY, 0, this.size.Height);
            maxTileX = Math.Clamp(maxTileX, 0, this.size.Width);
            maxTileY = Math.Clamp(maxTileY, 0, this.size.Height);

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    DPoint pos = new(x, y);
                    Tile tile = GetTile(pos);
                    Texture2D texture = GetTileTexture(tile.Type);
                    if (texture is null)
                    {
                        continue;
                    }

                    Vector2 drawPosition = new(x * WorldConstants.TILE_SIZE, y * WorldConstants.TILE_SIZE);
                    spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        internal void DrawAll(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < this.size.Height; y++)
            {
                for (int x = 0; x < this.size.Width; x++)
                {
                    DPoint pos = new(x, y);

                    Tile tile = GetTile(pos);
                    Texture2D texture = GetTileTexture(tile.Type);

                    if (texture == null)
                    {
                        continue;
                    }

                    Vector2 drawPosition = new(x * WorldConstants.TILE_SIZE, y * WorldConstants.TILE_SIZE);
                    spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        internal void SetTile(DPoint position, TileType type)
        {
            Tile tile = GetTile(position);
            if (tile == null)
            {
                return;
            }

            tile.Type = type;
            this.tileTypes[type]?.Invoke(tile, position);
        }

        internal Tile GetTile(DPoint position)
        {
            return !IsInsideBounds(position) ? null : this.tiles[position.X, position.Y];
        }

        internal Texture2D GetTileTexture(TileType tileType)
        {
            return tileType switch
            {
                TileType.Empty => null,
                TileType.Dirt => this.assetDatabase.GetTexture("texture_tile_1"),
                TileType.Stone => this.assetDatabase.GetTexture("texture_tile_2"),
                TileType.Ore => this.assetDatabase.GetTexture("texture_tile_3"),
                TileType.Stair => this.assetDatabase.GetTexture("texture_tile_4"),
                TileType.Box => this.assetDatabase.GetTexture("texture_tile_5"),
                TileType.SpikeTrap => this.assetDatabase.GetTexture("texture_tile_6"),
                TileType.Wood => this.assetDatabase.GetTexture("texture_tile_7"),
                TileType.Wall => this.assetDatabase.GetTexture("texture_tile_8"),
                TileType.BoulderTrap => this.assetDatabase.GetTexture("texture_tile_9"),
                TileType.Platform => this.assetDatabase.GetTexture("texture_tile_10"),
                TileType.Ghost => this.assetDatabase.GetTexture("texture_tile_11"),
                _ => null,
            };
        }

        internal bool IsInsideBounds(DPoint position)
        {
            return position.X >= 0 && position.X < this.size.Width &&
            position.Y >= 0 && position.Y < this.size.Height;
        }

        public void Reset()
        {
            this.boulderTrapFrameCounter = 0;
            this.updateCycleFlag = UpdateCycleFlag.None;

            for (byte y = 0; y < this.size.Height; y++)
            {
                for (byte x = 0; x < this.size.Width; x++)
                {
                    SetTile(new(x, y), TileType.Empty);
                }
            }
        }
    }
}
