using Depths.Core.Background;
using Depths.Core.Constants;
using Depths.Core.IO;
using Depths.Core.Managers;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;

namespace Depths.Core.Recorder
{
    internal sealed class DRecorder
    {
        private readonly DBackground background;
        private readonly DEntityManager entityManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly SpriteBatch spriteBatch;
        private readonly DWorld world;

        private readonly string directoryPath;

        internal DRecorder(DBackground background, DEntityManager entityManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, DWorld world)
        {
            this.background = background;
            this.entityManager = entityManager;
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.world = world;

            this.directoryPath = Path.Combine(DDirectory.Local, "Recorders");
            Directory.CreateDirectory(this.directoryPath);
        }

        internal void CaptureWorld()
        {
            string filename = $"{DGameConstants.TITLE.ToLower()}-world-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
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
            this.world.Draw(this.spriteBatch);
            this.entityManager.Draw(this.spriteBatch);

            this.spriteBatch.End();

            this.graphicsDevice.SetRenderTarget(null);
        }
    }
}
