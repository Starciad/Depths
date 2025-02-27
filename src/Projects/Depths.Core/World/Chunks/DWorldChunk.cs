using Depths.Core.Constants;
using Depths.Core.Enums.World;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World.Tiles;

using System;
using System.Collections.Generic;

namespace Depths.Core.World.Chunks
{
    internal sealed class DWorldChunk
    {
        internal required DWorldChunkType Type { get; init; }
        internal required string[,] Mapping
        {
            get => this.mapping;

            init
            {
                string[,] matrix = new string[DWorldConstants.TILES_PER_CHUNK_WIDTH, DWorldConstants.TILES_PER_CHUNK_HEIGHT];

                // Allocate elements from the string array to the chunk's 2d array.
                for (byte y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
                {
                    for (byte x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                    {
                        matrix[x, y] = value[y, x];
                    }
                }

                this.mapping = matrix;
            }
        }

        private string[,] mapping;

        private static readonly Dictionary<string, Action<DTilemap, DPoint>> tileActions = new()
        {
            #region TILES
            ["T0"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Empty),
            ["T1"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Dirt),
            ["T2"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stone),
            ["T3"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stair),
            ["T4"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Box),
            ["T5"] = (tilemap, position) => tilemap.SetTile(position, DTileType.SpikeTrap),
            ["T6"] = (tilemap, position) => tilemap.SetTile(position, DTileType.ArrowTrap),
            ["T7"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Wall),
            ["T8"] = (tilemap, position) => tilemap.SetTile(position, DTileType.BoulderTrap),
            ["T9"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Platform),
            #endregion

            #region RANDOM
            ["R0"] = (tilemap, position) => { },
            #endregion
        };

        internal void ApplyToTilemap(DPoint chunkPosition, DTilemap tilemap)
        {
            uint baseY = (uint)chunkPosition.Y * DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            uint baseX = (uint)chunkPosition.X * DWorldConstants.TILES_PER_CHUNK_WIDTH;

            for (uint y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (uint x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    PlaceTile(this.Mapping[x, y], tilemap, new((byte)(baseX + x), (byte)(baseY + y)));
                }
            }
        }

        private static void PlaceTile(string tileCode, DTilemap tilemap, DPoint position)
        {
            if (tileActions.TryGetValue(tileCode, out Action<DTilemap, DPoint> action))
            {
                action(tilemap, position);
            }
        }
    }
}
