using Microsoft.Xna.Framework;

using Depths.Core.Audio.Music;

namespace Depths.Core.Managers
{
    internal sealed class DMusicManager
    {
        private DMusic currentMusic;

        internal void SetMusic(DMusic music)
        {
            this.currentMusic = music;
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
