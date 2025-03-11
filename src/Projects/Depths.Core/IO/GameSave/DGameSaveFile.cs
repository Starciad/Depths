#if DESKTOP
using MessagePack;

namespace Depths.Core.IO.GameSave
{
    [MessagePackObject]
    public sealed class DGameSaveFile
    {
        [Key(0)] public DPlayerInfo PlayerInfo { get; set; }

        public DGameSaveFile()
        {
            this.PlayerInfo = new();
        }
    }
}
#endif