using Depths.Core.Audio.Music;
using Depths.Core.Enums.Audio;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class MusicDatabase
    {
        private readonly AssetDatabase assetDatabase;
        private readonly Dictionary<string, Music> musics;

        internal MusicDatabase(AssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;

            this.musics = new()
            {
                ["Main Menu"] = new(this, [
                    new(MusicNoteType.C, 0.4f),
                    new(MusicNoteType.E, 0.4f),
                    new(MusicNoteType.A, 0.4f),
                    new(MusicNoteType.G, 0.3f),

                    new(MusicNoteType.C, 0.3f),
                    new(MusicNoteType.E, 0.3f),
                    new(MusicNoteType.A, 0.3f),
                    new(MusicNoteType.G, 0.3f),

                    new(MusicNoteType.D, 0.4f),
                    new(MusicNoteType.F, 0.4f),
                    new(MusicNoteType.B, 0.4f),
                    new(MusicNoteType.A, 0.3f),

                    new(MusicNoteType.D, 0.3f),
                    new(MusicNoteType.F, 0.3f),
                    new(MusicNoteType.B, 0.3f),
                    new(MusicNoteType.A, 0.3f),

                    new(MusicNoteType.G, 0.2f),
                    new(MusicNoteType.A, 0.2f),
                    new(MusicNoteType.B, 0.2f),
                    new(MusicNoteType.C, 0.5f),
                ])
                {
                    IsRepeating = true,
                },

                ["Intro"] = new(this, [
                    new(MusicNoteType.C, 0.2f),
                    new(MusicNoteType.B, 0.2f),
                    new(MusicNoteType.C, 0.4f),
                    new(MusicNoteType.D, 0.5f),

                    new(MusicNoteType.E, 0.3f),
                    new(MusicNoteType.F, 0.3f),
                    new(MusicNoteType.G, 0.3f),

                    new(MusicNoteType.C, 0.1f),
                    new(MusicNoteType.B, 0.2f),
                    new(MusicNoteType.C, 0.1f),
                    new(MusicNoteType.D, 0.2f),
                    new(MusicNoteType.E, 0.3f),
                ])
                {
                    IsRepeating = true,
                },

                ["Surface"] = new(this, [
                    new(MusicNoteType.E, 0.5f),
                    new(MusicNoteType.B, 0.3f),
                    new(MusicNoteType.A, 0.3f),

                    new(MusicNoteType.GSharp, 0.3f),
                    new(MusicNoteType.FSharp, 0.3f),
                    new(MusicNoteType.E, 0.5f),

                    new(MusicNoteType.G, 0.3f),
                    new(MusicNoteType.A, 0.3f),
                ])
                {
                    IsRepeating = true,
                },

                ["Underground"] = new(this, [
                    new(MusicNoteType.B, 0.3f),
                    new(MusicNoteType.G, 0.3f),
                    new(MusicNoteType.E, 0.3f),
                    new(MusicNoteType.C, 0.3f),

                    new(MusicNoteType.C, 0.2f),
                    new(MusicNoteType.E, 0.2f),
                    new(MusicNoteType.G, 0.2f),
                    new(MusicNoteType.B, 0.2f),

                    new(MusicNoteType.C, 0.1f),
                    new(MusicNoteType.B, 0.1f),
                    new(MusicNoteType.A, 0.1f),
                    new(MusicNoteType.G, 0.1f),
                ])
                {
                    IsRepeating = true,
                },

                ["Depth"] = new(this, [
                    new(MusicNoteType.F, 0.5f),
                    new(MusicNoteType.D, 0.4f),
                    new(MusicNoteType.A, 0.4f),

                    new(MusicNoteType.GSharp, 0.4f),
                    new(MusicNoteType.E, 0.3f),
                    new(MusicNoteType.B, 0.3f),

                    new(MusicNoteType.FSharp, 0.4f),
                    new(MusicNoteType.D, 0.3f),
                ])
                {
                    IsRepeating = true,
                },

                ["Game Over"] = new(this, [
                    new(MusicNoteType.C, 0.4f),
                    new(MusicNoteType.A, 0.3f),
                    new(MusicNoteType.F, 0.3f),

                    new(MusicNoteType.D, 0.5f),
                    new(MusicNoteType.GSharp, 0.5f),

                    new(MusicNoteType.C, 0.2f),
                    new(MusicNoteType.B, 0.2f),
                    new(MusicNoteType.A, 0.2f),

                    new(MusicNoteType.F, 0.3f),
                    new(MusicNoteType.D, 0.3f),
                ])
                {
                    IsRepeating = false,
                },

                ["Victory"] = new(this, [
                    new(MusicNoteType.C, 0.3f),
                    new(MusicNoteType.E, 0.3f),
                    new(MusicNoteType.G, 0.3f),

                    new(MusicNoteType.B, 0.5f),
                    new(MusicNoteType.G, 0.3f),
                    new(MusicNoteType.E, 0.3f),

                    new(MusicNoteType.A, 0.4f),
                    new(MusicNoteType.F, 0.3f),
                    new(MusicNoteType.C, 0.3f),

                    new(MusicNoteType.C, 0.5f),
                ])
                {
                    IsRepeating = false,
                },

                ["Credits"] = new(this, [
                    new(MusicNoteType.D, 0.5f),
                    new(MusicNoteType.F, 0.5f),
                    new(MusicNoteType.A, 0.5f),

                    new(MusicNoteType.C, 0.5f),
                    new(MusicNoteType.E, 0.5f),

                    new(MusicNoteType.B, 0.3f),
                    new(MusicNoteType.G, 0.3f),
                    new(MusicNoteType.F, 0.3f),

                    new(MusicNoteType.D, 0.5f),
                ])
                {
                    IsRepeating = true,
                },
            };
        }

        internal Music GetMusicByIdentifier(string identifier)
        {
            return this.musics[identifier];
        }

        internal static string GetMusicalNoteIdentifier(MusicNoteType note)
        {
            return note switch
            {
                MusicNoteType.Silence => string.Empty,
                MusicNoteType.C => "sound_note_1",
                MusicNoteType.CSharp => "sound_note_2",
                MusicNoteType.D => "sound_note_3",
                MusicNoteType.DSharp => "sound_note_4",
                MusicNoteType.E => "sound_note_5",
                MusicNoteType.F => "sound_note_6",
                MusicNoteType.FSharp => "sound_note_7",
                MusicNoteType.G => "sound_note_8",
                MusicNoteType.GSharp => "sound_note_9",
                MusicNoteType.A => "sound_note_10",
                MusicNoteType.ASharp => "sound_note_11",
                MusicNoteType.B => "sound_note_12",
                _ => string.Empty,
            };
        }
    }
}
