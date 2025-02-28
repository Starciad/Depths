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
                ["Main Theme"] = new(this, [
                    new(DMusicNoteType.C, 0.5f),
                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.A, 0.5f),
                    new(DMusicNoteType.G, 0.5f),

                    new(DMusicNoteType.C, 0.5f),
                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.A, 0.5f),
                    new(DMusicNoteType.G, 0.5f),

                    new(DMusicNoteType.C, 0.5f),
                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.A, 0.5f),
                    new(DMusicNoteType.G, 0.5f),

                    new(DMusicNoteType.C, 0.5f),
                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.A, 0.5f),
                    new(DMusicNoteType.G, 0.5f),

                    new(DMusicNoteType.D, 0.5f),
                    new(DMusicNoteType.F, 0.5f),
                    new(DMusicNoteType.B, 0.5f),
                    new(DMusicNoteType.A, 0.5f),

                    new(DMusicNoteType.D, 0.5f),
                    new(DMusicNoteType.F, 0.5f),
                    new(DMusicNoteType.B, 0.5f),
                    new(DMusicNoteType.A, 0.5f),

                    new(DMusicNoteType.D, 0.5f),
                    new(DMusicNoteType.F, 0.5f),
                    new(DMusicNoteType.B, 0.5f),
                    new(DMusicNoteType.A, 0.5f),

                    new(DMusicNoteType.D, 0.5f),
                    new(DMusicNoteType.F, 0.5f),
                    new(DMusicNoteType.B, 0.5f),
                    new(DMusicNoteType.A, 0.5f),
                ])
                {
                    IsRepeating = true,
                },

                ["Intro"] = new(this, [
                    new(DMusicNoteType.C, 0.2f),
                    new(DMusicNoteType.B, 0.2f),
                    new(DMusicNoteType.C, 0.4f),
                    new(DMusicNoteType.D, 0.5f),

                    new(DMusicNoteType.C, 0.1f),
                    new(DMusicNoteType.C, 0.2f),
                    new(DMusicNoteType.D, 0.1f),
                    new(DMusicNoteType.D, 0.2f),
                    new(DMusicNoteType.E, 0.1f),
                    new(DMusicNoteType.E, 0.2f),
                ])
                {
                    IsRepeating = true,
                },

                ["Surface"] = new(this, [
                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.B, 0.2f),
                    new(DMusicNoteType.A, 0.2f),

                    new(DMusicNoteType.E, 0.5f),
                    new(DMusicNoteType.B, 0.2f),
                    new(DMusicNoteType.A, 0.2f),

                    new(DMusicNoteType.GSharp, 0.2f),
                    new(DMusicNoteType.FSharp, 0.2f),
                ])
                {
                    IsRepeating = true,
                },

                ["Underground"] = new(this, [
                    new(DMusicNoteType.B, 0.2f),
                    new(DMusicNoteType.G, 0.2f),
                    new(DMusicNoteType.E, 0.2f),
                    new(DMusicNoteType.C, 0.2f),

                    new(DMusicNoteType.B, 0.2f),
                    new(DMusicNoteType.G, 0.2f),
                    new(DMusicNoteType.E, 0.2f),
                    new(DMusicNoteType.C, 0.2f),

                    new(DMusicNoteType.C, 0.1f),
                    new(DMusicNoteType.B, 0.1f),
                    new(DMusicNoteType.C, 0.1f),
                    new(DMusicNoteType.B, 0.1f),

                    new(DMusicNoteType.C, 0.2f),
                    new(DMusicNoteType.E, 0.2f),
                    new(DMusicNoteType.G, 0.2f),
                    new(DMusicNoteType.B, 0.2f),

                    new(DMusicNoteType.C, 0.2f),
                    new(DMusicNoteType.E, 0.2f),
                    new(DMusicNoteType.G, 0.2f),
                    new(DMusicNoteType.B, 0.2f),

                    new(DMusicNoteType.C, 0.1f),
                    new(DMusicNoteType.B, 0.1f),
                    new(DMusicNoteType.C, 0.1f),
                    new(DMusicNoteType.B, 0.1f),
                ])
                {
                    IsRepeating = true,
                },

                ["Depth"] = null,
                ["Game Over"] = null,
                ["Victory"] = null,
                ["Pause"] = null,
                ["Menu"] = null,
                ["Credits"] = null,
            };
        }

        internal DMusic GetMusicByIdentifier(string identifier)
        {
            return this.musics[identifier];
        }

        internal static string GetMusicalNoteIdentifier(DMusicNoteType note)
        {
            return note switch
            {
                DMusicNoteType.Silence => string.Empty,
                DMusicNoteType.C => "sound_note_1",
                DMusicNoteType.CSharp => "sound_note_2",
                DMusicNoteType.D => "sound_note_3",
                DMusicNoteType.DSharp => "sound_note_4",
                DMusicNoteType.E => "sound_note_5",
                DMusicNoteType.F => "sound_note_6",
                DMusicNoteType.FSharp => "sound_note_7",
                DMusicNoteType.G => "sound_note_8",
                DMusicNoteType.GSharp => "sound_note_9",
                DMusicNoteType.A => "sound_note_10",
                DMusicNoteType.ASharp => "sound_note_11",
                DMusicNoteType.B => "sound_note_12",
                _ => string.Empty,
            };
        }
    }
}
