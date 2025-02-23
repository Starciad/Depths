using Depths.Core.Constants;
using Depths.Core.Interfaces.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Managers
{
    internal sealed class DGraphicsManager(GraphicsDeviceManager graphicsDeviceManager) : IDGraphicsManager
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this.graphicsDeviceManager;
        public GraphicsDevice GraphicsDevice => this.graphicsDeviceManager.GraphicsDevice;
        public Viewport Viewport => this.GraphicsDevice.Viewport;
        internal RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;

        private RenderTarget2D screenRenderTarget;

        private readonly GraphicsDeviceManager graphicsDeviceManager = graphicsDeviceManager;

        internal void Initialize()
        {
            this.screenRenderTarget = new(this.GraphicsDevice, DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT);
        }

        internal Vector2 GetScreenScaleFactor()
        {
            return new(
                this.graphicsDeviceManager.PreferredBackBufferWidth / (float)DScreenConstants.SCREEN_WIDTH,
                this.graphicsDeviceManager.PreferredBackBufferHeight / (float)DScreenConstants.SCREEN_HEIGHT
            );
        }
    }
}
