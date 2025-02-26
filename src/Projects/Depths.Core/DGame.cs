using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.General;
using Depths.Core.Generators;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core
{
    public sealed class DGame : Game
    {
        private SpriteBatch spriteBatch;

        private byte idolCutsceneFrameCounter = 0;
        private byte truckMovementCutsceneFrameCounter = 0;
        private byte playerMovementCutsceneFrameCounter = 0;

        private readonly byte idolCutsceneFrameDelay = 32;
        private readonly byte truckMovementCutsceneFrameDelay = 1;
        private readonly byte playerMovementCutsceneFrameDelay = 8;

        private readonly DPoint playerSpawnPosition;
        private readonly DPoint playerLobbyPosition;
        private readonly DPoint truckSpawnPosition;
        private readonly DPoint truckLobbyPosition;
        private readonly DPoint cameraIdolPosition;
        private readonly DPoint cameraLobbyPosition;

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

        private readonly DGameInformation gameInformation;

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

            // Infos
            this.gameInformation = new();

            // Core
            this.world = new(this.assetDatabase, this.gameInformation);
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
            this.playerLobbyPosition = new((this.world.Tilemap.Size.Width / 2) - 3, 4);
            this.truckSpawnPosition = DTilemapMath.ToGlobalPosition(new((DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2)) + DWorldConstants.TILES_PER_CHUNK_WIDTH, 0)) + new DPoint(0, 10);
            this.truckLobbyPosition = DTilemapMath.ToGlobalPosition(new((DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2)) + (DWorldConstants.TILES_PER_CHUNK_WIDTH / 2), 0)) + new DPoint(-6, 10);

            this.cameraIdolPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), DWorldConstants.TILES_PER_CHUNK_HEIGHT * (DWorldConstants.WORLD_HEIGHT - 1) * -1));
            this.cameraLobbyPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), 0));
        }

        protected override void Initialize()
        {
            this.assetDatabase.Initialize(this.Content);
            this.entityDatabase.Initialize(this.world, this.assetDatabase, this.entityManager, this.inputManager, this.musicManager, this.gameInformation);
            this.graphicsManager.Initialize();
            this.worldDatabase.Initialize(this.assetDatabase);
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

            this.gameInformation.PlayerEntity = (DPlayerEntity)this.entityManager.InstantiateEntity("Player", (DEntity entity) =>
            {
                entity.Position = this.playerSpawnPosition;
            });

            this.gameInformation.TruckEntity = (DTruckEntity)this.entityManager.InstantiateEntity("Truck", (DEntity entity) =>
            {
                entity.Position = this.truckSpawnPosition;
            });

            this.guiDatabase.Initialize(this.assetDatabase, this.textManager, this.guiManager, this.gameInformation);
            this.guiManager.Open("HUD");

            this.gameInformation.PlayerEntity.IsVisible = false;
            this.gameInformation.PlayerEntity.HorizontalDirectionDelta = -1;

            this.cameraManager.Position = this.cameraIdolPosition.ToVector2();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameInformation.IsGameFocused)
            {
                goto BASE_UPDATE;
            }

            this.gameInformation.Update();
            this.inputManager.Update();
            this.guiManager.Update();
            this.musicManager.Update(gameTime);

            if (this.gameInformation.IsGamePaused)
            {
                goto BASE_UPDATE;
            }

            if (this.gameInformation.IsGameCrucialMenuOpen || !this.worldTransitionManager.IsTransitioning())
            {
                this.entityManager.Update(gameTime);
                this.world.Update();
            }

            UpdateCutscenes();
            UpdateTransition();

        BASE_UPDATE:
            ;
            base.Update(gameTime);
        }

        private void UpdateCutscenes()
        {
            if (!this.gameInformation.IsIdolCutsceneRunning && !this.gameInformation.IsTruckCutsceneRunning && !this.gameInformation.IsPlayerCutsceneRunning)
            {
                return;
            }

            // Skip
            if (this.inputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.cameraManager.Position = this.cameraLobbyPosition.ToVector2();

                this.gameInformation.PlayerEntity.IsVisible = true;

                this.gameInformation.PlayerEntity.Position = this.playerLobbyPosition;
                this.gameInformation.TruckEntity.Position = this.truckLobbyPosition;

                this.gameInformation.IsIdolCutsceneRunning = false;
                this.gameInformation.IsTruckCutsceneRunning = false;
                this.gameInformation.IsPlayerCutsceneRunning = false;

                this.gameInformation.TransitionIsDisabled = false;
            }

            UpdateIdolCutscene();
            UpdateTruckCutscene();
            UpdatePlayerCutscene();
        }

        private void UpdateIdolCutscene()
        {
            if (!this.gameInformation.IsIdolCutsceneRunning)
            {
                return;
            }

            if (this.idolCutsceneFrameCounter < this.idolCutsceneFrameDelay)
            {
                this.idolCutsceneFrameCounter++;
                return;
            }

            this.gameInformation.TransitionIsDisabled = false;

            if (this.cameraManager.Position == this.cameraLobbyPosition.ToVector2())
            {
                this.gameInformation.IsTruckCutsceneRunning = true;
                this.gameInformation.IsIdolCutsceneRunning = false;
            }
        }

        private void UpdateTruckCutscene()
        {
            if (!this.gameInformation.IsTruckCutsceneRunning || this.gameInformation.IsIdolCutsceneRunning)
            {
                return;
            }

            this.truckMovementCutsceneFrameCounter++;

            if (this.truckMovementCutsceneFrameCounter < this.truckMovementCutsceneFrameDelay)
            {
                return;
            }

            this.truckMovementCutsceneFrameCounter = 0;

            if (this.gameInformation.TruckEntity.Position.X > this.truckLobbyPosition.X)
            {
                this.gameInformation.TruckEntity.Position += new DPoint(-1, 0);
            }
            else
            {
                this.gameInformation.PlayerEntity.IsVisible = true;
                this.gameInformation.IsPlayerCutsceneRunning = true;
                this.gameInformation.IsTruckCutsceneRunning = false;
            }
        }

        private void UpdatePlayerCutscene()
        {
            if (!this.gameInformation.IsPlayerCutsceneRunning || this.gameInformation.IsTruckCutsceneRunning || this.gameInformation.IsIdolCutsceneRunning)
            {
                return;
            }

            this.playerMovementCutsceneFrameCounter++;

            if (this.playerMovementCutsceneFrameCounter < this.playerMovementCutsceneFrameDelay)
            {
                return;
            }

            this.playerMovementCutsceneFrameCounter = 0;

            if (this.gameInformation.PlayerEntity.Position.X > this.playerLobbyPosition.X)
            {
                this.gameInformation.PlayerEntity.Position += new DPoint(-1, 0);
            }
            else
            {
                this.gameInformation.IsPlayerCutsceneRunning = false;
            }
        }

        private void UpdateTransition()
        {
            if (this.gameInformation.TransitionIsDisabled)
            {
                return;
            }

            this.worldTransitionManager.Update(DTilemapMath.ToGlobalPosition(this.gameInformation.PlayerEntity.Position));
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

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            this.gameInformation.IsGameFocused = true;
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            this.gameInformation.IsGameFocused = false;
        }

        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}