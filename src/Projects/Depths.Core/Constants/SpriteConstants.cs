using Microsoft.Xna.Framework;

namespace Depths.Core.Constants
{
    internal static class SpriteConstants
    {
        internal static byte IDOL_HEAD_VARIATIONS => (byte)IDOL_SOURCE_RECTANGLES.Length;

        internal const byte IDOL_HEAD_WIDTH = 21;
        internal const byte IDOL_HEAD_HEIGHT = 30;

        internal const byte TILE_SPRITE_SIZE = 7;
        internal const byte PLAYER_SPRITE_SIZE = 7;

        internal const byte ORE_ICON_SIZE = 11;

        internal static readonly Rectangle[] TRUCK_SOURCE_RECTANGLES = [
            new(new(0, 0), new(45, 25)), // Left [1]
            new(new(0, 25), new(45, 25)), // Left [2]
        ];

        internal static readonly Rectangle[] IDOL_SOURCE_RECTANGLES = [
            new(new(00, 00), new(21, 30)),  // [0]
            new(new(21, 00), new(21, 30)),  // [1]
            new(new(42, 00), new(21, 30)),  // [2]
            new(new(63, 00), new(21, 30)),  // [3]
            new(new(84, 00), new(21, 30)),  // [4]
            new(new(105, 00), new(21, 30)), // [5]
        ];
    }
}
