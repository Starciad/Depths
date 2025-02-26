using Depths.Core.Constants;
using Depths.Core.Mathematics.Primitives;

namespace Depths.Core.Mathematics
{
    internal static class DTilemapMath
    {
        internal static DPoint ToLocalPosition(DPoint position)
        {
            return new(
                position.X / DWorldConstants.TILE_SIZE,
                position.Y / DWorldConstants.TILE_SIZE
            );
        }

        internal static DPoint ToGlobalPosition(DPoint position)
        {
            return new(
                position.X * DWorldConstants.TILE_SIZE,
                position.Y * DWorldConstants.TILE_SIZE
            );
        }

        internal static DPoint GetTotalTileCount(byte chunkColumns, byte chunkRows)
        {
            return new(
                chunkColumns * DWorldConstants.TILES_PER_CHUNK_WIDTH,
                chunkRows * DWorldConstants.TILES_PER_CHUNK_HEIGHT
            );
        }
    }
}
