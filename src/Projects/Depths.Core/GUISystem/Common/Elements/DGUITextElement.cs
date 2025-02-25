using Depths.Core.Enums.Fonts;
using Depths.Core.Managers;

using Microsoft.Xna.Framework.Graphics;

using System.Text;

namespace Depths.Core.GUISystem.Common.Elements
{
    internal sealed class DGUITextElement : DGUIElement
    {
        internal DFontType FontType { get; set; }
        internal int Spacing { get; set; }

        private readonly StringBuilder stringBuilderContent;
        private readonly DTextManager textManager;

        internal DGUITextElement(DTextManager textManager) : base()
        {
            this.stringBuilderContent = new();
            this.textManager = textManager;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            this.textManager.DrawText(spriteBatch, this.stringBuilderContent, this.Position, this.FontType, this.Spacing);
        }

        internal void SetValue(string value)
        {
            this.stringBuilderContent.Clear();
            this.stringBuilderContent.Append(value);
        }
    }
}
