using Depths.Core.Interfaces.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class DEntity : IDPoolableObject
    {
        internal DEntityDescriptor Descriptor { get; private set; }
        internal Point Position { get; set; }

        internal DEntity(DEntityDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        internal virtual void Initialize() { return; }
        internal virtual void Update(GameTime gameTime) { return; }
        internal virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
        internal virtual void Destroy() { return; }
        public virtual void Reset() { return; }
    }
}