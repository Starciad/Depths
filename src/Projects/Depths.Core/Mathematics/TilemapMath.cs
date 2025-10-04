using Depths.Core.Constants;
using Depths.Core.Mathematics.Primitives;

namespace Depths.Core.Mathematics
{
    internal static class TilemapMath
    {
        internal static DPoint ToLocalPosition(DPoint position)
        {
            return new(
                position.X / WorldConstants.TILE_SIZE,
                position.Y / WorldConstants.TILE_SIZE
            );
        }

        internal static DPoint ToGlobalPosition(DPoint position)
        {
            return new(
                position.X * WorldConstants.TILE_SIZE,
                position.Y * WorldConstants.TILE_SIZE
            );
        }

        internal static DPoint GetTotalTileCount(byte chunkColumns, byte chunkRows)
        {
            return new(
                chunkColumns * WorldConstants.TILES_PER_CHUNK_WIDTH,
                chunkRows * WorldConstants.TILES_PER_CHUNK_HEIGHT
            );
        }
    }
}
