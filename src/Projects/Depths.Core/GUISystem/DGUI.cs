using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.GUISystem
{
    internal abstract class DGUI
    {
        internal bool IsVisible { get; set; }
        internal bool IsActive { get; set; }

        private readonly List<DGUIElement> elements = [];

        internal void Initialize()
        {
            OnBuild();
        }

        internal void Update()
        {
            if (!this.IsActive)
            {
                return;
            }

            foreach (DGUIElement element in this.elements)
            {
                if (element == null || !element.IsActive)
                {
                    continue;
                }

                element.Update();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            foreach (DGUIElement element in this.elements)
            {
                if (element == null || !element.IsVisible)
                {
                    continue;
                }

                element.Draw(spriteBatch);
            }
        }

        internal void AddElement(DGUIElement value)
        {
            this.elements.Add(value);
        }

        internal void RemoveElement(DGUIElement value)
        {
            this.elements.Remove(value);
        }

        protected abstract void OnBuild();
    }
}
