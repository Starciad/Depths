using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World;
using Depths.Core.Extensions;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.Utilities;
using Depths.Core.World;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using System;
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

        private readonly List<(DPoint Position, DTile Tile)> stoneTiles = [];
        private readonly List<(DPoint Position, DTile Tile)> emptyTiles = [];

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

            for (byte x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
            {
                DWorldChunk randomChunk = surfaceChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new(x, 0), this.worldTilemap);
            }
        }

        private void GenerateUndergroundLayer()
        {
            IEnumerable<DWorldChunk> undergroundChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == DWorldChunkType.Underground);

            for (byte y = 1; y < DWorldConstants.WORLD_HEIGHT - 1; y++)
            {
                for (byte x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
                {
                    DWorldChunk randomChunk = undergroundChunks.GetRandomItem();
                    randomChunk.ApplyToTilemap(new(x, y), this.worldTilemap);
                }
            }
        }

        private void GenerateDepthLayer()
        {
            IEnumerable<DWorldChunk> depthChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == DWorldChunkType.Depth);

            for (byte x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
            {
                DWorldChunk randomChunk = depthChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new(x, DWorldConstants.WORLD_HEIGHT - 1), this.worldTilemap);
            }
        }

        private void CollectUndergroundTiles()
        {
            int startY = DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            int endY = Convert.ToUInt16(this.worldSize.Height - DWorldConstants.TILES_PER_CHUNK_HEIGHT);

            for (int y = startY; y < endY; y++)
            {
                for (int x = 0; x < this.worldSize.Width; x++)
                {
                    DPoint position = new(x, y);
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
            GenerateDangers();
        }

        private void GenerateOres()
        {
            foreach (DOre ore in this.WorldDatabase.Ores)
            {
                byte orePlacementCount = (byte)DRandomMath.Range(50, 100);

                // Filter stone tiles to only include those in the ore's allowed chunk range.
                List<(DPoint Position, DTile Tile)> validStoneTiles = [];

                foreach ((DPoint Position, DTile Tile) stoneTile in this.stoneTiles)
                {
                    byte layerIndex = GetYLayerIndex(stoneTile.Position.Y);

                    if (layerIndex >= ore.LayerRange.Start.Value && layerIndex <= ore.LayerRange.End.Value)
                    {
                        validStoneTiles.Add(stoneTile);
                    }
                }

                // If there are not enough valid stone tiles, skip this ore.
                if (validStoneTiles.Count < orePlacementCount)
                {
                    continue;
                }

                for (byte i = 0; i < orePlacementCount; i++)
                {
                    if (!DRandomMath.Chance(DRarityUtility.GetOreNumericalChance(ore.Rarity), 100))
                    {
                        continue;
                    }

                    // Pick a random valid stone tile.
                    (DPoint Position, DTile Tile) selectedTile = validStoneTiles.GetRandomItem();

                    if (!this.stoneTiles.Remove(selectedTile) || !validStoneTiles.Remove(selectedTile))
                    {
                        continue;
                    }

                    this.worldTilemap.SetTile(selectedTile.Position, DTileType.Ore);
                    selectedTile.Tile.Ore = ore;
                    selectedTile.Tile.Resistance = ore.Resistance;
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
            byte boxCount = (byte)DRandomMath.Range(16, 32);

            if (this.stoneTiles.Count < boxCount)
            {
                return;
            }

            for (byte i = 0; i < boxCount; i++)
            {
                (DPoint Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Box);
            }
        }

        private void GenerateDirt()
        {
            byte dirtCount = (byte)DRandomMath.Range(100, 255);

            if (this.stoneTiles.Count < dirtCount)
            {
                return;
            }

            for (byte i = 0; i < dirtCount; i++)
            {
                (DPoint Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Dirt);
            }
        }

        private void GenerateWalls()
        {
            byte wallCount = (byte)DRandomMath.Range(100, 255);

            if (this.stoneTiles.Count < wallCount)
            {
                return;
            }

            for (byte i = 0; i < wallCount; i++)
            {
                (DPoint Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, DTileType.Wall);
            }
        }

        private void GenerateDangers()
        {
            byte trapCount = (byte)DRandomMath.Range(30, 60);

            if (this.stoneTiles.Count < trapCount)
            {
                return;
            }

            for (byte i = 0; i < trapCount; i++)
            {
                (DPoint Position, DTile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                byte dangerType = (byte)DRandomMath.Range(0, 1);

                if (dangerType == 0)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.BoulderTrap);
                }
                else if (dangerType == 1)
                {
                    this.worldTilemap.SetTile(tileEntry.Position, DTileType.SpikeTrap);
                }
            }
        }

        private void GenerateMapBorders()
        {
            // Set vertical borders (left and right)
            for (byte y = 0; y < this.worldSize.Height; y++)
            {
                this.worldTilemap.SetTile(new(0, y), DTileType.Wall);
                this.worldTilemap.SetTile(new(this.worldSize.Width - 1, y), DTileType.Wall);
            }

            // Set horizontal bottom border
            for (byte x = 0; x < this.worldSize.Width; x++)
            {
                this.worldTilemap.SetTile(new(x, this.worldSize.Height - 1), DTileType.Wall);
            }
        }

        // =============================== //
        // Utilities

        private static byte GetYLayerIndex(int tileYPosition)
        {
            return Convert.ToByte(MathF.Floor(tileYPosition / DWorldConstants.TILES_PER_CHUNK_HEIGHT));
        }
    }
}
