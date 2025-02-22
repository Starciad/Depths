using Microsoft.Xna.Framework.Audio;

using Depths.Core.Audio.Music;
using Depths.Core.Enums.Audio;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DMusicDatabase
    {
        private readonly DAssetDatabase assetDatabase;
        private readonly Dictionary<string, DMusic> musics;

        internal DMusicDatabase(DAssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;

            this.musics = new()
            {
                ["theme"] = new(this, [
                    new(DMusicNoteType.C, 0.4f),
                    new(DMusicNoteType.E, 0.4f),
                    new(DMusicNoteType.G, 0.4f),
                    new(DMusicNoteType.E, 0.4f),
                    new(DMusicNoteType.C, 0.6f),

                    new(DMusicNoteType.G, 0.4f),
                    new(DMusicNoteType.A, 0.4f),
                    new(DMusicNoteType.G, 0.4f),
                    new(DMusicNoteType.F, 0.4f),
                    new(DMusicNoteType.E, 0.6f),
                ])
                {
                    IsRepeating = true,
                },
            };
        }

        internal DMusic GetMusicByIdentifier(string identifier)
        {
            return this.musics[identifier];
        }

        internal SoundEffect GetMusicalNoteSoundEffect(DMusicNoteType note)
        {
            return note switch
            {
                DMusicNoteType.Silence => null,
                DMusicNoteType.C => this.assetDatabase.GetSoundEffect("sound_note_1"),
                DMusicNoteType.CSharp => this.assetDatabase.GetSoundEffect("sound_note_2"),
                DMusicNoteType.D => this.assetDatabase.GetSoundEffect("sound_note_3"),
                DMusicNoteType.DSharp => this.assetDatabase.GetSoundEffect("sound_note_4"),
                DMusicNoteType.E => this.assetDatabase.GetSoundEffect("sound_note_5"),
                DMusicNoteType.F => this.assetDatabase.GetSoundEffect("sound_note_6"),
                DMusicNoteType.FSharp => this.assetDatabase.GetSoundEffect("sound_note_7"),
                DMusicNoteType.G => this.assetDatabase.GetSoundEffect("sound_note_8"),
                DMusicNoteType.GSharp => this.assetDatabase.GetSoundEffect("sound_note_9"),
                DMusicNoteType.A => this.assetDatabase.GetSoundEffect("sound_note_10"),
                DMusicNoteType.ASharp => this.assetDatabase.GetSoundEffect("sound_note_11"),
                DMusicNoteType.B => this.assetDatabase.GetSoundEffect("sound_note_12"),
                _ => null,
            };
        }
    }
}
