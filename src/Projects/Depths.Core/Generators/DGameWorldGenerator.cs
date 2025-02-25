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
    internal sealed class DGameWorldGenerator
    {
        internal required DAssetDatabase AssetDatabase { get; init; }
        internal required DEntityDatabase EntityDatabase { get; init; }
        internal required DWorldDatabase WorldDatabase { get; init; }
        internal required DEntityManager EntityManager { get; init; }
        internal required DWorld World { get; init; }

        private DTilemap worldTilemap;
        private DSize2 worldSize;

        private readonly List<(Point Position, DTile Tile)> stoneTiles = [];
        private readonly List<(Point Position, DTile Tile)> emptyTiles = [];

        internal void Initialize()
        {
            this.worldTilemap = this.World.Tilemap;
            this.worldSize = this.worldTilemap.Size;

            GenerateWorld();
        }

        private void GenerateWorld()
        {
            GenerateSurfaceLayer();
            GenerateUndergroundLayer();
            GenerateDepthLayer();
            CollectUndergroundTiles();
            GenerateUndergroundFeatures();
            GenerateMapBorders();
        }

        private void GenerateSurfaceLayer()
        {
            IEnumerable<DWorldChunk> surfaceChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == DWorldChunkType.Surface);

            for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
            {
                DWorldChunk randomChunk = surfaceChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new Point(x, 0), this.worldTilemap);
            }
        }

        private void GenerateUndergroundLayer()
        {
            IEnumerable<DWorldChunk> undergroundChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == DWorldChunkType.Underground);

            for (int y = 1; y < DWorldConstants.WORLD_HEIGHT - 1; y++)
            {
                for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
                {
                    DWorldChunk randomChunk = undergroundChunks.GetRandomItem();
                    randomChunk.ApplyToTilemap(new Point(x, y), this.worldTilemap);
                }
            }
        }

        private void GenerateDepthLayer()
        {
            IEnumerable<DWorldChunk> depthChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == DWorldChunkType.Depth);

            for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
            {
                DWorldChunk randomChunk = depthChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new Point(x, DWorldConstants.WORLD_HEIGHT - 1), this.worldTilemap);
            }
        }

        private void CollectUndergroundTiles()
        {
            int startY = DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            int endY = this.worldSize.Height - DWorldConstants.TILES_PER_CHUNK_HEIGHT;

            for (int y = startY; y < endY; y++)
            {
                for (int x = 0; x < this.worldSize.Width; x++)
                {
                    Point position = new(x, y);
                    DTile tile = this.worldTilemap.GetTile(position);

                    if (tile == null)
                    {
                        continue;
                    }

                    switch (tile.Type)
                    {
                        case DTileType.Empty:
                            this.emptyTiles.Add((position, tile));
                            break;
                        case DTileType.Stone:
                            this.stoneTiles.Add((position, tile));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GenerateUndergroundFeatures()
        {
            GenerateOres();
            GenerateAdditionalBlocks();
            GenerateTraps();
            GenerateEntities();
        }

        private void GenerateOres()
        {
            foreach (DOre ore in this.WorldDatabase.Ores)
            {
                int orePlacementCount = DRandomMath.Range(50, 150);
                if (this.stoneTiles.Count < orePlacementCount)
                {
                    return;
                }

                for (int i = 0; i < orePlacementCount; i++)
                {
                    bool isOrePlaced = DRandomMath.Chance(DRarityUtility.GetOreNumericalChance(ore.Rarity), 100);
                    if (!isOrePlaced)
                    {
                        continue;
                    }

                    (Point Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();
                    bool removed = this.stoneTiles.Remove(tileEntry);
                    if (!removed)
                    {
                        continue;
                    }

                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.Ore);
                    tileEntry.Tile.Ore = ore;
                }
            }
        }

        private void GenerateAdditionalBlocks()
        {
            GenerateBoxes();
            GenerateDirt();
            GenerateWalls();
        }

        private void GenerateBoxes()
        {
            int boxCount = DRandomMath.Range(16, 32);
            if (this.stoneTiles.Count < boxCount)
            {
                return;
            }

            for (int i = 0; i < boxCount; i++)
            {
                (Point Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();
                bool removed = this.stoneTiles.Remove(tileEntry);
                if (!removed)
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Box);
            }
        }

        private void GenerateDirt()
        {
            int dirtCount = DRandomMath.Range(100, 200);
            if (this.stoneTiles.Count < dirtCount)
            {
                return;
            }

            for (int i = 0; i < dirtCount; i++)
            {
                (Point Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();
                bool removed = this.stoneTiles.Remove(tileEntry);
                if (!removed)
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Dirt);
            }
        }

        private void GenerateWalls()
        {
            int wallCount = DRandomMath.Range(100, 200);
            if (this.stoneTiles.Count < wallCount)
            {
                return;
            }

            for (int i = 0; i < wallCount; i++)
            {
                (Point Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();
                bool removed = this.stoneTiles.Remove(tileEntry);
                if (!removed)
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Wall);
            }
        }

        private void GenerateTraps()
        {
            GenerateStoneTraps();
            GenerateVoidTraps();
        }

        private void GenerateStoneTraps()
        {
            int trapCount = DRandomMath.Range(30, 60);
            if (this.stoneTiles.Count < trapCount)
            {
                return;
            }

            for (int i = 0; i < trapCount; i++)
            {
                (Point Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();
                bool removed = this.stoneTiles.Remove(tileEntry);
                if (!removed)
                {
                    continue;
                }

                int trapType = DRandomMath.Range(0, 1);
                if (trapType == 0)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.BoulderTrap);
                }
                else if (trapType == 1)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.ExplosiveTrap);
                }
            }
        }

        private void GenerateVoidTraps()
        {
            int trapCount = DRandomMath.Range(30, 60);
            if (this.emptyTiles.Count < trapCount)
            {
                return;
            }

            for (int i = 0; i < trapCount; i++)
            {
                (Point Position, DTile Tile) tileEntry = this.emptyTiles.GetRandomItem();
                Point belowPosition = new(tileEntry.Position.X, tileEntry.Position.Y + 1);
                DTile tileBelow = this.worldTilemap.GetTile(belowPosition);

                int trapType = DRandomMath.Range(0, 2);
                if (trapType == 0)
                {
                    if (tileBelow != null && tileBelow.Type != DTileType.Empty)
                    {
                        this.worldTilemap.SetTile(tileEntry.Position, DTileType.SpikeTrap);
                        _ = this.emptyTiles.Remove(tileEntry);
                    }
                }
                else if (trapType == 1)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.ArrowTrap);
                    _ = this.emptyTiles.Remove(tileEntry);
                }
                else if (trapType == 2)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.BoulderTrap);
                    _ = this.emptyTiles.Remove(tileEntry);
                }
            }
        }

        private static void GenerateEntities()
        {
            // Implementation for entity generation can be added here.
        }

        private void GenerateMapBorders()
        {
            // Set vertical borders (left and right)
            for (int y = 0; y < this.worldSize.Height; y++)
            {
                this.worldTilemap.SetTile(new Point(0, y), DTileType.Wall);
                this.worldTilemap.SetTile(new Point(this.worldSize.Width - 1, y), DTileType.Wall);
            }

            // Set horizontal bottom border
            for (int x = 0; x < this.worldSize.Width; x++)
            {
                this.worldTilemap.SetTile(new Point(x, this.worldSize.Height - 1), DTileType.Wall);
            }
        }
    }
}
