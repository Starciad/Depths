using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.Entities.Common
{
    internal sealed class DStarEntityDescriptor : DEntityDescriptor
    {
        internal DStarEntityDescriptor(string identifier, Texture2D texture, DWorld world) : base(identifier, texture, world)
        {

        }

        internal override DEntity CreateEntity()
        {
            return new DStarEntity(this);
        }
    }

    internal sealed class DStarEntity : DEntity
    {
        internal Vector2 Velocity { get; set; }
        internal float Deceleration { get; set; } = 0.1f;
        internal Vector2 Direction => this.Velocity != Vector2.Zero ? Vector2.Normalize(this.Velocity) : Vector2.Zero;

        private byte animationIndex;
        private byte animationFrameCounter;
        private Vector2 internalPosition;

        private readonly byte animationFrameDelay = 3;
        private readonly Texture2D texture;
        private readonly Rectangle[] sourceRectangles =
        [
            new(new(00, 00), new(8)),
            new(new(00, 8), new(8)),
            new(new(00, 16), new(8)),
            new(new(00, 24), new(8)),
        ];

        internal DStarEntity(DEntityDescriptor descriptor) : base(descriptor)
        {
            this.texture = descriptor.Texture;
        }

        protected override void OnInitialize()
        {
            this.internalPosition = this.Position.ToVector2();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.animationFrameCounter++;

            if (this.animationFrameCounter >= this.animationFrameDelay)
            {
                this.animationFrameCounter = 0;
                this.animationIndex = (byte)((this.animationIndex + 1) % this.sourceRectangles.Length);
            }

            if (this.Velocity != Vector2.Zero)
            {
                Vector2 norm = this.Velocity;
                norm.Normalize();
                Vector2 decelerationVector = norm * this.Deceleration;
                if (decelerationVector.Length() > this.Velocity.Length())
                {
                    this.Velocity = Vector2.Zero;
                }
                else
                {
                    this.Velocity -= decelerationVector;
                }
            }

            this.internalPosition += this.Velocity;
            this.Position = new((int)Math.Round(this.internalPosition.X), (int)Math.Round(this.internalPosition.Y));
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), this.sourceRectangles[this.animationIndex], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
