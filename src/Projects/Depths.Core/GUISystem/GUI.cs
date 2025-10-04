using Depths.Core.Interfaces.General;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.GUISystem
{
    internal abstract class GUI : IResettable
    {
        internal string Identifier { get; private set; }

        private readonly List<GUIElement> elements = [];

        internal GUI(string identifier)
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
            foreach (GUIElement element in this.elements)
            {
                if (element == null || !element.IsVisible)
                {
                    continue;
                }

                element.Draw(spriteBatch);
            }
        }

        internal void AddElement(GUIElement value)
        {
            this.elements.Add(value);
        }

        internal void RemoveElement(GUIElement value)
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
