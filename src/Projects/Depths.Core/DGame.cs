using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Generators;
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
        private DTruckEntity truckEntity;

        private bool transitionIsDisabled = true;
        private bool isIdolIntroCutsceneRunning = true;
        private bool isTruckIntroCutsceneRunning = false;

        private byte idolIntroCutsceneFrameCounter = 0;
        private byte truckMovementIntroCutsceneFrameCounter = 0;

        private readonly byte idolIntroCutsceneFrameDelay = 32;
        private readonly byte truckMovementIntroCutsceneFrameDelay = 1;

        private readonly Point playerSpawnPosition;
        private readonly Point truckSpawnPosition;
        private readonly Point truckLobbyPosition;
        private readonly Point cameraIdolPosition;
        private readonly Point cameraLobbyPosition;

        private readonly DAssetDatabase assetDatabase;
        private readonly DMusicDatabase musicDatabase;
        private readonly DEntityDatabase entityDatabase;
        private readonly DWorldDatabase worldDatabase;
        private readonly DGUIDatabase guiDatabase;

        private readonly DGraphicsManager graphicsManager;
        private readonly DInputManager inputManager;
        private readonly DTextManager textManager;
        private readonly DMusicManager musicManager;
        private readonly DEntityManager entityManager;
        private readonly DCameraManager cameraManager;
        private readonly DWorldTransitionManager worldTransitionManager;
        private readonly DGUIManager guiManager;

        private readonly DWorld world;
        private readonly DGameWorldGenerator gameGenerator;

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
            this.worldDatabase = new();
            this.guiDatabase = new();

            // Managers
            this.inputManager = new();
            this.textManager = new(this.assetDatabase);
            this.musicManager = new();
            this.entityManager = new(this.entityDatabase);
            this.cameraManager = new(this.graphicsManager);
            this.worldTransitionManager = new(this.assetDatabase, this.cameraManager);
            this.guiManager = new(this.guiDatabase);

            // Core
            this.world = new(this.assetDatabase);
            this.gameGenerator = new()
            {
                AssetDatabase = this.assetDatabase,
                EntityDatabase = this.entityDatabase,
                WorldDatabase = this.worldDatabase,
                EntityManager = this.entityManager,
                World = this.world,
            };

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

            // Positions
            this.playerSpawnPosition = new(this.world.Tilemap.Size.Width / 2, 4);
            this.truckSpawnPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2) + DWorldConstants.TILES_PER_CHUNK_WIDTH, 0)) + new Point(0, 10);
            this.truckLobbyPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2) + DWorldConstants.TILES_PER_CHUNK_WIDTH / 2, 0)) + new Point(-10, 10);

            this.cameraIdolPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), DWorldConstants.TILES_PER_CHUNK_HEIGHT * (DWorldConstants.WORLD_HEIGHT - 1) * -1));
            this.cameraLobbyPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), 0));
        }

        protected override void Initialize()
        {
            this.assetDatabase.Initialize(this.Content);
            this.entityDatabase.Initialize(this.world, this.assetDatabase, this.inputManager, this.musicManager);
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
            this.gameGenerator.Initialize();

            this.musicManager.SetMusic(this.musicDatabase.GetMusicByIdentifier("theme"));
            this.musicManager.PlayMusic();

            this.playerEntity = (DPlayerEntity)this.entityManager.InstantiateEntity("Player", (DEntity entity) =>
            {
                entity.Position = this.playerSpawnPosition;
            });

            this.truckEntity = (DTruckEntity)this.entityManager.InstantiateEntity("Truck", (DEntity entity) =>
            {
                entity.Position = this.truckSpawnPosition;
            });

            this.guiDatabase.Initialize(this.textManager, this.playerEntity);
            this.guiManager.Open("HUD");

            this.cameraManager.Position = this.cameraIdolPosition.ToVector2();
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateManagers(gameTime);
            UpdateCutscenes();
            UpdateTransition(gameTime);

            base.Update(gameTime);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            this.inputManager.Update();
            this.musicManager.Update(gameTime);
            this.guiManager.Update();
        }

        private void UpdateCutscenes()
        {
            if (!this.isIdolIntroCutsceneRunning && !this.isTruckIntroCutsceneRunning)
            {
                return;
            }

            // Skip
            if (this.inputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.cameraManager.Position = this.cameraLobbyPosition.ToVector2();

                this.truckEntity.Position = this.truckLobbyPosition;

                this.isIdolIntroCutsceneRunning = false;
                this.isTruckIntroCutsceneRunning = false;
            }

            UpdateIdolCutscene();
            UpdateTruckCutscene();
        }

        private void UpdateIdolCutscene()
        {
            if (!this.isIdolIntroCutsceneRunning)
            {
                return;
            }

            if (this.idolIntroCutsceneFrameCounter < this.idolIntroCutsceneFrameDelay)
            {
                this.idolIntroCutsceneFrameCounter++;
                return;
            }

            this.transitionIsDisabled = false;

            if (this.cameraManager.Position == this.cameraLobbyPosition.ToVector2())
            {
                this.isTruckIntroCutsceneRunning = true;
                this.isIdolIntroCutsceneRunning = false;
            }
        }

        private void UpdateTruckCutscene()
        {
            if (!this.isTruckIntroCutsceneRunning || this.isIdolIntroCutsceneRunning)
            {
                return;
            }

            this.truckMovementIntroCutsceneFrameCounter++;

            if (this.truckMovementIntroCutsceneFrameCounter < this.truckMovementIntroCutsceneFrameDelay)
            {
                return;
            }

            this.truckMovementIntroCutsceneFrameCounter = 0;

            if (this.truckEntity.Position.X > this.truckLobbyPosition.X)
            {
                this.truckEntity.Position += new Point(-1, 0);
            }
            else
            {
                this.isTruckIntroCutsceneRunning = false;
            }
        }

        private void UpdateTransition(GameTime gameTime)
        {
            if (this.transitionIsDisabled)
            {
                return;
            }

            if (!this.worldTransitionManager.IsTransitioning())
            {
                this.entityManager.Update(gameTime);
                this.world.Update();
            }

            this.worldTransitionManager.Update(DTilemapMath.ToGlobalPosition(this.playerEntity.Position));
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (COMPONENTS)
            // WORLD
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(this.spriteBatch);
            this.entityManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();

            // GUI
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.guiManager.Draw(this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(DColorPalette.LightGreen);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.GuiRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
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