using Depths.Core.Audio.Music;
using Depths.Core.Databases;

using Microsoft.Xna.Framework;

namespace Depths.Core.Managers
{
    internal sealed class DMusicManager
    {
        private DMusic currentMusic;

        private readonly DMusicDatabase musicDatabase;

        internal DMusicManager(DMusicDatabase musicDatabase)
        {
            this.musicDatabase = musicDatabase;
        }

        internal void SetMusic(string identifier)
        {
            this.currentMusic = this.musicDatabase.GetMusicByIdentifier(identifier);
        }

        internal void PlayMusic()
        {
            this.currentMusic.Play();
        }

        internal void StopMusic()
        {
            this.currentMusic.Stop();
        }

        internal void Update(GameTime gameTime)
        {
            this.currentMusic?.Update(gameTime);
        }
    }
}
