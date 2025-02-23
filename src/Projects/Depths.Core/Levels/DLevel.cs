using Depths.Core.Databases;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Levels
{
    internal abstract class DLevel
    {
        protected DAssetDatabase AssetDatabase { get; private set; }

        internal DLevel(DAssetDatabase assetDatabase)
        {
            this.AssetDatabase = assetDatabase;
        }

        internal virtual void Load()
        {
            return;
        }

        internal virtual void Unload()
        {
            return;
        }

        internal virtual void Initialize()
        {
            return;
        }

        internal virtual void Update()
        {
            return;
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            return;
        }
    }
}
