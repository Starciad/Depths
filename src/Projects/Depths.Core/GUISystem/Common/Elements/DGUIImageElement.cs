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
        internal DSize2 Size { get; private set; }
        internal Rectangle? TextureClipArea { get; set; }
        internal Texture2D Texture { get; private set; }

        internal DGUIImageElement() : base()
        {
            this.Color = Color.White;
            this.Origin = Vector2.Zero;
            this.Rotation = 0f;
            this.Scale = Vector2.One;
            this.Size = DSize2.Empty;
            this.TextureClipArea = null;
            this.Texture = null;

            this.IsVisible = true;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position.ToVector2(), this.TextureClipArea, this.Color, this.Rotation, this.Origin, this.Scale, SpriteEffects.None, 0f);
        }

        internal void SetTexture(Texture2D value)
        {
            this.Texture = value;
            this.Size = new(this.Texture.Width, this.Texture.Height);
        }
    }
}
