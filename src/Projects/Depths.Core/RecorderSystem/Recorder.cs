#if DEBUG
using Depths.Core.BackgroundSystem;
using Depths.Core.Constants;
using Depths.Core.IO;
using Depths.Core.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;

namespace Depths.Core.RecorderSystem
{
    internal sealed class Recorder
    {
        private readonly Background background;
        private readonly CameraManager cameraManager;
        private readonly EntityManager entityManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly SpriteBatch spriteBatch;
        private readonly World.World world;

        private readonly string directoryPath;

        internal Recorder(Background background, CameraManager cameraManager, EntityManager entityManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, World.World world)
        {
            this.background = background;
            this.cameraManager = cameraManager;
            this.entityManager = entityManager;
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.world = world;

            this.directoryPath = Path.Combine(DDirectory.Local, "Recorders");
            _ = Directory.CreateDirectory(this.directoryPath);
        }

        internal void CaptureWorld()
        {
            string filename = $"{GameConstants.TITLE.ToLower()}-world-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            string filepath = Path.Combine(this.directoryPath, filename);

            using RenderTarget2D screenshot = new(this.graphicsDevice, this.background.WorldPixelWidth, this.background.WorldPixelHeight);
            RenderSceneToTarget(screenshot);

            using FileStream fileStream = new(filepath, FileMode.Create, FileAccess.Write, FileShare.Read);
            screenshot.SaveAsPng(fileStream, this.background.WorldPixelWidth, this.background.WorldPixelHeight);
        }

        private void RenderSceneToTarget(RenderTarget2D target)
        {
            this.graphicsDevice.SetRenderTarget(target);
            this.graphicsDevice.Clear(Color.Transparent);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            this.spriteBatch.Draw(this.background.RenderTarget, Vector2.Zero, Color.White);
            this.world.DrawAll(this.spriteBatch);
            this.entityManager.Draw(this.spriteBatch);

            this.spriteBatch.End();

            this.graphicsDevice.SetRenderTarget(null);
        }
    }
}
#endif
