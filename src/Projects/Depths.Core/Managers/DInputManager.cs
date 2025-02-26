using Depths.Core.Interfaces.Managers;

using Microsoft.Xna.Framework.Input;

namespace Depths.Core.Managers
{
    internal sealed class DInputManager : IDInputManager
    {
        public MouseState MouseState => this.mouseState;
        public MouseState PreviousMouseState => this.previousMouseState;
        public KeyboardState KeyboardState => this.keyboardState;
        public KeyboardState PreviousKeyboardState => this.previousKeyboardState;

        private MouseState mouseState;
        private MouseState previousMouseState;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        internal void Update()
        {
            this.previousMouseState = this.mouseState;
            this.previousKeyboardState = this.keyboardState;

            this.mouseState = Mouse.GetState();
            this.keyboardState = Keyboard.GetState();
        }

        internal bool Started(Keys key)
        {
            return this.keyboardState.IsKeyDown(key) &&
                  !this.previousKeyboardState.IsKeyDown(key);
        }

        internal bool Performed(Keys key)
        {
            return this.keyboardState.IsKeyDown(key) &&
                   this.previousKeyboardState.IsKeyDown(key);
        }

        internal bool Canceled(Keys key)
        {
            return !this.keyboardState.IsKeyDown(key) &&
                   this.previousKeyboardState.IsKeyDown(key);
        }
    }
}
