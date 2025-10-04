using Depths.Core.Audio;
using Depths.Core.Constants;
using Depths.Core.Enums.InputSystem;
using Depths.Core.Enums.World;
using Depths.Core.Items;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.Entities.Common
{
    internal sealed class PlayerEntityDescriptor : EntityDescriptor
    {
        private readonly GameInformation gameInformation;
        private readonly GUIManager guiManager;
        private readonly EntityManager entityManager;
        private readonly InputManager inputManager;
        private readonly MusicManager musicManager;

        internal PlayerEntityDescriptor(string identifier, Texture2D texture, World.World world, EntityManager entityManager, GameInformation gameInformation, GUIManager guiManager, InputManager inputManager, MusicManager musicManager) : base(identifier, texture, world)
        {
            this.entityManager = entityManager;
            this.gameInformation = gameInformation;
            this.guiManager = guiManager;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
        }

        internal override Entity CreateEntity()
        {
            return new PlayerEntity(this, this.entityManager, this.gameInformation, this.guiManager, this.inputManager, this.musicManager);
        }
    }

    internal sealed class PlayerEntity : Entity
    {
        internal byte Damage { get => this.damage; set => this.damage = value; }
        internal byte BackpackSize { get => this.backpackSize; set => this.backpackSize = value; }
        internal sbyte HorizontalDirectionDelta { get => this.horizontalDirectionDelta; set => this.horizontalDirectionDelta = value; }
        internal byte Energy { get => this.energy; set => this.energy = value; }
        internal bool IsDead { get => this.isDead; set => this.isDead = value; }
        internal byte MaximumEnergy { get => this.maximumEnergy; set => this.maximumEnergy = value; }
        internal uint Money { get => this.money; set => this.money = value; }
        internal byte Power { get => this.power; set => this.power = value; }
        internal uint StairCount { get => this.stairCount; set => this.stairCount = value; }
        internal uint PlataformCount { get => this.plataformCount; set => this.plataformCount = value; }
        internal uint RobotCount { get => this.robotCount; set => this.robotCount = value; }
        internal Queue<Ore> CollectedMinerals => this.collectedMinerals;

        internal delegate void Died();
        internal delegate void EnergyDepleted();
        internal delegate void FullBackpack();
        internal delegate void CollectedOre(Ore ore);
        internal delegate void TriedMineToughBlock(Tile tile);
        internal delegate void TriedMineIndestructibleBlock(Tile tile);
        internal delegate void CollectedItemFromBox(BoxItem boxItem, uint quantityObtained);

        internal event Died OnDied;
        internal event EnergyDepleted OnEnergyDepleted;
        internal event FullBackpack OnFullBackpack;
        internal event CollectedOre OnCollectedOre;
        internal event TriedMineToughBlock OnTriedMineToughBlock;
        internal event TriedMineIndestructibleBlock OnTriedMineIndestructibleBlock;
        internal event CollectedItemFromBox OnCollectedItemFromBox;

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
        private readonly Tilemap tilemap;

        private readonly Queue<Ore> collectedMinerals = [];

        private readonly byte gravityDelayFrames = 5;

        private readonly Rectangle[] textureClipAreas = [
            new(new(0, 0), new(7)), // Right Sprite
            new(new(0, 7), new(7)), // Left Sprite
            new(new(0, 14), new(7)), // Death Sprite
        ];

        private readonly EntityManager entityManager;
        private readonly GameInformation gameInformation;
        private readonly GUIManager guiManager;
        private readonly InputManager inputManager;
        private readonly MusicManager musicManager;

        internal PlayerEntity(EntityDescriptor descriptor, EntityManager entityManager, GameInformation gameInformation, GUIManager guiManager, InputManager inputManager, MusicManager musicManager) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.tilemap = descriptor.World.Tilemap;

            this.gameInformation = gameInformation;
            this.guiManager = guiManager;
            this.entityManager = entityManager;
            this.inputManager = inputManager;
            this.musicManager = musicManager;

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

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, TilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.collectedMinerals.Clear();

            this.horizontalDirectionDelta = -1;

            this.damage = PlayerConstants.DEFAULT_STARTING_DAMAGE;
            this.backpackSize = PlayerConstants.DEFAULT_STARTING_BAG_SIZE;
            this.energy = PlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
            this.gravityFrameCounter = 0;
            this.isDead = false;
            this.maximumEnergy = PlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
            this.money = 0;
            this.power = PlayerConstants.DEFAULT_STARTING_POWER;
            this.stairCount = PlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS;
            this.plataformCount = PlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS;
            this.robotCount = PlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS;
        }

        private bool CheckForSuffocation()
        {
            Tile tile = this.tilemap.GetTile(this.Position);

            return (tile.Type != TileType.Empty && tile.Type != TileType.Platform && tile.Type != TileType.Stair) ||
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

            Tile currentTile = this.tilemap.GetTile(this.Position);
            Tile bottomTile = this.tilemap.GetTile(checkPointBottom);

            if ((currentTile != null && currentTile.Type == TileType.Stair) ||
                (bottomTile != null && bottomTile.Type == TileType.Stair) ||
                (bottomTile.Type == TileType.Platform) ||
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

            HandleGuiInput();
            HandleHorizontalMovementInput();
            HandleVerticalMovementInput();
            HandlingItemPositioningInput();
        }

        private void HandleGuiInput()
        {
            if (this.inputManager.Started(CommandType.Cancel))
            {
                this.guiManager.Open("Pause");
                return;
            }

            if (this.inputManager.Started(CommandType.GameInfos))
            {
                this.guiManager.Open("Player Information");
                return;
            }

            if (this.inputManager.Started(CommandType.TruckStore) && this.gameInformation.IsPlayerOnSurface)
            {
                this.guiManager.Open("Truck Store");
                return;
            }
        }

        private void HandleHorizontalMovementInput()
        {
            Tile currentTile = this.tilemap.GetTile(new(this.Position.X, this.Position.Y));
            Tile tileBelow = this.tilemap.GetTile(new(this.Position.X, this.Position.Y + 1));

            if (tileBelow == null || tileBelow.Type == TileType.Empty || (currentTile != null && currentTile.Type != TileType.Stair && tileBelow.Type == TileType.SpikeTrap))
            {
                return;
            }

            sbyte deltaX = 0;

            if (this.inputManager.Started(CommandType.Left))
            {
                deltaX = -1;
            }
            else if (this.inputManager.Started(CommandType.Right))
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
                AudioEngine.Play("sound_blip_9");
            }
            else
            {
                TryMineBlock(checkPointFront);
            }
        }

        private void HandleVerticalMovementInput()
        {
            sbyte deltaY = 0;

            if (this.inputManager.Started(CommandType.Up))
            {
                deltaY = -1;
            }
            else if (this.inputManager.Started(CommandType.Down))
            {
                deltaY = 1;
            }

            if (deltaY == 0)
            {
                return;
            }

            DPoint checkPointFront = new(this.Position.X, this.Position.Y + deltaY);
            Tile currentTile = this.tilemap.GetTile(this.Position);
            Tile frontTile = this.tilemap.GetTile(checkPointFront);

            if (deltaY == 1 && frontTile?.Type == TileType.Platform)
            {
                this.Position = checkPointFront;
                return;
            }

            if ((currentTile?.Type == TileType.Stair) || (frontTile?.Type == TileType.Stair))
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
            if (!this.inputManager.Started(CommandType.PlaceStair))
            {
                return;
            }

            Tile tile = this.tilemap.GetTile(this.Position);

            if (tile?.Type == TileType.Platform || (tile != null && tile.Type == TileType.Empty && this.stairCount > 0))
            {
                this.stairCount--;
                this.tilemap.SetTile(this.Position, TileType.Stair);
            }
        }

        private void HandlePlacePlataformInput()
        {
            if (!this.inputManager.Started(CommandType.PlacePlataform))
            {
                return;
            }

            DPoint targetPosition = new(this.Position.X + this.horizontalDirectionDelta, this.Position.Y + 1);
            Tile deltaTile = this.tilemap.GetTile(targetPosition);

            if ((deltaTile?.Type == TileType.Empty || deltaTile?.Type == TileType.Stair) && this.plataformCount > 0)
            {
                this.plataformCount--;
                this.tilemap.SetTile(targetPosition, TileType.Platform);
            }
        }

        private void HandlePlaceRobotInput()
        {
            if (!this.inputManager.Started(CommandType.PlaceRobot))
            {
                return;
            }

            Tile tile = this.tilemap.GetTile(this.Position);

            if (tile != null && (tile.Type == TileType.Empty || tile.Type == TileType.Stair || tile.Type == TileType.Platform) && this.robotCount > 0)
            {
                this.robotCount--;
                _ = this.entityManager.InstantiateEntity("Robot", entity =>
                {
                    entity.Position = this.Position;
                });
            }
        }

        private void TryMineBlock(DPoint position)
        {
            Tile tile = this.tilemap.GetTile(position);

            if (tile == null)
            {
                return;
            }

            if (!tile.IsDestructible)
            {
                this.OnTriedMineIndestructibleBlock?.Invoke(tile);
                return;
            }

            if (this.power >= tile.Resistance)
            {
                AudioEngine.Play("sound_hit_6");
                this.tilemap.DamageTile(position, this.damage);
            }
            else
            {
                AudioEngine.Play("sound_negative_2");
                this.OnTriedMineToughBlock?.Invoke(tile);
                return;
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

                AudioEngine.Play("sound_good_3");
            }
        }

        private bool IsCollidingAt(DPoint position)
        {
            Tile tile = this.tilemap.GetTile(position);

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
            AudioEngine.Play("sound_negative_1");

            this.OnDied?.Invoke();
        }

        internal void CollectOre(Ore ore)
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

        internal void CollectBoxItem(BoxItem boxItem)
        {
            this.OnCollectedItemFromBox?.Invoke(boxItem, boxItem.OnCollectionCallback.Invoke(boxItem, this));
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
