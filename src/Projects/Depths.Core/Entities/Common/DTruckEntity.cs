using Depths.Core.Constants;
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
        internal bool IsMoving { get; set; }

        private bool spriteState;
        private byte spriteAnimationFrameCounter;

        private readonly byte spriteAnimationFrameDelay = 3;
        private readonly Texture2D texture;

        internal DTruckEntity(DEntityDescriptor descriptor) : base(descriptor)
        {
            this.texture = descriptor.Texture;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.IsMoving && ++this.spriteAnimationFrameCounter > this.spriteAnimationFrameDelay)
            {
                this.spriteAnimationFrameCounter = 0;
                this.spriteState = !this.spriteState;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.IsMoving = true;
        }

        private Rectangle GetCurrentSpriteRectangle()
        {
            return !this.IsMoving
                ? DSpriteConstants.TRUCK_SOURCE_RECTANGLES[0]
                : this.spriteState ? DSpriteConstants.TRUCK_SOURCE_RECTANGLES[0] : DSpriteConstants.TRUCK_SOURCE_RECTANGLES[1];
        }
    }
}
