using Depths.Core.World;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class DEntityDescriptor(string identifier, Texture2D texture, DWorld world)
    {
        internal string Identifier => identifier;
        internal Texture2D Texture => texture;
        internal DWorld World => world;

        internal abstract DEntity CreateEntity();
    }
}