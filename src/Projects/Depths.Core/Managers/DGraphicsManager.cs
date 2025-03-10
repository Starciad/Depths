using Depths.Core.Constants;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Managers
{
    internal sealed class DGraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
    {
        internal GraphicsDeviceManager GraphicsDeviceManager => this.graphicsDeviceManager;
        internal GraphicsDevice GraphicsDevice => this.graphicsDeviceManager.GraphicsDevice;
        internal Viewport Viewport => this.GraphicsDevice.Viewport;
        internal RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        internal RenderTarget2D WorldRenderTarget => this.worldRenderTarget;
        internal RenderTarget2D GuiRenderTarget => this.guiRenderTarget;

        private RenderTarget2D screenRenderTarget;
        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D guiRenderTarget;

        private readonly GraphicsDeviceManager graphicsDeviceManager = graphicsDeviceManager;

        internal void Initialize()
        {
            this.screenRenderTarget = new(this.GraphicsDevice, DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT);
            this.worldRenderTarget = new(this.GraphicsDevice, DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT);
            this.guiRenderTarget = new(this.GraphicsDevice, DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT);
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
