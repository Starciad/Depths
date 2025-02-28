using Depths.Core.Constants;
using Depths.Core.Enums.General;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class DTruckEntityDescriptor : DEntityDescriptor
    {
        internal DTruckEntityDescriptor(string identifier, Texture2D texture, DWorld world) : base(identifier, texture, world)
        {

        }

        internal override DEntity CreateEntity()
        {
            return new DTruckEntity(this);
        }
    }

    internal sealed class DTruckEntity : DEntity
    {
        private DDirection direction;

        private readonly Texture2D texture;

        internal DTruckEntity(DEntityDescriptor descriptor) : base(descriptor)
        {
            this.texture = descriptor.Texture;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.direction = DDirection.Left;
        }

        private Rectangle? GetCurrentSpriteRectangle()
        {
            return this.direction switch
            {
                DDirection.Right => (Rectangle?)DSpriteConstants.TRUCK_SOURCE_RECTANGLES[2],
                DDirection.Left => (Rectangle?)DSpriteConstants.TRUCK_SOURCE_RECTANGLES[0],
                _ => null,
            };
        }
    }
}
