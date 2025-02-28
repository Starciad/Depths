using Depths.Core.Audio;
using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities.Common;
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
        private readonly DPoint idolHeadSpawnPosition;

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
            this.musicManager = new(this.musicDatabase);
            this.entityManager = new(this.entityDatabase);
            this.cameraManager = new(this.graphicsManager);
            this.worldTransitionManager = new(this.cameraManager);
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
            this.cameraLobbyPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), 0));
            this.cameraIdolPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), DWorldConstants.TILES_PER_CHUNK_HEIGHT * (DWorldConstants.WORLD_HEIGHT - 1) * -1));
            this.idolHeadSpawnPosition = DTilemapMath.ToGlobalPosition(new(DWorldConstants.TILES_PER_CHUNK_WIDTH * (DWorldConstants.WORLD_WIDTH / 2), DWorldConstants.TILES_PER_CHUNK_HEIGHT * (DWorldConstants.WORLD_HEIGHT - 1))) + new DPoint((DScreenConstants.GAME_WIDTH / 2) - 10, 12);
        }

        protected override void Initialize()
        {
            this.assetDatabase.Initialize(this.Content);
            this.entityDatabase.Initialize(this.world, this.entityManager, this.gameInformation, this.guiManager, this.inputManager, this.musicManager);
            this.graphicsManager.Initialize();
            this.worldDatabase.Initialize(this.assetDatabase);
            this.textManager.Initialize();

            DAudioEngine.Initialize(this.assetDatabase);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this.guiDatabase.Initialize(this.assetDatabase, this.gameInformation, this.guiManager, this.inputManager, this.musicManager, this.textManager);

            BuildEvents();

            this.guiManager.Open("Main Menu");
        }

        private void BuildEvents()
        {
            this.gameInformation.OnPlayerReachedTheSurface += () =>
            {
                this.musicManager.SetMusic("Surface");
                this.musicManager.PlayMusic();
            };

            this.gameInformation.OnPlayerReachedTheUnderground += () =>
            {
                this.musicManager.SetMusic("Underground");
                this.musicManager.PlayMusic();
            };

            this.gameInformation.OnPlayerReachedTheDepth += () =>
            {
                this.musicManager.SetMusic("Depth");
                this.musicManager.PlayMusic();
            };

            this.gameInformation.OnGameStarted += () =>
            {
                this.entityManager.Reset();
                this.gameInformation.Reset();
                this.world.Reset();

                this.musicManager.StopMusic();

                // ======================= //

                this.gameInformation.SetPlayerEntity((DPlayerEntity)this.entityManager.InstantiateEntity("Player", null));
                this.gameInformation.SetTruckEntity((DTruckEntity)this.entityManager.InstantiateEntity("Truck Store", null));
                this.gameInformation.SetIdolHeadEntity((DIdolHeadEntity)this.entityManager.InstantiateEntity("Idol Head", null));

                // ======================= //

                this.gameGenerator.Initialize();

                // ======================= //

                this.musicManager.SetMusic("Intro");
                this.musicManager.PlayMusic();

                this.cameraManager.Position = this.cameraIdolPosition.ToVector2();

                this.gameInformation.PlayerEntity.IsVisible = false;

                this.gameInformation.PlayerEntity.Position = this.playerSpawnPosition;
                this.gameInformation.TruckEntity.Position = this.truckSpawnPosition;
                this.gameInformation.IdolHeadEntity.Position = this.idolHeadSpawnPosition;

                // ======================= //

                this.gameInformation.PlayerEntity.Position = this.playerSpawnPosition;
                this.gameInformation.TruckEntity.Position = this.truckSpawnPosition;

                this.gameInformation.PlayerEntity.IsVisible = false;
                this.gameInformation.PlayerEntity.HorizontalDirectionDelta = -1;

                this.guiManager.Open("HUD");
            };

            this.gameInformation.OnGameOver += () =>
            {
                this.gameInformation.IsWorldActive = false;

                this.musicManager.SetMusic("Game Over");
                this.musicManager.PlayMusic();

                this.guiManager.Open("Game Over");
            };

            this.gameInformation.OnGameWon += () =>
            {
                this.gameInformation.IsWorldActive = false;

                this.musicManager.SetMusic("Victory");
                this.musicManager.PlayMusic();

                this.guiManager.Open("Victory");
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameInformation.IsGameFocused)
            {
                base.Update(gameTime);
                return;
            }

            this.guiManager.Update();
            this.inputManager.Update();
            this.gameInformation.Update();
            this.musicManager.Update(gameTime);

            if (this.gameInformation.IsGamePaused)
            {
                base.Update(gameTime);
                return;
            }

            if (this.gameInformation.IsWorldActive && !this.gameInformation.IsGameCrucialMenuOpen && !this.worldTransitionManager.IsTransitioning())
            {
                this.entityManager.Update(gameTime);
                this.world.Update();
            }

            UpdateCutscenes();
            UpdateTransition();

            base.Update(gameTime);
        }

        private void UpdateCutscenes()
        {
            if (!this.gameInformation.IsGameStarted || (!this.gameInformation.IsIdolCutsceneRunning && !this.gameInformation.IsTruckCutsceneRunning && !this.gameInformation.IsPlayerCutsceneRunning))
            {
                return;
            }

            // Skip Cutscene
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

                this.musicManager.StopMusic();
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

            if (++this.idolCutsceneFrameCounter < this.idolCutsceneFrameDelay)
            {
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

            if (++this.truckMovementCutsceneFrameCounter < this.truckMovementCutsceneFrameDelay)
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

            if (++this.playerMovementCutsceneFrameCounter < this.playerMovementCutsceneFrameDelay)
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
                this.musicManager.StopMusic();
                this.gameInformation.IsPlayerCutsceneRunning = false;
            }
        }

        private void UpdateTransition()
        {
            if (this.gameInformation.TransitionIsDisabled || this.gameInformation.PlayerEntity == null)
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

            if (this.gameInformation.IsWorldVisible)
            {
                this.world.Draw(this.spriteBatch);
                this.entityManager.Draw(gameTime, this.spriteBatch);
            }

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

#if DESKTOP
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
#endif
    }
}