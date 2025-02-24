using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World.Chunks;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.Extensions;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.Utilities;
using Depths.Core.World;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace Depths.Core.Generators
{
    internal sealed class DGameGenerator
    {
        internal required DAssetDatabase AssetDatabase { get; init; }
        internal required DEntityDatabase EntityDatabase { get; init; }
        internal required DWorldDatabase WorldDatabase { get; init; }
        internal required DEntityManager EntityManager { get; init; }
        internal required DWorld World { get; init; }

        private DTilemap worldTilemap;
        private DSize2 worldSize;

        private readonly List<(Point, DTile)> stoneTiles = [];
        private readonly List<(Point, DTile)> emptyTiles = [];

        internal void Initialize()
        {
            this.worldTilemap = this.World.Tilemap;
            this.worldSize = this.worldTilemap.Size;

            GenerateWorld();
        }

        private void GenerateWorld()
        {
            GenerateWorldSurface();
            GenerateWorldUnderground();
            GenerateWorldDepths();
            GetAllUndergroundTiles();
            GenerateUnderground();
            GenerateMapBorders();
        }

        private void GenerateWorldSurface()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Surface);

            for (int i = 0; i < DWorldConstants.WORLD_WIDTH; i++)
            {
                chunks.GetRandomItem().ApplyToTilemap(new(i, 0), this.worldTilemap);
            }
        }

        private void GenerateWorldUnderground()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Underground);

            for (int y = 1; y < DWorldConstants.WORLD_HEIGHT - 1; y++)
            {
                for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
                {
                    chunks.GetRandomItem().ApplyToTilemap(new(x, y), this.worldTilemap);
                }
            }
        }

        private void GenerateWorldDepths()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Depth);

            for (int i = 0; i < DWorldConstants.WORLD_WIDTH; i++)
            {
                chunks.GetRandomItem().ApplyToTilemap(new(i, DWorldConstants.WORLD_HEIGHT - 1), this.worldTilemap);
            }
        }

        private void GetAllUndergroundTiles()
        {
            for (int y = DWorldConstants.TILES_PER_CHUNK_HEIGHT; y < this.worldSize.Height - DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < this.worldSize.Width; x++)
                {
                    DTile tile = this.worldTilemap.GetTile(new(x, y));

                    if (tile == null)
                    {
                        continue;
                    }

                    switch (tile.Type)
                    {
                        case DTileType.Empty:
                            this.emptyTiles.Add((new(x, y), tile));
                            break;

                        case DTileType.Stone:
                            this.stoneTiles.Add((new(x, y), tile));
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void GenerateUnderground()
        {
            GenerateOres();
            GenerateExtraBlocks();
            GenerateTraps();
            GenerateEntities();
        }

        private void GenerateOres()
        {
            foreach (DOre ore in this.WorldDatabase.Ores)
            {
                int total = DRandomMath.Range(50, 150);

                if (this.stoneTiles.Count < total)
                {
                    return;
                }

                for (int i = 0; i < total; i++)
                {
                    if (!DRandomMath.Chance(DRarityUtility.GetOreNumericalChance(ore.Rarity), 100))
                    {
                        continue;
                    }

                    (Point, DTile) value = this.stoneTiles.GetRandomItem();
                    _ = this.stoneTiles.Remove(value);

                    this.worldTilemap.SetTile(value.Item1, DTileType.Ore);
                    value.Item2.Ore = ore;
                }
            }
        }

        private void GenerateExtraBlocks()
        {
            GenerateBoxes();
            GenerateDirt();
            GenerateWalls();
        }

        private void GenerateBoxes()
        {
            int total = DRandomMath.Range(80, 150);

            if (this.stoneTiles.Count < total)
            {
                return;
            }

            for (int i = 0; i < total; i++)
            {
                (Point, DTile) value = this.stoneTiles.GetRandomItem();
                _ = this.stoneTiles.Remove(value);

                this.worldTilemap.SetTile(value.Item1, DTileType.Box);
            }
        }

        private void GenerateDirt()
        {
            int total = DRandomMath.Range(100, 200);

            if (this.stoneTiles.Count < total)
            {
                return;
            }

            for (int i = 0; i < total; i++)
            {
                (Point, DTile) value = this.stoneTiles.GetRandomItem();
                _ = this.stoneTiles.Remove(value);

                this.worldTilemap.SetTile(value.Item1, DTileType.Dirt);
            }
        }

        private void GenerateWalls()
        {
            int total = DRandomMath.Range(100, 200);

            if (this.stoneTiles.Count < total)
            {
                return;
            }

            for (int i = 0; i < total; i++)
            {
                (Point, DTile) value = this.stoneTiles.GetRandomItem();
                _ = this.stoneTiles.Remove(value);

                this.worldTilemap.SetTile(value.Item1, DTileType.Wall);
            }
        }

        private void GenerateTraps()
        {
            GenerateTrapsInStones();
            GenerateTrapsInVoids();
        }

        private void GenerateTrapsInStones()
        {
            int total = DRandomMath.Range(30, 60);

            if (this.stoneTiles.Count < total)
            {
                return;
            }

            for (int i = 0; i < total; i++)
            {
                (Point, DTile) value = this.stoneTiles.GetRandomItem();
                _ = this.stoneTiles.Remove(value);

                switch (DRandomMath.Range(0, 1))
                {
                    case 0:
                        this.worldTilemap.SetTile(value.Item1, DTileType.BoulderTrap);
                        break;

                    case 1:
                        this.worldTilemap.SetTile(value.Item1, DTileType.ExplosiveTrap);
                        break;

                    default:
                        break;
                }
            }
        }

        private void GenerateTrapsInVoids()
        {
            int total = DRandomMath.Range(30, 60);

            if (this.emptyTiles.Count < total)
            {
                return;
            }

            for (int i = 0; i < total; i++)
            {
                (Point, DTile) value = this.emptyTiles.GetRandomItem();
                DTile tileBelow = this.worldTilemap.GetTile(new(value.Item1.X, value.Item1.Y + 1));

                switch (DRandomMath.Range(0, 1))
                {
                    case 0:
                        if (tileBelow != null && tileBelow.Type != DTileType.Empty)
                        {
                            this.worldTilemap.SetTile(value.Item1, DTileType.SpikeTrap);
                            _ = this.emptyTiles.Remove(value);
                        }

                        break;

                    case 1:
                        this.worldTilemap.SetTile(value.Item1, DTileType.ArrowTrap);
                        _ = this.emptyTiles.Remove(value);
                        break;

                    default:
                        break;
                }
            }
        }

        private void GenerateEntities()
        {

        }

        private void GenerateMapBorders()
        {
            for (int i = 0; i < this.worldSize.Height; i++)
            {
                // Right
                this.worldTilemap.SetTile(new(0, i), DTileType.Wall);

                // Left
                this.worldTilemap.SetTile(new(this.worldSize.Width - 1, i), DTileType.Wall);
            }

            for (int i = 0; i < this.worldSize.Width; i++)
            {
                // Bottom
                this.worldTilemap.SetTile(new(i, this.worldSize.Height - 1), DTileType.Wall);
            }
        }
    }
}
