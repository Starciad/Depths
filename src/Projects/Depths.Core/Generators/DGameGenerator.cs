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

using System;
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
            GenerateOres();
            GenerateExtraBlocks();
            GenerateTreasures();
            GenerateTraps();
            GenerateEntities();
            GenerateWalls();
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

        private void GenerateOres()
        {
            List<(Point, DTile)> stoneTiles = [];

            for (int y = 0; y < this.worldSize.Height; y++)
            {
                for (int x = 0; x < this.worldSize.Width; x++)
                {
                    DTile tile = this.worldTilemap.GetTile(new(x, y));

                    if (tile != null && tile.Type == DTileType.Stone)
                    {
                        stoneTiles.Add((new(x, y), tile));
                    }
                }
            }

            foreach (DOre ore in this.WorldDatabase.Ores)
            {
                for (int i = 0; i < DRandomMath.Range(50, 100); i++)
                {
                    if (!DRandomMath.Chance(DRarityUtility.GetOreNumericalChance(ore.Rarity), 100))
                    {
                        continue;
                    }

                    (Point, DTile) value = stoneTiles.GetRandomItem();

                    stoneTiles.Remove(value);

                    this.worldTilemap.SetTile(value.Item1, DTileType.Ore);
                    value.Item2.Ore = ore;
                }
            }
        }

        private void GenerateExtraBlocks()
        {

        }

        private void GenerateTreasures()
        {

        }

        private void GenerateTraps()
        {

        }

        private void GenerateEntities()
        {

        }

        private void GenerateWalls()
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
