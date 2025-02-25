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
        private readonly DDirection direction = DDirection.Left;

        private readonly Texture2D texture;

        private readonly Rectangle[] textureClipAreas = [
            new(new(0, 0), new(48, 25)), // Left [1]
            new(new(0, 25), new(48, 25)), // Left [2]
            new(new(48, 0), new(48, 25)), // Right [1]
            new(new(48, 25), new(48, 25)), // Right [2]
        ];

        internal DTruckEntity(DEntityDescriptor descriptor) : base(descriptor)
        {
            this.texture = descriptor.Texture;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private Rectangle? GetCurrentSpriteRectangle()
        {
            return this.direction switch
            {
                DDirection.Right => (Rectangle?)this.textureClipAreas[2],
                DDirection.Left => (Rectangle?)this.textureClipAreas[0],
                _ => null,
            };
        }
    }
}
