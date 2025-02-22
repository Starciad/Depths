using Microsoft.Xna.Framework.Input;

namespace Depths.Core.Interfaces.Managers
{
    internal interface IDInputManager
    {
        MouseState MouseState { get; }
        MouseState PreviousMouseState { get; }
        KeyboardState KeyboardState { get; }
        KeyboardState PreviousKeyboardState { get; }
    }
}
