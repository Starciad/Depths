using Depths.Core.Enums.Audio;

namespace Depths.Core.Audio.Music
{
    internal readonly struct MusicNote(MusicNoteType note, float duration)
    {
        internal MusicNoteType Note => note;
        internal float Duration => duration;
    }
}
