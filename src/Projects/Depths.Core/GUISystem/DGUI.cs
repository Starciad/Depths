using Depths.Core.Interfaces.General;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.GUISystem
{
    internal abstract class DGUI : IDResettable
    {
        internal string Identifier { get; private set; }

        private readonly List<DGUIElement> elements = [];

        internal DGUI(string identifier)
        {
            this.Identifier = identifier;
        }

        internal void Initialize()
        {
            OnBuild();
        }

        internal virtual void Load()
        {
            return;
        }

        internal virtual void Unload()
        {
            return;
        }

        internal virtual void Update()
        {
            return;
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
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
            _ = this.elements.Remove(value);
        }

        protected abstract void OnBuild();

        public virtual void Reset()
        {
            return;
        }
    }
}
