using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.GUISystem
{
    internal abstract class DGUIElement
    {
        internal bool IsVisible { get; set; }
        internal bool IsActive { get; set; }

        internal Point Position { get; set; }

        internal virtual void Initialize() { return; }
        internal virtual void Update() { return; }
        internal virtual void Draw(SpriteBatch spriteBatch) { return; }
    }
}
