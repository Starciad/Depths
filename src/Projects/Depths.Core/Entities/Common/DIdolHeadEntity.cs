using Depths.Core.Constants;
using Depths.Core.Mathematics;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class DIdolHeadEntityDescriptor : DEntityDescriptor
    {
        private readonly DGameInformation gameInformation;

        internal DIdolHeadEntityDescriptor(string identifier, Texture2D texture, DWorld world, DGameInformation gameInformation) : base(identifier, texture, world)
        {
            this.gameInformation = gameInformation;
        }

        internal override DEntity CreateEntity()
        {
            return new DIdolHeadEntity(this, this.gameInformation);
        }
    }

    internal sealed class DIdolHeadEntity : DEntity
    {
        private readonly Texture2D texture;

        private readonly Rectangle[] idolHeadSourceRectangles = [
            new(new(00, 00), new(21, 30)), // [0]
            new(new(21, 00), new(21, 30)), // [1]
            new(new(42, 00), new(21, 30)), // [2]
            new(new(63, 00), new(21, 30)), // [3]
            new(new(84, 00), new(21, 30)), // [4]
        ];

        private readonly DGameInformation gameInformation;

        internal bool IsCollected { get; private set; }

        internal delegate void Collected();

        internal event Collected OnCollected;

        internal DIdolHeadEntity(DEntityDescriptor descriptor, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.gameInformation = gameInformation;

            OnReset();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.IsCollected)
            {
                return;
            }

            Rectangle idolBounds = new(this.Position.ToPoint(), new(DSpriteConstants.IDOL_HEAD_WIDTH, DSpriteConstants.IDOL_HEAD_HEIGHT));
            Rectangle playerBounds = new(DTilemapMath.ToGlobalPosition(this.gameInformation.PlayerEntity.Position).ToPoint(), new(DSpriteConstants.PLAYER_SPRITE_SIZE));

            if (idolBounds.Intersects(playerBounds))
            {
                this.IsCollected = true;
                this.IsVisible = false;

                this.OnCollected?.Invoke();
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), this.idolHeadSourceRectangles[this.gameInformation.IdolHeadSpriteIndex], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.IsCollected = false;
        }
    }
}
