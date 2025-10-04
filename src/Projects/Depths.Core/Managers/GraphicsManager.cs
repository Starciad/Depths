using Depths.Core.Constants;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Managers
{
    internal sealed class GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
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
            this.screenRenderTarget = new(this.GraphicsDevice, ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
            this.worldRenderTarget = new(this.GraphicsDevice, ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
            this.guiRenderTarget = new(this.GraphicsDevice, ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
        }

        internal Vector2 GetScreenScaleFactor()
        {
            return new(
                this.graphicsDeviceManager.PreferredBackBufferWidth / (float)ScreenConstants.SCREEN_WIDTH,
                this.graphicsDeviceManager.PreferredBackBufferHeight / (float)ScreenConstants.SCREEN_HEIGHT
            );
        }
    }
}
