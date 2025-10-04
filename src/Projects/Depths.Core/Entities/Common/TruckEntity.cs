using Depths.Core.Constants;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class TruckEntityDescriptor : EntityDescriptor
    {
        internal TruckEntityDescriptor(string identifier, Texture2D texture, World.World world) : base(identifier, texture, world)
        {

        }

        internal override Entity CreateEntity()
        {
            return new TruckEntity(this);
        }
    }

    internal sealed class TruckEntity : Entity
    {
        internal bool IsMoving { get; set; }

        private bool spriteState;
        private byte spriteAnimationFrameCounter;

        private readonly byte spriteAnimationFrameDelay = 3;
        private readonly Texture2D texture;

        internal TruckEntity(EntityDescriptor descriptor) : base(descriptor)
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
                ? SpriteConstants.TRUCK_SOURCE_RECTANGLES[0]
                : this.spriteState ? SpriteConstants.TRUCK_SOURCE_RECTANGLES[0] : SpriteConstants.TRUCK_SOURCE_RECTANGLES[1];
        }
    }
}
