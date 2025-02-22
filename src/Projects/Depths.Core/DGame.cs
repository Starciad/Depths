using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Depths.Core.Audio;
using Depths.Core.Colors;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Managers;
using Depths.Core.Mathematics;

using System;

namespace Depths.Core
{
    public sealed class DGame : Game
    {
        private SpriteBatch spriteBatch;

        private Point position;

        private readonly DAssetDatabase assetDatabase;
        private readonly DMusicDatabase musicDatabase;

        private readonly DGraphicsManager graphicsManager;
        private readonly DInputManager inputManager;
        private readonly DTextManager textManager;
        private readonly DMusicManager musicManager;

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

            // Managers
            this.inputManager = new();
            this.textManager = new(this.assetDatabase);
            this.musicManager = new();

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
        }

        protected override void Update(GameTime gameTime)
        {
            this.inputManager.Update();
            this.musicManager.Update(gameTime);

            if (this.inputManager.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                this.position.Y--;
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_blip_1"));
            }
            else if (this.inputManager.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                this.position.X--;
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_blip_1"));
            }
            else if (this.inputManager.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                this.position.X++;
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_blip_1"));
            }
            else if (this.inputManager.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                this.position.Y++;
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_blip_1"));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(DColorPalette.LightGreen);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.assetDatabase.GetTexture("texture_other_1"), this.position.ToVector2(), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.textManager.DrawText(this.spriteBatch, string.Concat("Text", DRandomMath.Range(0, 100)), Point.Zero, DFontType.Dark, 1);
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