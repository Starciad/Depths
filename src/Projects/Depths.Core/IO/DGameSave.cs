#if DESKTOP
using Depths.Core.Constants;
using Depths.Core.IO.GameSave;
using Depths.Core.Mathematics;

using MessagePack;

using System.IO;

namespace Depths.Core.IO
{
    internal static class DGameSave
    {
        private static string SaveFilePath => Path.Combine(DDirectory.Local, "save.bin");

        internal static void Serialize(DGameInformation gameInformation)
        {
            using FileStream fileStream = new(SaveFilePath, FileMode.Create, FileAccess.Write, FileShare.Write);
            
            DGameSaveFile gameSaveFile = new()
            {
                PlayerInfo = new(gameInformation.PlayerEntity),
            };

            fileStream.Write(MessagePackSerializer.Serialize(gameSaveFile));
        }

        internal static void Deserialize(DGameInformation gameInformation)
        {
            DGameSaveFile gameSaveFile = MessagePackSerializer.Deserialize<DGameSaveFile>(File.ReadAllBytes(SaveFilePath));

            gameInformation.PlayerEntity = null;
            gameInformation.TruckEntity = null;
            gameInformation.IdolHeadEntity = null;
            gameInformation.IdolHeadSpriteIndex = (byte)DRandomMath.Range(0, DSpriteConstants.IDOL_HEAD_VARIATIONS - 1);

            gameInformation.IsPlayerOnSurface = true;
            gameInformation.IsPlayerInUnderground = false;
            gameInformation.IsPlayerInDepth = false;

            gameInformation.IsGameCrucialMenuOpen = false;

#if DESKTOP
            gameInformation.IsGameFocused = true;
#endif

            gameInformation.IsIdolCutsceneRunning = true;
            gameInformation.IsTruckCutsceneRunning = false;
            gameInformation.IsPlayerCutsceneRunning = false;

            gameInformation.TransitionIsDisabled = true;

            gameInformation.IsWorldActive = true;
            gameInformation.IsWorldVisible = true;
        }
    }
}
#endif
