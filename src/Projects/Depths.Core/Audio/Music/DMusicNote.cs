using Depths.Core.Enums.Audio;

namespace Depths.Core.Audio.Music
{
    internal readonly struct DMusicNote(DMusicNoteType note, float duration)
    {
        internal DMusicNoteType Note => note;
        internal float Duration => duration;
    }
}
