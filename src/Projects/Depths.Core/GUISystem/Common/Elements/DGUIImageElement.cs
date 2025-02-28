using Depths.Core.Extensions;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.GUISystem.Common.Elements
{
    internal sealed class DGUIImageElement : DGUIElement
    {
        internal Color Color { get; set; }
        internal Vector2 Origin { get; set; }
        internal float Rotation { get; set; }
        internal Vector2 Scale { get; set; }
        internal Texture2D Texture
        {
            get
            {
                return texture;
            }

            set
            {
                this.texture = value;
                this.textureSize = this.Texture.GetSize();
            }
        }
        internal Rectangle? TextureClipArea { get; set; }
        internal DSize2 TextureSize => this.textureSize;

        private Texture2D texture;
        private DSize2 textureSize;

        internal DGUIImageElement() : base()
        {
            this.Color = Color.White;
            this.Origin = Vector2.Zero;
            this.Rotation = 0f;
            this.Scale = Vector2.One;
            this.textureSize = DSize2.Empty;
            this.TextureClipArea = null;
            this.texture = null;

            this.IsVisible = true;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position.ToVector2(), this.TextureClipArea, this.Color, this.Rotation, this.Origin, this.Scale, SpriteEffects.None, 0f);
        }
    }
}
