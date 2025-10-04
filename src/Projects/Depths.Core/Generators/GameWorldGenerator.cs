using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World;
using Depths.Core.Extensions;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.Utilities;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Depths.Core.Generators
{
    internal sealed class GameWorldGenerator
    {
        internal required AssetDatabase AssetDatabase { get; init; }
        internal required EntityDatabase EntityDatabase { get; init; }
        internal required WorldDatabase WorldDatabase { get; init; }
        internal required EntityManager EntityManager { get; init; }
        internal required World.World World { get; init; }

        private Tilemap worldTilemap;
        private DSize2 worldSize;

        private readonly List<(DPoint Position, Tile Tile)> stoneTiles = [];
        private readonly List<(DPoint Position, Tile Tile)> emptyTiles = [];

        internal void Initialize()
        {
            this.worldTilemap = this.World.Tilemap;
            this.worldSize = this.worldTilemap.Size;

            GenerateWorld();
        }

        private void GenerateWorld()
        {
            GenerateUndergroundLayer();
            GenerateSurfaceLayer();
            GenerateDepthLayer();
            CollectUndergroundTiles();
            GenerateUndergroundFeatures();
            GenerateMapBorders();
        }

        private void GenerateUndergroundLayer()
        {
            IEnumerable<WorldChunk> undergroundChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == WorldChunkType.Underground);

            for (byte y = 1; y < WorldConstants.WORLD_HEIGHT - 1; y++)
            {
                for (byte x = 0; x < WorldConstants.WORLD_WIDTH; x++)
                {
                    WorldChunk randomChunk = undergroundChunks.GetRandomItem();
                    randomChunk.ApplyToTilemap(new(x, y), this.worldTilemap);
                }
            }
        }

        private void GenerateSurfaceLayer()
        {
            IEnumerable<WorldChunk> surfaceChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == WorldChunkType.Surface);

            for (byte x = 0; x < WorldConstants.WORLD_WIDTH; x++)
            {
                WorldChunk randomChunk = surfaceChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new(x, 0), this.worldTilemap);
            }
        }

        private void GenerateDepthLayer()
        {
            IEnumerable<WorldChunk> depthChunks = this.WorldDatabase.Chunks
                .Where(chunk => chunk.Type == WorldChunkType.Depth);

            for (byte x = 0; x < WorldConstants.WORLD_WIDTH; x++)
            {
                WorldChunk randomChunk = depthChunks.GetRandomItem();
                randomChunk.ApplyToTilemap(new(x, WorldConstants.WORLD_HEIGHT - 1), this.worldTilemap);
            }
        }

        private void CollectUndergroundTiles()
        {
            int startY = WorldConstants.TILES_PER_CHUNK_HEIGHT;
            int endY = Convert.ToUInt16(this.worldSize.Height - WorldConstants.TILES_PER_CHUNK_HEIGHT);

            for (int y = startY; y < endY; y++)
            {
                for (int x = 0; x < this.worldSize.Width; x++)
                {
                    DPoint position = new(x, y);
                    Tile tile = this.worldTilemap.GetTile(position);

                    if (tile == null)
                    {
                        continue;
                    }

                    switch (tile.Type)
                    {
                        case TileType.Empty:
                            this.emptyTiles.Add((position, tile));
                            break;

                        case TileType.Stone:
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
        }

        private void GenerateOres()
        {
            foreach (Ore ore in this.WorldDatabase.Ores)
            {
                byte orePlacementCount = (byte)RandomMath.Range(50, 100);

                // Filter stone tiles to only include those in the ore's allowed chunk range.
                List<(DPoint Position, Tile Tile)> validStoneTiles = [];

                foreach ((DPoint Position, Tile Tile) stoneTile in this.stoneTiles)
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
                    if (!RandomMath.Chance(RarityUtility.GetOreNumericalChance(ore.Rarity), 100))
                    {
                        continue;
                    }

                    // Pick a random valid stone tile.
                    (DPoint Position, Tile Tile) selectedTile = validStoneTiles.GetRandomItem();

                    if (!this.stoneTiles.Remove(selectedTile) || !validStoneTiles.Remove(selectedTile))
                    {
                        continue;
                    }

                    this.worldTilemap.SetTile(selectedTile.Position, TileType.Ore);
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
            byte boxCount = (byte)RandomMath.Range(16, 32);

            if (this.stoneTiles.Count < boxCount)
            {
                return;
            }

            for (byte i = 0; i < boxCount; i++)
            {
                (DPoint Position, Tile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, TileType.Box);
            }
        }

        private void GenerateDirt()
        {
            byte dirtCount = (byte)RandomMath.Range(100, 255);

            if (this.stoneTiles.Count < dirtCount)
            {
                return;
            }

            for (byte i = 0; i < dirtCount; i++)
            {
                (DPoint Position, Tile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, TileType.Dirt);
            }
        }

        private void GenerateWalls()
        {
            byte wallCount = (byte)RandomMath.Range(150, 350);

            if (this.stoneTiles.Count < wallCount)
            {
                return;
            }

            for (byte i = 0; i < wallCount; i++)
            {
                (DPoint Position, Tile Tile) tileEntry = this.stoneTiles.GetRandomItem();

                if (!this.stoneTiles.Remove(tileEntry))
                {
                    continue;
                }

                this.worldTilemap.SetTile(tileEntry.Position, TileType.Wall);
            }
        }

        private void GenerateMapBorders()
        {
            // Set vertical borders (left and right)
            for (byte y = 0; y < this.worldSize.Height; y++)
            {
                this.worldTilemap.SetTile(new(0, y), TileType.Wall);
                this.worldTilemap.SetTile(new(this.worldSize.Width - 1, y), TileType.Wall);
            }

            // Set horizontal bottom border
            for (byte x = 0; x < this.worldSize.Width; x++)
            {
                this.worldTilemap.SetTile(new(x, this.worldSize.Height - 1), TileType.Wall);
            }
        }

        // =============================== //
        // Utilities

        private static byte GetYLayerIndex(int tileYPosition)
        {
            return Convert.ToByte(MathF.Floor(tileYPosition / WorldConstants.TILES_PER_CHUNK_HEIGHT));
        }
    }
}
