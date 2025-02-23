using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class DEntityDescriptor(string identifier, Texture2D texture)
    {
        internal string Identifier => identifier;
        internal Texture2D Texture => texture;

        internal abstract DEntity CreateEntity();
    }
}