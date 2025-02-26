using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Extensions
{
    internal static class DTextureExtensions
    {
        internal static DSize2 GetSize(this Texture2D texture)
        {
            return new(texture.Width, texture.Height);
        }
    }
}
