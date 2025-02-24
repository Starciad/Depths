using Depths.Core.Constants;
using Depths.Core.Enums.World.Chunks;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace Depths.Core.World.Chunks
{
    internal sealed class DWorldChunk
    {
        internal DWorldChunkType Type => this.type;

        private readonly DWorldChunkType type;
        private readonly string[,] content;

        private static readonly Dictionary<string, Action<DTilemap, Point>> tileActions = new()
        {
            #region TILES
            ["T0"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Empty),
            ["T1"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Ground),
            ["T2"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stone),
            ["T3"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Stairs),
            ["T4"] = (tilemap, position) => tilemap.SetTile(position, DTileType.MovableBlock),
            ["T5"] = (tilemap, position) => tilemap.SetTile(position, DTileType.SpikeTrap),
            ["T6"] = (tilemap, position) => tilemap.SetTile(position, DTileType.ArrowTrap),
            ["T7"] = (tilemap, position) => tilemap.SetTile(position, DTileType.Wall),
            ["T8"] = (tilemap, position) => tilemap.SetTile(position, DTileType.BoulderTrap),
            ["T9"] = (tilemap, position) => tilemap.SetTile(position, DTileType.ExplosiveTrap),
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

        internal void ApplyToTilemap(Point chunkPosition, DTilemap tilemap)
        {
            uint baseY = (uint)chunkPosition.Y * DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            uint baseX = (uint)chunkPosition.X * DWorldConstants.TILES_PER_CHUNK_WIDTH;

            for (uint y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (uint x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    PlaceTile(this.content[x, y], tilemap, new((int)(baseX + x), (int)(baseY + y)));
                }
            }
        }

        private static void PlaceTile(string tileCode, DTilemap tilemap, Point position)
        {
            if (tileActions.TryGetValue(tileCode, out Action<DTilemap, Point> action))
            {
                action(tilemap, position);
            }
        }
    }
}
