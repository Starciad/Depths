using Depths.Core.Interfaces.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class DEntity : IDPoolableObject
    {
        internal DEntityDescriptor Descriptor { get; private set; }
        internal Point Position { get; set; }

        internal bool IsActive { get; set; }
        internal bool IsVisible { get; set; }

        internal DEntity(DEntityDescriptor descriptor)
        {
            this.Descriptor = descriptor;

            this.IsActive = true;
            this.IsVisible = true;
        }

        internal void Initialize()
        {
            OnInitialize();
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            OnUpdate(gameTime);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            OnDraw(gameTime, spriteBatch);
        }

        internal void Destroy()
        {
            OnDestroy();
        }

        public void Reset()
        {
            OnReset();
        }

        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate(GameTime gameTime) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
        protected virtual void OnDestroy() { return; }
        protected virtual void OnReset() { return; }
    }
}