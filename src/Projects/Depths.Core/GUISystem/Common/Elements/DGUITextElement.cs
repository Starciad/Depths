using Depths.Core.Managers;
using Depths.Core.TextRendering;

using Microsoft.Xna.Framework.Graphics;

using System.Text;

namespace Depths.Core.GUISystem.Common.Elements
{
    internal sealed class DGUITextElement : DGUIElement
    {
        internal DTextRenderOptions Options { get; private set; }

        private readonly StringBuilder stringBuilderContent;
        private readonly DTextManager textManager;

        internal DGUITextElement(DTextManager textManager, DTextRenderOptions options) : base()
        {
            this.stringBuilderContent = new();
            this.textManager = textManager;
            this.Options = options;

            this.IsVisible = true;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            this.textManager.DrawText(spriteBatch, this.stringBuilderContent.ToString(), this.Position, this.Options);
        }

        internal void SetValue(string value)
        {
            _ = this.stringBuilderContent.Clear();
            _ = this.stringBuilderContent.Append(value);
        }
    }
}
