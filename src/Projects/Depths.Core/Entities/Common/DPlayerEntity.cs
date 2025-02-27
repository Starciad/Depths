using Depths.Core.Audio;
using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.Entities.Common
{
    internal sealed class DPlayerEntityDescriptor : DEntityDescriptor
    {
        private readonly DAssetDatabase assetDatabase;
        private readonly DEntityManager entityManager;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;
        private readonly DGameInformation gameInformation;

        internal DPlayerEntityDescriptor(string identifier, Texture2D texture, DWorld world, DAssetDatabase assetDatabase, DEntityManager entityManager, DInputManager inputManager, DMusicManager musicManager, DGameInformation gameInformation) : base(identifier, texture, world)
        {
            this.assetDatabase = assetDatabase;
            this.entityManager = entityManager;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
            this.gameInformation = gameInformation;
        }

        internal override DEntity CreateEntity()
        {
            return new DPlayerEntity(this, this.assetDatabase, this.entityManager, this.inputManager, this.musicManager, this.gameInformation);
        }
    }

    internal sealed class DPlayerEntity : DEntity
    {
        public byte Damage { get => this.damage; set => this.damage = value; }
        public byte BackpackSize { get => this.backpackSize; set => this.backpackSize = value; }
        internal sbyte HorizontalDirectionDelta { get => this.horizontalDirectionDelta; set => this.horizontalDirectionDelta = value; }
        public byte Energy { get => this.energy; set => this.energy = value; }
        internal bool IsDead => this.isDead;
        public byte MaximumEnergy { get => this.maximumEnergy; set => this.maximumEnergy = value; }
        public uint Money { get => this.money; set => this.money = value; }
        public byte Power { get => this.power; set => this.power = value; }
        public uint StairCount { get => this.stairCount; set => this.stairCount = value; }
        public uint PlataformCount { get => this.plataformCount; set => this.plataformCount = value; }
        public uint RobotCount { get => this.robotCount; set => this.robotCount = value; }
        public Queue<DOre> CollectedMinerals => this.collectedMinerals;

        internal delegate void Died();
        internal delegate void EnergyDepleted();
        internal delegate void FullBackpack();
        internal delegate void CollectedOre(DOre ore);

        internal event Died OnDied;
        internal event EnergyDepleted OnEnergyDepleted;
        internal event FullBackpack OnFullBackpack;
        internal event CollectedOre OnCollectedOre;

        private sbyte horizontalDirectionDelta;

        private byte damage;
        private byte backpackSize;
        private byte energy;
        private byte gravityFrameCounter;
        private bool isDead;
        private byte maximumEnergy;
        private uint money;
        private byte power;
        private uint stairCount;
        private uint plataformCount;
        private uint robotCount;

        private readonly Texture2D texture;
        private readonly DTilemap tilemap;
        private readonly DAssetDatabase assetDatabase;
        private readonly DEntityManager entityManager;
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

        internal DPlayerEntity(DEntityDescriptor descriptor, DAssetDatabase assetDatabase, DEntityManager entityManager, DInputManager inputManager, DMusicManager musicManager, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.tilemap = descriptor.World.Tilemap;
            this.assetDatabase = assetDatabase;
            this.entityManager = entityManager;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
            this.gameInformation = gameInformation;

            OnReset();
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
            spriteBatch.Draw(this.texture, DTilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.collectedMinerals.Clear();

            this.horizontalDirectionDelta = -1;

            this.damage = DPlayerConstants.DEFAULT_STARTING_DAMAGE;
            this.backpackSize = DPlayerConstants.DEFAULT_STARTING_BAG_SIZE;
            this.energy = DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
            this.gravityFrameCounter = 0;
            this.isDead = false;
            this.maximumEnergy = DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
            this.money = 0;
            this.power = DPlayerConstants.DEFAULT_STARTING_POWER;
            this.stairCount = DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS;
            this.plataformCount = DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS;
            this.robotCount = DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS;
        }

        private bool CheckForSuffocation()
        {
            DTile tile = this.tilemap.GetTile(this.Position);

            return (tile.Type != DTileType.Empty && tile.Type != DTileType.Platform && tile.Type != DTileType.Stair) ||
                tile.IsSolid;
        }

        private bool TryApplyGravityStep()
        {
            if (++this.gravityFrameCounter < this.gravityDelayFrames)
            {
                return false;
            }

            this.gravityFrameCounter = 0;

            DPoint checkPointBottom = new(this.Position.X, this.Position.Y + 1);

            DTile currentTile = this.tilemap.GetTile(this.Position);
            DTile bottomTile = this.tilemap.GetTile(checkPointBottom);

            if ((currentTile != null && currentTile.Type == DTileType.Stair) ||
                (bottomTile != null && bottomTile.Type == DTileType.Stair) ||
                (bottomTile != null && bottomTile.Type == DTileType.Platform) ||
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

            HandleHorizontalMovementInput();
            HandleVerticalMovementInput();
            HandlingItemPositioningInput();
        }

        private void HandleHorizontalMovementInput()
        {
            DTile tileBelow = this.tilemap.GetTile(new DPoint(this.Position.X, this.Position.Y + 1));

            if (tileBelow == null || tileBelow.Type == DTileType.Empty)
            {
                return;
            }

            sbyte deltaX = 0;
            if (this.inputManager.Started(DKeyMappingConstant.Left))
            {
                deltaX = -1;
            }
            else if (this.inputManager.Started(DKeyMappingConstant.Right))
            {
                deltaX = 1;
            }

            if (deltaX == 0)
            {
                return;
            }

            if (deltaX != this.horizontalDirectionDelta)
            {
                this.horizontalDirectionDelta = deltaX;
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

        private void HandleVerticalMovementInput()
        {
            sbyte deltaY = 0;

            if (this.inputManager.Started(DKeyMappingConstant.Up))
            {
                deltaY = -1;
            }
            else if (this.inputManager.Started(DKeyMappingConstant.Down))
            {
                deltaY = 1;
            }

            if (deltaY == 0)
            {
                return;
            }

            DPoint checkPointFront = new(this.Position.X, this.Position.Y + deltaY);
            DTile currentTile = this.tilemap.GetTile(this.Position);
            DTile frontTile = this.tilemap.GetTile(checkPointFront);

            if (deltaY == 1 && frontTile?.Type == DTileType.Platform)
            {
                this.Position = checkPointFront;
                return;
            }

            if ((currentTile?.Type == DTileType.Stair) || (frontTile?.Type == DTileType.Stair))
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

        private void HandlingItemPositioningInput()
        {
            HandlePlaceStairInput();
            HandlePlacePlataformInput();
            HandlePlaceRobotInput();
        }

        private void HandlePlaceStairInput()
        {
            if (!this.inputManager.Started(DKeyMappingConstant.PlaceStair))
            {
                return;
            }

            DTile tile = this.tilemap.GetTile(this.Position);

            if (tile?.Type == DTileType.Platform || (tile != null && tile.Type == DTileType.Empty && this.stairCount > 0))
            {
                this.stairCount--;
                this.tilemap.SetTile(this.Position, DTileType.Stair);
            }
        }

        private void HandlePlacePlataformInput()
        {
            if (!this.inputManager.Started(DKeyMappingConstant.PlacePlataform))
            {
                return;
            }

            DPoint targetPosition = new(this.Position.X + this.horizontalDirectionDelta, this.Position.Y + 1);
            DTile deltaTile = this.tilemap.GetTile(targetPosition);

            if ((deltaTile?.Type == DTileType.Empty || deltaTile?.Type == DTileType.Stair) && this.plataformCount > 0)
            {
                this.plataformCount--;
                this.tilemap.SetTile(targetPosition, DTileType.Platform);
            }
        }

        private void HandlePlaceRobotInput()
        {
            if (!this.inputManager.Started(DKeyMappingConstant.PlaceRobot))
            {
                return;
            }

            DTile tile = this.tilemap.GetTile(this.Position);

            if (tile != null && (tile.Type == DTileType.Empty || tile.Type == DTileType.Stair || tile.Type == DTileType.Platform) && this.robotCount > 0)
            {
                this.robotCount--;
                _ = this.entityManager.InstantiateEntity("Robot", (DEntity entity) =>
                {
                    entity.Position = this.Position;
                });
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
                switch (--this.energy)
                {
                    case 0:
                        Kill();
                        break;

                    case 1:
                        this.OnEnergyDepleted?.Invoke();
                        break;

                    default:
                        break;
                }

                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_good_3"));
            }
        }

        private bool IsCollidingAt(DPoint position)
        {
            DTile tile = this.tilemap.GetTile(position);

            return tile == null || tile.IsSolid;
        }

        private void Kill()
        {
            if (this.isDead)
            {
                return;
            }

            this.isDead = true;
            this.musicManager.StopMusic();
            DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_negative_1"));

            this.OnDied?.Invoke();
        }

        internal void CollectOre(DOre ore)
        {
            if (this.collectedMinerals.Count < this.backpackSize)
            {
                this.OnCollectedOre?.Invoke(ore);
                this.collectedMinerals.Enqueue(ore);
            }
            else
            {
                this.OnFullBackpack?.Invoke();
            }
        }

        // ==================================================== //
        // Utilities

        private Rectangle? GetCurrentSpriteRectangle()
        {
            return this.isDead
                ? this.textureClipAreas[2]
                : this.horizontalDirectionDelta switch
                {
                    1 => (Rectangle?)this.textureClipAreas[0],
                    -1 => (Rectangle?)this.textureClipAreas[1],
                    _ => null,
                };
        }
    }
}
