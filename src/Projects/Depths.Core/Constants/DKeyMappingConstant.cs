using Depths.Core.Enums.Inputs;

using Microsoft.Xna.Framework.Input;

namespace Depths.Core.Constants
{
    internal static class DKeyMappingConstant
    {
        // ==================================== //
        // Core mappings

        internal static Keys Up => Keys.W;
        internal static Keys Right => Keys.D;
        internal static Keys Down => Keys.S;
        internal static Keys Left => Keys.A;

        internal static Keys PlaceStair => Keys.K;
        internal static Keys PlacePlataform => Keys.J;
        internal static Keys PlaceRobot => Keys.H;

        internal static Keys Confirm => Keys.Enter;
        internal static Keys Cancel => Keys.Escape;

        internal static Keys GameInfos => Keys.Q;
        internal static Keys TruckStore => Keys.E;

        // ==================================== //
        // Numpad Mappings

        internal static Keys NumPad_Up => Keys.NumPad8;
        internal static Keys NumPad_Right => Keys.NumPad6;
        internal static Keys NumPad_Down => Keys.NumPad2;
        internal static Keys NumPad_Left => Keys.NumPad4;

        internal static Keys NumPad_PlaceStair => Keys.NumPad7;
        internal static Keys NumPad_PlacePlataform => Keys.NumPad9;
        internal static Keys NumPad_PlaceRobot => Keys.NumPad1;

        internal static Keys NumPad_Confirm => Keys.NumPad5;
        internal static Keys NumPad_Cancel => Keys.NumPad3;

        internal static Keys NumPad_GameInfos => Keys.NumPad0;
        internal static Keys NumPad_TruckStore => Keys.Decimal;

        // ==================================== //

        internal static Keys[] GetKeysForCommand(DCommandType commandType)
        {
            return commandType switch
            {
                DCommandType.Up => [Up, NumPad_Up],
                DCommandType.Right => [Right, NumPad_Right],
                DCommandType.Down => [Down, NumPad_Down],
                DCommandType.Left => [Left, NumPad_Left],
                DCommandType.PlaceStair => [PlaceStair, NumPad_PlaceStair],
                DCommandType.PlacePlataform => [PlacePlataform, NumPad_PlacePlataform],
                DCommandType.PlaceRobot => [PlaceRobot, NumPad_PlaceRobot],
                DCommandType.Confirm => [Confirm, NumPad_Confirm],
                DCommandType.Cancel => [Cancel, NumPad_Cancel],
                DCommandType.GameInfos => [GameInfos, NumPad_GameInfos],
                DCommandType.TruckStore => [TruckStore, NumPad_TruckStore],
                _ => [],
            };
        }
    }
}
