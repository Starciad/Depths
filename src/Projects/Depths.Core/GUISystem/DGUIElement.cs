using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.GUISystem
{
    internal abstract class DGUIElement
    {
        internal bool IsVisible { get; set; }

        internal DPoint Position { get; set; }

        internal virtual void Draw(SpriteBatch spriteBatch) { return; }
    }
}
