using Depths.Core.Constants;
using Depths.Core.Enums.World;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World.Tiles;

using System;
using System.Collections.Generic;

namespace Depths.Core.World.Chunks
{
    internal sealed class WorldChunk
    {
        internal required WorldChunkType Type { get; init; }
        internal required CI[,] Mapping
        {
            get => this.mapping;

            init
            {
                CI[,] matrix = new CI[WorldConstants.TILES_PER_CHUNK_WIDTH, WorldConstants.TILES_PER_CHUNK_HEIGHT];

                // Allocate elements from the string array to the chunk's 2d array.
                for (byte y = 0; y < WorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
                {
                    for (byte x = 0; x < WorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                    {
                        matrix[x, y] = value[y, x];
                    }
                }

                this.mapping = matrix;
            }
        }

        private CI[,] mapping;

        private static readonly Dictionary<CI, Action<Tilemap, DPoint>> tileActions = new()
        {
            [CI.NN] = (tilemap, position) => { },
            [CI.T0] = (tilemap, position) => tilemap.SetTile(position, TileType.Empty),
            [CI.T1] = (tilemap, position) => tilemap.SetTile(position, TileType.Dirt),
            [CI.T2] = (tilemap, position) => tilemap.SetTile(position, TileType.Stone),
            [CI.T3] = (tilemap, position) => tilemap.SetTile(position, TileType.Stair),
            [CI.T4] = (tilemap, position) => tilemap.SetTile(position, TileType.Box),
            [CI.T5] = (tilemap, position) => tilemap.SetTile(position, TileType.SpikeTrap),
            [CI.T6] = (tilemap, position) => tilemap.SetTile(position, TileType.Wood),
            [CI.T7] = (tilemap, position) => tilemap.SetTile(position, TileType.Wall),
            [CI.T8] = (tilemap, position) => tilemap.SetTile(position, TileType.BoulderTrap),
            [CI.T9] = (tilemap, position) => tilemap.SetTile(position, TileType.Platform),
            [CI.T10] = (tilemap, position) => tilemap.SetTile(position, TileType.Ghost),
        };

        internal void ApplyToTilemap(DPoint chunkPosition, Tilemap tilemap)
        {
            uint baseY = (uint)chunkPosition.Y * WorldConstants.TILES_PER_CHUNK_HEIGHT;
            uint baseX = (uint)chunkPosition.X * WorldConstants.TILES_PER_CHUNK_WIDTH;

            for (uint y = 0; y < WorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (uint x = 0; x < WorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    PlaceItem(this.Mapping[x, y], tilemap, new((byte)(baseX + x), (byte)(baseY + y)));
                }
            }
        }

        private static void PlaceItem(CI chunkItem, Tilemap tilemap, DPoint position)
        {
            if (tileActions.TryGetValue(chunkItem, out Action<Tilemap, DPoint> action))
            {
                action(tilemap, position);
            }
        }
    }
}
