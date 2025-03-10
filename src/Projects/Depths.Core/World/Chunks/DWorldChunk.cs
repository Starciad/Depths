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
        internal required DCI[,] Mapping
        {
            get => this.mapping;

            init
            {
                DCI[,] matrix = new DCI[DWorldConstants.TILES_PER_CHUNK_WIDTH, DWorldConstants.TILES_PER_CHUNK_HEIGHT];

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

        private DCI[,] mapping;

        private static readonly Dictionary<DCI, Action<DTilemap, DPoint>> tileActions = new()
        {
            [DCI.NN] = (tilemap, position) => { },
            [DCI.T0] = (tilemap, position) => tilemap.SetTile(position, DTileType.Empty),
            [DCI.T1] = (tilemap, position) => tilemap.SetTile(position, DTileType.Dirt),
            [DCI.T2] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stone),
            [DCI.T3] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stair),
            [DCI.T4] = (tilemap, position) => tilemap.SetTile(position, DTileType.Box),
            [DCI.T5] = (tilemap, position) => tilemap.SetTile(position, DTileType.SpikeTrap),
            [DCI.T6] = (tilemap, position) => tilemap.SetTile(position, DTileType.Wood),
            [DCI.T7] = (tilemap, position) => tilemap.SetTile(position, DTileType.Wall),
            [DCI.T8] = (tilemap, position) => tilemap.SetTile(position, DTileType.BoulderTrap),
            [DCI.T9] = (tilemap, position) => tilemap.SetTile(position, DTileType.Platform),
            [DCI.T10] = (tilemap, position) => tilemap.SetTile(position, DTileType.Ghost),
        };

        internal void ApplyToTilemap(DPoint chunkPosition, DTilemap tilemap)
        {
            uint baseY = (uint)chunkPosition.Y * DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            uint baseX = (uint)chunkPosition.X * DWorldConstants.TILES_PER_CHUNK_WIDTH;

            for (uint y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (uint x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    PlaceItem(this.Mapping[x, y], tilemap, new((byte)(baseX + x), (byte)(baseY + y)));
                }
            }
        }

        private static void PlaceItem(DCI chunkItem, DTilemap tilemap, DPoint position)
        {
            if (tileActions.TryGetValue(chunkItem, out Action<DTilemap, DPoint> action))
            {
                action(tilemap, position);
            }
        }
    }
}
