using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Extensions;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Background
{
    internal sealed class DBackground
    {
        internal RenderTarget2D RenderTarget => this.renderTarget;

        internal int WorldPixelWidth => this.worldPixelWidth;
        internal int WorldPixelHeight => this.worldPixelHeight;

        private readonly RenderTarget2D renderTarget;

        private readonly Texture2D[] surfaceTextures;
        private readonly Texture2D[] undergroundTextures;
        private readonly Texture2D[] depthTextures;

        private readonly DGraphicsManager graphicsManager;

        private readonly int chunkWidth;
        private readonly int chunkHeight;
        private readonly int worldPixelWidth;
        private readonly int worldPixelHeight;

        internal DBackground(DAssetDatabase assetDatabase, DGraphicsManager graphicsManager)
        {
            this.graphicsManager = graphicsManager;

            this.surfaceTextures =
            [
                assetDatabase.GetTexture("texture_background_1"),
                assetDatabase.GetTexture("texture_background_4"),
                assetDatabase.GetTexture("texture_background_5"),
                assetDatabase.GetTexture("texture_background_6"),
                assetDatabase.GetTexture("texture_background_7"),
            ];

            this.undergroundTextures =
            [
                assetDatabase.GetTexture("texture_background_2"),
                assetDatabase.GetTexture("texture_background_8"),
                assetDatabase.GetTexture("texture_background_9"),
            ];

            this.depthTextures =
            [
                assetDatabase.GetTexture("texture_background_3"),
            ];

            this.chunkWidth = DWorldConstants.TILES_PER_CHUNK_WIDTH;
            this.chunkHeight = DWorldConstants.TILES_PER_CHUNK_HEIGHT;
            this.worldPixelWidth = DWorldConstants.WORLD_WIDTH * this.chunkWidth * DSpriteConstants.TILE_SPRITE_SIZE;
            this.worldPixelHeight = DWorldConstants.WORLD_HEIGHT * this.chunkHeight * DSpriteConstants.TILE_SPRITE_SIZE;

            this.renderTarget = new RenderTarget2D(graphicsManager.GraphicsDevice, this.worldPixelWidth, this.worldPixelHeight);
        }

        internal void Initialize(SpriteBatch spriteBatch)
        {
            GraphicsDevice graphicsDevice = this.graphicsManager.GraphicsDevice;
            graphicsDevice.SetRenderTarget(this.renderTarget);
            graphicsDevice.Clear(DColorPalette.LightGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            for (int y = 0; y < DWorldConstants.WORLD_HEIGHT; y++)
            {
                Texture2D[] selectedTextures = y switch
                {
                    0 => this.surfaceTextures, // Undeground
                    DWorldConstants.WORLD_HEIGHT - 1 => this.depthTextures, // Underground
                    _ => this.undergroundTextures // Depth
                };

                for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
                {
                    DPoint chunkPosition = DTilemapMath.ToGlobalPosition(new(x * this.chunkWidth, y * this.chunkHeight));
                    Texture2D texture = selectedTextures.GetRandomItem();

                    spriteBatch.Draw(texture, chunkPosition.ToVector2(), Color.White);
                }
            }

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }
    }
}
