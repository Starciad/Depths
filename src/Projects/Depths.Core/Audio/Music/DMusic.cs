using Depths.Core.Databases;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Depths.Core.Audio.Music
{
    internal sealed class DMusic(DMusicDatabase musicDatabase, DMusicNote[] noteSequence)
    {
        internal bool IsRepeating { get; set; } = false;
        internal bool IsPlaying { get; private set; } = false;

        private byte currentNoteIndex = 0;
        private float noteTimer = 0f;

        private readonly DMusicNote[] notes = noteSequence;
        private readonly DMusicDatabase musicDatabase = musicDatabase;

        internal void Play()
        {
            if (this.IsPlaying || this.notes.Length == 0)
            {
                return;
            }

            this.IsPlaying = true;
            this.currentNoteIndex = 0;
            this.noteTimer = 0f;
            PlayCurrentNote();
        }

        internal void Stop()
        {
            this.IsPlaying = false;
            this.currentNoteIndex = 0;
            this.noteTimer = 0f;
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsPlaying)
            {
                return;
            }

            this.noteTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.noteTimer <= 0)
            {
                this.currentNoteIndex++;
                PlayCurrentNote();
            }
        }

        private void PlayCurrentNote()
        {
            if (!this.IsPlaying || this.notes.Length == 0)
            {
                return;
            }

            if (this.currentNoteIndex >= this.notes.Length)
            {
                if (this.IsRepeating)
                {
                    this.currentNoteIndex = 0;
                }
                else
                {
                    Stop();
                    return;
                }
            }

            DMusicNote currentNote = this.notes[this.currentNoteIndex];
            SoundEffect noteSound = this.musicDatabase.GetMusicalNoteSoundEffect(currentNote.Note);

            if (noteSound != null)
            {
                DAudioEngine.Play(noteSound);
            }

            this.noteTimer = currentNote.Duration;
        }
    }
}
