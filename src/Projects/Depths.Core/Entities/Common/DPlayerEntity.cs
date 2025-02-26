using Depths.Core.Audio;
using Depths.Core.Databases;
using Depths.Core.Enums.General;
using Depths.Core.Enums.World.Tiles;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace Depths.Core.Entities.Common
{
    internal sealed class DPlayerEntityDescriptor : DEntityDescriptor
    {
        private readonly DAssetDatabase assetDatabase;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;
        private readonly DGameInformation gameInformation;

        internal DPlayerEntityDescriptor(string identifier, Texture2D texture, DWorld world, DAssetDatabase assetDatabase, DInputManager inputManager, DMusicManager musicManager, DGameInformation gameInformation) : base(identifier, texture, world)
        {
            this.assetDatabase = assetDatabase;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
            this.gameInformation = gameInformation;
        }

        internal override DEntity CreateEntity()
        {
            return new DPlayerEntity(this, this.assetDatabase, this.inputManager, this.musicManager, this.gameInformation);
        }
    }

    internal sealed class DPlayerEntity : DEntity
    {
        public byte Damage { get => this.damage; set => this.damage = value; }
        public byte BackpackSize { get => this.backpackSize; set => this.backpackSize = value; }
        internal DDirection Direction { get => this.direction; set => this.direction = value; }
        public byte Energy { get => this.energy; set => this.energy = value; }
        internal bool IsDead => this.isDead;
        public byte MaximumEnergy { get => this.maximumEnergy; set => this.maximumEnergy = value; }
        public uint Money { get => this.money; set => this.money = value; }
        public byte Power { get => this.power; set => this.power = value; }
        public uint StairCount { get => this.stairCount; set => this.stairCount = value; }
        public Queue<DOre> CollectedMinerals => this.collectedMinerals;

        internal delegate void FullBackpack();
        internal delegate void CollectedOre(DOre ore);

        internal event FullBackpack OnFullBackpack;
        internal event CollectedOre OnCollectedOre;

        private DDirection direction = DDirection.Right;

        private byte damage = 1;
        private byte backpackSize = 10;
        private byte energy = 30;
        private byte gravityFrameCounter = 0;
        private bool isDead;
        private byte maximumEnergy = 30;
        private uint money = 0;
        private byte power = 1;
        private uint stairCount = 10;

        private readonly Texture2D texture;
        private readonly DTilemap tilemap;
        private readonly DAssetDatabase assetDatabase;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;
        private readonly DGameInformation gameInformation;

        private readonly Queue<DOre> collectedMinerals = [];

        private readonly byte gravityDelayFrames = 5;

        private readonly Rectangle[] textureClipAreas = [
            new(new(0, 0), new(7)), // Right Sprite
            new(new(0, 7), new(7)), // Left Sprite
            new(new(0, 14), new(7)), // Death Sprite
        ];

        internal DPlayerEntity(DEntityDescriptor descriptor, DAssetDatabase assetDatabase, DInputManager inputManager, DMusicManager musicManager, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.tilemap = descriptor.World.Tilemap;
            this.assetDatabase = assetDatabase;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
            this.gameInformation = gameInformation;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!this.isDead && CheckForSuffocation())
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

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            spriteBatch.Draw(this.texture, DTilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private Rectangle? GetCurrentSpriteRectangle()
        {
            return this.isDead
                ? this.textureClipAreas[2]
                : this.direction switch
                {
                    DDirection.Right => (Rectangle?)this.textureClipAreas[0],
                    DDirection.Left => (Rectangle?)this.textureClipAreas[1],
                    _ => null,
                };
        }

        private bool CheckForSuffocation()
        {
            DTile tile = this.tilemap.GetTile(this.Position);

            return (tile.Type != DTileType.Empty && tile.Type != DTileType.Stair) ||
                tile.IsSolid;
        }

        private bool TryApplyGravityStep()
        {
            this.gravityFrameCounter++;

            if (this.gravityFrameCounter < this.gravityDelayFrames)
            {
                return false;
            }

            this.gravityFrameCounter = 0;

            DPoint checkPointBottom = new(this.Position.X, this.Position.Y + 1);

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
            if (this.gameInformation.IsCutsceneRunning)
            {
                return;
            }

            HandleHorizontalMovement();
            HandleVerticalMovement();
            HandlePlaceLadder();
        }

        private void HandleHorizontalMovement()
        {
            DTile tileBelow = this.tilemap.GetTile(new(this.Position.X, this.Position.Y + 1));

            if (tileBelow == null || tileBelow.Type == DTileType.Empty)
            {
                return;
            }

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

                DPoint checkPointFront = new(this.Position.X + deltaX, this.Position.Y);

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
                DPoint checkPointFront = new(this.Position.X, this.Position.Y + deltaY);

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

        private void HandlePlaceLadder()
        {
            if (this.inputManager.KeyboardState.IsKeyDown(Keys.K) &&
               !this.inputManager.PreviousKeyboardState.IsKeyDown(Keys.K))
            {
                DTile tile = this.tilemap.GetTile(this.Position);

                if (tile == null || tile.Type != DTileType.Empty || this.stairCount <= 0)
                {
                    return;
                }

                this.stairCount--;
                this.tilemap.SetTile(this.Position, DTileType.Stair);
            }
        }

        private void TryMineBlock(DPoint position)
        {
            DTile tile = this.tilemap.GetTile(position);

            if (tile == null || !tile.IsDestructible)
            {
                return;
            }

            if (this.power > tile.Resistance)
            {
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_hit_6"));
                tile.Health -= this.damage;
            }
            else
            {
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_negative_2"));
            }

            if (tile.Health <= 0)
            {
                this.energy--;

                if (this.energy == 0)
                {
                    Kill();
                }

                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_good_3"));

                GetTileRewards(tile);
            }
        }

        private void GetTileRewards(DTile tile)
        {
            switch (tile.Type)
            {
                case DTileType.Ore:
                    if (this.collectedMinerals.Count < this.backpackSize)
                    {
                        this.collectedMinerals.Enqueue(tile.Ore);
                        this.OnCollectedOre?.Invoke(tile.Ore);
                    }
                    else
                    {
                        this.OnFullBackpack?.Invoke();
                    }

                    break;

                case DTileType.Box:
                    switch (DRandomMath.Range(0, 1))
                    {
                        case 0:
                            this.money += (uint)DRandomMath.Range(1, 5);
                            break;

                        case 1:
                            this.stairCount += (uint)DRandomMath.Range(3, 6);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        private bool IsCollidingAt(DPoint position)
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
