using Depths.Core.Extensions;
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
        private Rectangle currentIdolHeadSourceRectangle;

        private readonly Texture2D texture;
        private readonly Rectangle[] idolHeadSourceRectangles = [
            new(new(0, 0), new(21, 30)), // [0]
        ];

        private readonly DGameInformation gameInformation;

        internal DIdolHeadEntity(DEntityDescriptor descriptor, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.gameInformation = gameInformation;

            OnReset();
        }

        protected override void OnUpdate(GameTime gameTime)
        {

        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position.ToVector2(), this.currentIdolHeadSourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.currentIdolHeadSourceRectangle = this.idolHeadSourceRectangles.GetRandomItem();
        }
    }
}
