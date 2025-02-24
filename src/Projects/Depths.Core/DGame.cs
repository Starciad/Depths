using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities.Common;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core
{
    public sealed class DGame : Game
    {
        private SpriteBatch spriteBatch;

        private DPlayerEntity playerEntity;

        private readonly DAssetDatabase assetDatabase;
        private readonly DMusicDatabase musicDatabase;
        private readonly DEntityDatabase entityDatabase;

        private readonly DGraphicsManager graphicsManager;
        private readonly DInputManager inputManager;
        private readonly DTextManager textManager;
        private readonly DMusicManager musicManager;
        private readonly DEntityManager entityManager;
        private readonly DCameraManager cameraManager;
        private readonly DWorldTransitionManager worldTransitionManager;

        private readonly DWorld world;

        public DGame()
        {
            // Graphics
            this.graphicsManager = new(new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = DScreenConstants.SCREEN_WIDTH,
                PreferredBackBufferHeight = DScreenConstants.SCREEN_HEIGHT,
                GraphicsProfile = GraphicsProfile.Reach,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true,
            });

            // Databases
            this.assetDatabase = new();
            this.musicDatabase = new(this.assetDatabase);
            this.entityDatabase = new(this.assetDatabase);

            // Managers
            this.inputManager = new();
            this.textManager = new(this.assetDatabase);
            this.musicManager = new();
            this.entityManager = new(this.entityDatabase);
            this.cameraManager = new(this.graphicsManager);
            this.worldTransitionManager = new(this.assetDatabase, this.cameraManager);

            // Core
            this.world = new(this.assetDatabase);

            // Initialize Content
            this.Content.RootDirectory = DDirectoryConstants.ASSETS;

            // Configure the game's window
            this.Window.AllowUserResizing = true;
            this.Window.IsBorderless = false;
            this.Window.Title = DGameConstants.GetTitleAndVersionString();

            // Configure game settings
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / DVideoConstants.GAME_FRAME_RATE);
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            this.assetDatabase.Initialize(this.Content);
            this.entityDatabase.Initialize(this.world, this.inputManager);
            this.graphicsManager.Initialize();
            this.textManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this.musicManager.SetMusic(this.musicDatabase.GetMusicByIdentifier("theme"));
            this.musicManager.PlayMusic();

            this.playerEntity = (DPlayerEntity)this.entityManager.InstantiateEntity("Player", null);
        }

        protected override void Update(GameTime gameTime)
        {
            this.inputManager.Update();
            this.musicManager.Update(gameTime);

            if (!this.worldTransitionManager.IsTransitioning())
            {
                this.entityManager.Update(gameTime);
            }

            this.worldTransitionManager.Update(DTilemapMath.ToGlobalPosition(this.playerEntity.Position));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(DColorPalette.LightGreen);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(this.spriteBatch);
            this.entityManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (FINAL)
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(DColorPalette.LightGreen);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, DScreenConstants.SCALE_FACTOR * this.graphicsManager.GetScreenScaleFactor(), SpriteEffects.None, 0f);
            this.spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}