using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class DPlayerEntityDescriptor : DEntityDescriptor
    {
        internal DPlayerEntityDescriptor(string identifier, Texture2D texture) : base(identifier, texture)
        {

        }

        internal override DEntity CreateEntity()
        {
            return new DPlayerEntity(this);
        }
    }

    internal sealed class DPlayerEntity : DEntity
    {
        internal DPlayerEntity(DEntityDescriptor descriptor) : base(descriptor)
        {

        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Descriptor.Texture, this.Position.ToVector2(), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
