using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.Helpers;
using Depths.Core.Interfaces.General;
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
        private byte boulderTrapFrameCounter = 0;
        private byte monsterMovementFrameCounter = 0;
        private byte ghostMovementFrameCounter = 0;

        private readonly byte boulderTrapFrameDelay = 5;
        private readonly byte monsterMovementFrameDelay = 4;
        private readonly byte ghostMovementFrameDelay = 4;

        private readonly DSize2 size;
        private readonly DTile[,] tiles;
        private readonly DAssetDatabase assetDatabase;
        private readonly Dictionary<DTileType, Action<DTile, DPoint>> tileTypes;

        internal DTilemap(DSize2 size, DAssetDatabase assetDatabase, DGameInformation gameInformation)
        {
            this.assetDatabase = assetDatabase;
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
                tile.Direction = DDirection.None;
                tile.HasGravity = false;
                tile.Health = 0;
                tile.IsSolid = false;
                tile.IsDestructible = false;
                tile.Ore = null;
                tile.Resistance = 0;
                tile.OnDestroyed = null;
            }

            this.tileTypes = new Dictionary<DTileType, Action<DTile, DPoint>>
            {
                [DTileType.Empty] = SetDefaults,

                [DTileType.Dirt] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Health = 1;
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                },

                [DTileType.Stone] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Health = Convert.ToInt32(DRandomMath.Range(2, 3) + MathF.Floor(position.Y / DWorldConstants.TILES_PER_CHUNK_HEIGHT));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                },

                [DTileType.Ore] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Health = Convert.ToInt32(DRandomMath.Range(4, 5) + MathF.Floor(position.Y / DWorldConstants.TILES_PER_CHUNK_HEIGHT));
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.OnDestroyed = () => gameInformation.PlayerEntity.CollectOre(tile.Ore);
                },

                [DTileType.Stair] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                },

                [DTileType.Box] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                    tile.Health = 2;
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                    tile.OnDestroyed = () =>
                    {
                        switch (DRandomMath.Range(0, 1))
                        {
                            case 0:
                                gameInformation.PlayerEntity.Money += (uint)DRandomMath.Range(1, 5);
                                break;
                            case 1:
                                gameInformation.PlayerEntity.StairCount += (uint)DRandomMath.Range(3, 6);
                                break;
                        }
                    };
                },

                [DTileType.SpikeTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.HasGravity = true;
                },

                [DTileType.Monster] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = DDirection.Right;
                    tile.HasGravity = true;
                    tile.Health = 3;
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                },

                [DTileType.Wall] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.IsSolid = true;
                },

                [DTileType.BoulderTrap] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = DRandomMath.Chance(50, 100) ? DDirection.Left : DDirection.Right;
                    tile.HasGravity = true;
                    tile.Health = 5;
                    tile.IsSolid = true;
                    tile.IsDestructible = true;
                },

                [DTileType.Platform] = SetDefaults,

                [DTileType.Ghost] = (tile, position) =>
                {
                    SetDefaults(tile, position);
                    tile.Direction = DDirection.Left;
                    tile.IsSolid = true;
                },
            };
        }

        internal void Update()
        {
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
                    UpdateTileHealth(tile, position);
                    UpdateTileGravity(tile, position);

                    switch (tile.Type)
                    {
                        case DTileType.SpikeTrap:
                            UpdateSpikeTrap(position);
                            break;

                        case DTileType.BoulderTrap:
                            if (++this.boulderTrapFrameCounter > this.boulderTrapFrameDelay)
                            {
                                this.boulderTrapFrameCounter = 0;
                                UpdateBoulderTrap(tile, position);
                            }
                            break;

                        case DTileType.Monster:
                            if (++this.monsterMovementFrameCounter > this.monsterMovementFrameDelay)
                            {
                                this.monsterMovementFrameCounter = 0;
                                UpdateMonster(position);
                            }
                            break;

                        case DTileType.Ghost:
                            if (++this.ghostMovementFrameCounter > this.ghostMovementFrameDelay)
                            {
                                this.ghostMovementFrameCounter = 0;
                                UpdateGhost(position);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
        }

        private void UpdateTileHealth(DTile tile, DPoint position)
        {
            if (tile.Health == 0 && tile.IsDestructible)
            {
                tile.OnDestroyed?.Invoke();
                SetTile(position, DTileType.Empty);
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
            DTile bottomTile = GetTile(new DPoint(position.X, position.Y + 1));
            if (bottomTile != null && !bottomTile.IsSolid)
            {
                bottomTile.Copy(tile);
                SetTile(position, DTileType.Empty);
                return;
            }

            switch (tile.Direction)
            {
                case DDirection.Right:
                    DTile rightTile = GetTile(new DPoint(position.X + 1, position.Y));
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
                    DTile leftTile = GetTile(new DPoint(position.X - 1, position.Y));
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

        private void UpdateMonster(DPoint position)
        {
            DTile tile = GetTile(position);
            if (tile == null || tile.Type != DTileType.Monster)
            {
                return;
            }

            DDirection nextDirection = tile.Direction == DDirection.Left ? DDirection.Right : DDirection.Left;
            DPoint nextPosition = new(position.X + (tile.Direction == DDirection.Left ? -1 : 1), position.Y);
            DTile nextTile = GetTile(nextPosition);
            DTile tileBelow = GetTile(new(position.X, position.Y + 1));
            DTile nextTileBelow = GetTile(new(nextPosition.X, nextPosition.Y + 1));

            // Se há um bloco sólido à frente ou o próximo tile não tem chão, inverte a direção.
            if (nextTile == null || nextTile.IsSolid || (nextTileBelow != null && !nextTileBelow.IsSolid))
            {
                tile.Direction = nextDirection;
            }
            else
            {
                nextTile.Copy(tile);
                SetTile(position, DTileType.Empty);
            }
        }

        private void UpdateGhost(DPoint position)
        {
            DTile tile = GetTile(position);
            if (tile == null || tile.Type != DTileType.Ghost)
            {
                return;
            }

            List<DDirection> possibleDirections = new();
            Dictionary<DDirection, DPoint> directionOffsets = new()
            {
                { DDirection.Left, new DPoint(position.X - 1, position.Y) },
                { DDirection.Right, new DPoint(position.X + 1, position.Y) },
                { DDirection.Up, new DPoint(position.X, position.Y - 1) },
                { DDirection.Down, new DPoint(position.X, position.Y + 1) }
            };

            foreach (KeyValuePair<DDirection, DPoint> entry in directionOffsets)
            {
                DTile nextTile = GetTile(entry.Value);
                if (nextTile != null && !nextTile.IsSolid)
                {
                    possibleDirections.Add(entry.Key);
                }
            }

            if (possibleDirections.Count > 0)
            {
                DDirection chosenDirection = possibleDirections[DRandomMath.Range(0, possibleDirections.Count - 1)];
                DPoint nextPosition = directionOffsets[chosenDirection];

                DTile nextTile = GetTile(nextPosition);
                nextTile.Copy(tile);
                SetTile(position, DTileType.Empty);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (byte y = 0; y < this.size.Height; y++)
            {
                for (byte x = 0; x < this.size.Width; x++)
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
                DTileType.Monster => this.assetDatabase.GetTexture("texture_tile_7"),
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
            for (byte y = 0; y < this.size.Height; y++)
            {
                for (byte x = 0; x < this.size.Width; x++)
                {
                    SetTile(new DPoint(x, y), DTileType.Empty);
                }
            }
        }
    }
}
