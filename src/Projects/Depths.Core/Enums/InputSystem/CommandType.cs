namespace Depths.Core.Enums.InputSystem
{
    internal enum CommandType : byte
    {
        None = 0,

        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4,

        PlaceStair = 5,
        PlacePlataform = 6,
        PlaceRobot = 7,

        Confirm = 8,
        Cancel = 9,

        GameInfos = 10,
        TruckStore = 11,
    }
}
