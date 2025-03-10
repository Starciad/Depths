using Depths.Core.Constants;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.Entities.Common
{
    internal sealed class DIdolHeadEntityDescriptor : DEntityDescriptor
    {
        private readonly DEntityManager entityManager;
        private readonly DGameInformation gameInformation;

        internal DIdolHeadEntityDescriptor(string identifier, Texture2D texture, DWorld world, DEntityManager entityManager, DGameInformation gameInformation) : base(identifier, texture, world)
        {
            this.entityManager = entityManager;
            this.gameInformation = gameInformation;
        }

        internal override DEntity CreateEntity()
        {
            return new DIdolHeadEntity(this, this.entityManager, this.gameInformation);
        }
    }

    internal sealed class DIdolHeadEntity : DEntity
    {
        internal bool IsCollected { get; private set; }

        internal delegate void Collected();

        internal event Collected OnCollected;

        private byte victoryFrameCounter;

        private readonly Texture2D texture;
        private readonly int totalStars = 8;
        private readonly byte victoryFrameDelay = 32;

        private readonly DEntityManager entityManager;
        private readonly DGameInformation gameInformation;

        internal DIdolHeadEntity(DEntityDescriptor descriptor, DEntityManager entityManager, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.entityManager = entityManager;
            this.gameInformation = gameInformation;

            OnReset();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.IsCollected)
            {
                if (++this.victoryFrameCounter > this.victoryFrameDelay)
                {
                    this.OnCollected?.Invoke();
                }
            }
            else
            {
                Rectangle idolBounds = new(this.Position.ToPoint(), new(DSpriteConstants.IDOL_HEAD_WIDTH, DSpriteConstants.IDOL_HEAD_HEIGHT));
                Rectangle playerBounds = new(DTilemapMath.ToGlobalPosition(this.gameInformation.PlayerEntity.Position).ToPoint(), new(DSpriteConstants.PLAYER_SPRITE_SIZE));

                if (idolBounds.Intersects(playerBounds))
                {
                    this.IsCollected = true;
                    this.IsVisible = false;

                    InstantiateStars();
                }
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), DSpriteConstants.IDOL_SOURCE_RECTANGLES[this.gameInformation.IdolHeadSpriteIndex], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.IsCollected = false;
            this.IsVisible = true;
            this.victoryFrameCounter = 0;
        }

        private void InstantiateStars()
        {
            float angleIncrement = MathHelper.TwoPi / this.totalStars;
            const float initialSpeed = 2f;

            for (int i = 0; i < this.totalStars; i++)
            {
                float angle = i * angleIncrement;
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * initialSpeed;

                _ = this.entityManager.InstantiateEntity("Star", (DEntity entity) =>
                {
                    DStarEntity starEntity = entity as DStarEntity;

                    starEntity.Position = new(this.Position.X + (DSpriteConstants.IDOL_HEAD_WIDTH / 2), this.Position.Y + (DSpriteConstants.IDOL_HEAD_HEIGHT / 2));
                    starEntity.Velocity = velocity;
                });
            }
        }
    }
}
