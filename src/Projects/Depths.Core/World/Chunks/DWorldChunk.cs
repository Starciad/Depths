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
        internal DWorldChunkType Type => this.type;

        private readonly DWorldChunkType type;
        private readonly string[,] content;

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

        internal DWorldChunk(DWorldChunkType type, string[,] content)
        {
            this.type = type;
            this.content = content;
        }

        internal void ApplyToTilemap(DPoint chunkPosition, DTilemap tilemap)
        {
            uint baseY = (uint)chunkPosition.Y * DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            uint baseX = (uint)chunkPosition.X * DWorldConstants.TILES_PER_CHUNK_WIDTH;

            for (uint y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (uint x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    PlaceTile(this.content[x, y], tilemap, new((byte)(baseX + x), (byte)(baseY + y)));
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
