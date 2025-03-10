using Depths.Core.Managers;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.Entities.Common
{
    internal sealed class DDustEntityDescriptor : DEntityDescriptor
    {
        private readonly DEntityManager entityManager;

        internal DDustEntityDescriptor(string identifier, Texture2D texture, DWorld world, DEntityManager entityManager) : base(identifier, texture, world)
        {
            this.entityManager = entityManager;
        }

        internal override DEntity CreateEntity()
        {
            return new DDustEntity(this, this.entityManager);
        }
    }

    internal sealed class DDustEntity : DEntity
    {
        internal Vector2 Velocity { get; set; }
        internal float Deceleration { get; set; } = 0.1f;
        internal Vector2 Direction => this.Velocity != Vector2.Zero ? Vector2.Normalize(this.Velocity) : Vector2.Zero;

        private byte animationIndex;
        private byte lifespanFrameCounter;
        private byte animationFrameCounter;
        private Vector2 internalPosition;

        private readonly byte lifespanFrameDelay = 8;
        private readonly byte animationFrameDelay = 3;
        private readonly Texture2D texture;
        private readonly Rectangle[] sourceRectangles =
        [
            new(new(00, 00), new(4)),
            new(new(00, 04), new(4)),
            new(new(00, 08), new(4)),
            new(new(00, 12), new(4)),
        ];

        private readonly DEntityManager entityManager;

        internal DDustEntity(DEntityDescriptor descriptor, DEntityManager entityManager) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.entityManager = entityManager;
        }

        protected override void OnInitialize()
        {
            this.internalPosition = this.Position.ToVector2();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (++this.lifespanFrameCounter > this.lifespanFrameDelay)
            {
                this.entityManager.RemoveEntity(this);
                return;
            }

            if (++this.animationFrameCounter > this.animationFrameDelay)
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

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), this.sourceRectangles[this.animationIndex], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.lifespanFrameCounter = 0;
        }
    }
}
