using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class EntityDescriptor(string identifier, Texture2D texture, World.World world)
    {
        internal string Identifier => identifier;
        internal Texture2D Texture => texture;
        internal World.World World => world;

        internal abstract Entity CreateEntity();
    }
}