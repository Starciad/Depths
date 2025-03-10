using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.Extensions;
using Depths.Core.Helpers;
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
    internal sealed class DTilemap : IDResettable
    {
        internal DSize2 Size => this.size;

        private DUpdateCycleFlag updateCycleFlag;
        private byte boulderTrapFrameCounter;

        private readonly byte boulderTrapFrameDelay = 5;
        private readonly int totalDust = 8;

        private readonly DSize2 size;
        private readonly DTile[,] tiles;

        private readonly Dictionary<DTileType, Action<DTile, DPoint>> tileTypes;

        private static readonly DBoxItem[] items = [
            new("Money", new(3, 6), (DBoxItem item, DPlayerEntity playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.Money += count;

                return count;
            }),

            new("Stair", new(5, 10), (DBoxItem item, DPlayerEntity playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.StairCount += count;

                return count;
            }),

            new("Platform", new(4, 8), (DBoxItem item, DPlayerEntity playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.PlataformCount += count;

                return count;
            }),

            new("Robot", new(1, 3), (DBoxItem item, DPlayerEntity playerEntity) =>
            {
                uint count = item.GetRandomCount();

                playerEntity.RobotCount += count;

                return count;
            }),
        ];

        private readonly DAssetDatabase assetDatabase;
        private readonly DEntityManager entityManager;

        internal DTilemap(DSize2 size, DAssetDatabase assetDatabase, DEntityManager entityManager, DGameInformation gameInformation)
        {
            this.assetDatabase = assetDatabase;
            this.entityManager = entityManager;
            this.size = size;
            this.tiles = new DTile[size.Width, size.Height];

            for (byte y = 0; y < size.Height; y++)
            {
                for (byte x = 0; x < size.Width; x++)
                {
                    this.tiles[x, y] = new DTile();
                }
            }

            void SetDefaults(DTile tile, DPoint position)
            {
                tile.Reset();
            }

            this.tileTypes = new Dictionary<DTileType, Action<DTile, DPoint>>
            {
                [DTileType.Empty] = SetDefaults,

                [DTileType.Dirt] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(1);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = DTileType.Dirt;
                },

                [DTileType.Stone] = (tile, position) =>
                {
                    int layer = (int)MathF.Floor(position.Y / DWorldConstants.TILES_PER_CHUNK_HEIGHT);

                    SetDefaults(tile, position);
                    tile.SetHealth(Convert.ToUInt32(DRandomMath.Range(3, 6) + layer));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Resistance = (byte)int.Max(0, layer - 1);
                    tile.Type = DTileType.Stone;
                },

                [DTileType.Ore] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(Convert.ToUInt32(DRandomMath.Range(4, 5) + MathF.Floor(position.Y / DWorldConstants.TILES_PER_CHUNK_HEIGHT)));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.OnDestroyed = () => gameInformation.PlayerEntity.CollectOre(tile.Ore);
                    tile.Type = DTileType.Ore;
                },

                [DTileType.Stair] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.Type = DTileType.Stair;
                },

                [DTileType.Box] = (tile, position) =>
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
                    tile.Type = DTileType.Box;
                },

                [DTileType.SpikeTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.Type = DTileType.SpikeTrap;
                },

                [DTileType.Wood] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.SetHealth(4);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = DTileType.Wood;
                },

                [DTileType.Wall] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.IsSolid = true;
                    tile.Type = DTileType.Wall;
                },

                [DTileType.BoulderTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = DRandomMath.Chance(50, 100) ? DDirection.Left : DDirection.Right;
                    tile.HasGravity = true;
                    tile.SetHealth(5);
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.Type = DTileType.BoulderTrap;
                },

                [DTileType.Platform] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Type = DTileType.Platform;
                },

                [DTileType.Ghost] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = DDirection.Left;
                    tile.IsSolid = true;
                    tile.Type = DTileType.Ghost;
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
                    DTile tile = GetTile(position);
                    if (tile == null || tile.UpdateCycleFlag == this.updateCycleFlag)
                    {
                        continue;
                    }

                    tile.UpdateCycleFlag = tile.UpdateCycleFlag.GetNextCycle();

                    UpdateTileGravity(tile, position);

                    switch (tile.Type)
                    {
                        case DTileType.SpikeTrap:
                            UpdateSpikeTrap(position);
                            break;

                        case DTileType.BoulderTrap:
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
            DTile tile = GetTile(position);

            if (!tile.TryDamage(value))
            {
                return;
            }

            if (tile.IsDestroyed)
            {
                SetTile(position, DTileType.Empty);
                InstantiateDust(DTilemapMath.ToGlobalPosition(position));
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

                _ = this.entityManager.InstantiateEntity("Dust", (DEntity entity) =>
                {
                    DDustEntity dustEntity = entity as DDustEntity;

                    dustEntity.Position = new(position.X + ((DSpriteConstants.TILE_SPRITE_SIZE - 4) / 2), position.Y + ((DSpriteConstants.TILE_SPRITE_SIZE - 4) / 2));
                    dustEntity.Velocity = velocity;
                });
            }
        }

        private void UpdateTileGravity(DTile tile, DPoint position)
        {
            if (!tile.HasGravity)
            {
                return;
            }

            DPoint below = new(position.X, position.Y + 1);
            DTile tileBelow = GetTile(below);
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

        private void UpdateBoulderTrap(DTile tile, DPoint position)
        {
            DTile bottomTile = GetTile(new(position.X, position.Y + 1));

            if (bottomTile != null && !bottomTile.IsSolid)
            {
                bottomTile.Copy(tile);
                SetTile(position, DTileType.Empty);
                return;
            }

            switch (tile.Direction)
            {
                case DDirection.Right:
                    DTile rightTile = GetTile(new(position.X + 1, position.Y));
                    if (rightTile != null && !rightTile.IsSolid)
                    {
                        rightTile.Copy(tile);
                        SetTile(position, DTileType.Empty);
                    }
                    else
                    {
                        tile.Direction = DDirection.Left;
                    }

                    break;

                case DDirection.Left:
                    DTile leftTile = GetTile(new(position.X - 1, position.Y));
                    if (leftTile != null && !leftTile.IsSolid)
                    {
                        leftTile.Copy(tile);
                        SetTile(position, DTileType.Empty);
                    }
                    else
                    {
                        tile.Direction = DDirection.Right;
                    }

                    break;
            }
        }

        private void UpdateSpikeTrap(DPoint position)
        {
            DTile tileBelow = this.tiles[position.X, position.Y + 1];

            if (tileBelow != null &&
                (tileBelow.Type == DTileType.Dirt ||
                tileBelow.Type == DTileType.Stone ||
                tileBelow.Type == DTileType.Ore ||
                tileBelow.Type == DTileType.Wall))
            {
                return;
            }

            SetTile(position, DTileType.Empty);
        }

        internal void Draw(SpriteBatch spriteBatch, DCameraManager cameraManager)
        {
            Vector2 topLeftWorld = cameraManager.ScreenToWorld(new Vector2(0, 0));
            Vector2 bottomRightWorld = cameraManager.ScreenToWorld(new Vector2(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT));

            int minTileX = (int)Math.Floor(topLeftWorld.X / DWorldConstants.TILE_SIZE);
            int minTileY = (int)Math.Floor(topLeftWorld.Y / DWorldConstants.TILE_SIZE);
            int maxTileX = (int)Math.Ceiling(bottomRightWorld.X / DWorldConstants.TILE_SIZE);
            int maxTileY = (int)Math.Ceiling(bottomRightWorld.Y / DWorldConstants.TILE_SIZE);

            minTileX = Math.Clamp(minTileX, 0, this.size.Width);
            minTileY = Math.Clamp(minTileY, 0, this.size.Height);
            maxTileX = Math.Clamp(maxTileX, 0, this.size.Width);
            maxTileY = Math.Clamp(maxTileY, 0, this.size.Height);

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    DPoint pos = new(x, y);
                    DTile tile = GetTile(pos);
                    Texture2D texture = GetTileTexture(tile.Type);
                    if (texture is null)
                    {
                        continue;
                    }
                    Vector2 drawPosition = new(x * DWorldConstants.TILE_SIZE, y * DWorldConstants.TILE_SIZE);
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

                    DTile tile = GetTile(pos);
                    Texture2D texture = GetTileTexture(tile.Type);

                    if (texture == null)
                    {
                        continue;
                    }

                    Vector2 drawPosition = new(x * DWorldConstants.TILE_SIZE, y * DWorldConstants.TILE_SIZE);
                    spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        internal void SetTile(DPoint position, DTileType type)
        {
            DTile tile = GetTile(position);
            if (tile == null)
            {
                return;
            }

            tile.Type = type;
            this.tileTypes[type]?.Invoke(tile, position);
        }

        internal DTile GetTile(DPoint position)
        {
            return !IsInsideBounds(position) ? null : this.tiles[position.X, position.Y];
        }

        internal Texture2D GetTileTexture(DTileType tileType)
        {
            return tileType switch
            {
                DTileType.Empty => null,
                DTileType.Dirt => this.assetDatabase.GetTexture("texture_tile_1"),
                DTileType.Stone => this.assetDatabase.GetTexture("texture_tile_2"),
                DTileType.Ore => this.assetDatabase.GetTexture("texture_tile_3"),
                DTileType.Stair => this.assetDatabase.GetTexture("texture_tile_4"),
                DTileType.Box => this.assetDatabase.GetTexture("texture_tile_5"),
                DTileType.SpikeTrap => this.assetDatabase.GetTexture("texture_tile_6"),
                DTileType.Wood => this.assetDatabase.GetTexture("texture_tile_7"),
                DTileType.Wall => this.assetDatabase.GetTexture("texture_tile_8"),
                DTileType.BoulderTrap => this.assetDatabase.GetTexture("texture_tile_9"),
                DTileType.Platform => this.assetDatabase.GetTexture("texture_tile_10"),
                DTileType.Ghost => this.assetDatabase.GetTexture("texture_tile_11"),
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
            this.updateCycleFlag = DUpdateCycleFlag.None;

            for (byte y = 0; y < this.size.Height; y++)
            {
                for (byte x = 0; x < this.size.Width; x++)
                {
                    SetTile(new(x, y), DTileType.Empty);
                }
            }
        }
    }
}
