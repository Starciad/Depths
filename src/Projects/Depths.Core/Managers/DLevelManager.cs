using Depths.Core.Databases;
using Depths.Core.Levels;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Managers
{
    internal sealed class DLevelManager
    {
        private DLevel levelLoaded = null;

        private readonly DLevelDatabase levelDatabase;

        internal DLevelManager(DLevelDatabase levelDatabase)
        {
            this.levelDatabase = levelDatabase;
        }

        internal void LoadLevel(string identifier)
        {
            UnloadLevel();

            this.levelLoaded = this.levelDatabase.GetLevelByIdentifier(identifier);
            this.levelLoaded.Load();
            this.levelLoaded.Initialize();
        }

        private void UnloadLevel()
        {
            if (this.levelLoaded == null)
            {
                return;
            }

            this.levelLoaded.Unload();
            this.levelLoaded = null;
        }

        internal void Update()
        {
            this.levelLoaded.Update();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.levelLoaded.Draw(spriteBatch);
        }
    }
}
