using Depths.Core.Constants;

using Microsoft.Xna.Framework;

namespace Depths.Core.Mathematics
{
    internal static class DTilemapMath
    {
        internal static Point ToLocalPosition(Point position)
        {
            return new(
                position.X / DWorldConstants.TILE_SIZE,
                position.Y / DWorldConstants.TILE_SIZE
            );
        }

        internal static Point ToGlobalPosition(Point position)
        {
            return new(
                position.X * DWorldConstants.TILE_SIZE,
                position.Y * DWorldConstants.TILE_SIZE
            );
        }
    }
}
