using Depths.Core.Audio;
using Depths.Core.Databases;
using Depths.Core.Enums.General;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.World;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Depths.Core.Entities.Common
{
    internal sealed class DPlayerEntityDescriptor : DEntityDescriptor
    {
        private readonly DAssetDatabase assetDatabase;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;

        internal DPlayerEntityDescriptor(string identifier, Texture2D texture, DWorld world, DAssetDatabase assetDatabase, DInputManager inputManager, DMusicManager musicManager) : base(identifier, texture, world)
        {
            this.assetDatabase = assetDatabase;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
        }

        internal override DEntity CreateEntity()
        {
            return new DPlayerEntity(this, this.assetDatabase, this.inputManager, this.musicManager);
        }
    }

    internal sealed class DPlayerEntity : DEntity
    {
        internal bool IsDead => this.isDead;

        private DDirection direction = DDirection.Right;

        private bool isDead = false;

        private readonly byte currentHealth = 3;
        private readonly byte power = 1;
        private readonly byte attack = 1;
        private readonly byte energy = 10;

        private readonly byte maximumHealth = 3;
        private readonly byte maximumEnergy = 10;

        private byte gravityFrameCounter = 0;

        private readonly Texture2D texture;
        private readonly DTilemap tilemap;
        private readonly DAssetDatabase assetDatabase;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;

        private readonly byte gravityDelayFrames = 5;

        private readonly Rectangle[] textureClipAreas = [
            new(new(0, 0), new(7)), // Right Sprite
            new(new(0, 7), new(7)), // Left Sprite
            new(new(0, 14), new(7)), // Death Sprite
        ];

        internal DPlayerEntity(DEntityDescriptor descriptor, DAssetDatabase assetDatabase, DInputManager inputManager, DMusicManager musicManager) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.tilemap = descriptor.World.Tilemap;
            this.assetDatabase = assetDatabase;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
        }

        internal override void Update(GameTime gameTime)
        {
            if (!this.isDead && (CheckForSuffocation() || this.currentHealth == 0))
            {
                Kill();
                return;
            }

            if (TryApplyGravityStep())
            {
                return;
            }

            if (!this.isDead)
            {
                HandleInput();
            }
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, DTilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private Rectangle? GetCurrentSpriteRectangle()
        {
            if (this.isDead)
            {
                return this.textureClipAreas[2];
            }

            return this.direction switch
            {
                DDirection.Right => (Rectangle?)this.textureClipAreas[0],
                DDirection.Left => (Rectangle?)this.textureClipAreas[1],
                _ => null,
            };
        }

        private bool CheckForSuffocation()
        {
            DTile tile = this.tilemap.GetTile(this.Position);

            if ((tile.Type != DTileType.Empty && tile.Type != DTileType.Stair) ||
                tile.IsSolid)
            {
                return true;
            }

            return false;
        }

        private bool TryApplyGravityStep()
        {
            this.gravityFrameCounter++;

            if (this.gravityFrameCounter < this.gravityDelayFrames)
            {
                return false;
            }

            this.gravityFrameCounter = 0;

            Point checkPointBottom = new(this.Position.X, this.Position.Y + 1);

            DTile currentTile = this.tilemap.GetTile(this.Position);
            DTile bottomTile = this.tilemap.GetTile(checkPointBottom);

            if ((currentTile != null && currentTile.Type == DTileType.Stair) ||
                (bottomTile != null && bottomTile.Type == DTileType.Stair) ||
                IsCollidingAt(checkPointBottom))
            {
                return false;
            }

            this.Position = checkPointBottom;
            return true;
        }

        private void HandleInput()
        {
            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void HandleHorizontalMovement()
        {
            sbyte deltaX = 0;
            DDirection targetDirection = DDirection.None;

            if (this.inputManager.KeyboardState.IsKeyDown(Keys.A) &&
               !this.inputManager.PreviousKeyboardState.IsKeyDown(Keys.A))
            {
                deltaX = -1;
                targetDirection = DDirection.Left;
            }

            if (this.inputManager.KeyboardState.IsKeyDown(Keys.D) &&
               !this.inputManager.PreviousKeyboardState.IsKeyDown(Keys.D))
            {
                deltaX = 1;
                targetDirection = DDirection.Right;
            }

            if (deltaX != 0)
            {
                if (targetDirection != this.direction)
                {
                    this.direction = targetDirection;
                    return;
                }

                Point checkPointFront = new(this.Position.X + deltaX, this.Position.Y);

                if (!IsCollidingAt(checkPointFront))
                {
                    this.Position = checkPointFront;
                }
                else
                {
                    TryMineBlock(checkPointFront);
                }
            }
        }

        private void HandleVerticalMovement()
        {
            sbyte deltaY = 0;

            if (this.inputManager.KeyboardState.IsKeyDown(Keys.W) &&
               !this.inputManager.PreviousKeyboardState.IsKeyDown(Keys.W))
            {
                deltaY = -1;
            }

            if (this.inputManager.KeyboardState.IsKeyDown(Keys.S) &&
               !this.inputManager.PreviousKeyboardState.IsKeyDown(Keys.S))
            {
                deltaY = 1;
            }

            if (deltaY != 0)
            {
                Point checkPointFront = new(this.Position.X, this.Position.Y + deltaY);

                DTile currentTile = this.tilemap.GetTile(this.Position);
                DTile frontTile = this.tilemap.GetTile(checkPointFront);

                if ((currentTile != null && currentTile.Type == DTileType.Stair) ||
                    (frontTile != null && frontTile.Type == DTileType.Stair))
                {
                    if (!IsCollidingAt(checkPointFront))
                    {
                        this.Position = checkPointFront;
                        return;
                    }
                }

                if (IsCollidingAt(checkPointFront))
                {
                    TryMineBlock(checkPointFront);
                }
            }
        }

        private void TryMineBlock(Point position)
        {
            DTile tile = this.tilemap.GetTile(position);

            if (tile == null || !tile.IsDestructible)
            {
                return;
            }

            if (this.power > tile.Resistance)
            {
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_hit_6"));
                tile.Health -= this.attack;
            }
            else
            {
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_negative_2"));
            }

            if (tile.Health <= 0)
            {
                this.tilemap.SetTile(position, DTileType.Empty);
            }
        }

        private bool IsCollidingAt(Point position)
        {
            DTile tile = this.tilemap.GetTile(position);

            return tile == null || tile.IsSolid;
        }

        private void Kill()
        {
            this.isDead = true;
            this.musicManager.StopMusic();
            DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_negative_1"));
        }
    }
}
