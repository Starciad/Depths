using Depths.Core.Audio;
using Depths.Core.Enums.World;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class RobotEntityDescriptor : EntityDescriptor
    {
        private readonly EntityManager entityManager;
        private readonly GameInformation gameInformation;

        internal RobotEntityDescriptor(string identifier, Texture2D texture, World.World world, EntityManager entityManager, GameInformation gameInformation) : base(identifier, texture, world)
        {
            this.entityManager = entityManager;
            this.gameInformation = gameInformation;
        }

        internal override Entity CreateEntity()
        {
            return new RobotEntity(this, this.World, this.entityManager, this.gameInformation);
        }
    }

    internal sealed class RobotEntity : Entity
    {
        private byte playerEnergy;
        private byte playerDamage;
        private byte playerPower;

        private byte brokenBlockCount;
        private sbyte horizontalDirectionDelta;
        private byte animationIndex;

        private byte animationFrameCounter;
        private byte movementFrameCounter;
        private byte lifespanFrameCounter;

        private readonly ushort lifespanFrameDelay = 360;
        private readonly byte movementFrameDelay = 16;
        private readonly byte animationFrameDelay = 10;

        private readonly Texture2D texture;
        private readonly EntityManager entityManager;
        private readonly Tilemap worldTilemap;
        private readonly GameInformation gameInformation;

        private readonly Rectangle[] textureClipAreas =
        [
            new Rectangle(new Point(0, 0), new Point(7)), // Right Sprite [1]
            new Rectangle(new Point(0, 7), new Point(7)), // Right Sprite [2]
            new Rectangle(new Point(0, 14), new Point(7)), // Left Sprite [1]
            new Rectangle(new Point(0, 21), new Point(7)), // Left Sprite [2]
        ];

        internal RobotEntity(EntityDescriptor descriptor, World.World world, EntityManager entityManager, GameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.entityManager = entityManager;
            this.worldTilemap = world.Tilemap;
            this.gameInformation = gameInformation;

            OnReset();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (TryCheckDeath())
            {
                return;
            }

            if (++this.movementFrameCounter >= this.movementFrameDelay)
            {
                this.movementFrameCounter = 0;
                TryMoveOrBreak();
            }

            if (++this.animationFrameCounter >= this.animationFrameDelay)
            {
                this.animationFrameCounter = 0;
                this.animationIndex = (byte)((this.animationIndex + 1) % 2);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, TilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.horizontalDirectionDelta = this.gameInformation.PlayerEntity.HorizontalDirectionDelta;

            this.playerEnergy = this.gameInformation.PlayerEntity.MaximumEnergy;
            this.playerDamage = this.gameInformation.PlayerEntity.Damage;
            this.playerPower = this.gameInformation.PlayerEntity.Power;

            this.brokenBlockCount = 0;
            this.animationIndex = 0;

            this.animationFrameCounter = 0;
            this.lifespanFrameCounter = 0;
            this.movementFrameCounter = 0;
        }

        private Rectangle? GetCurrentSpriteRectangle()
        {
            return this.horizontalDirectionDelta switch
            {
                -1 => (Rectangle?)this.textureClipAreas[this.animationIndex + 2],
                1 => (Rectangle?)this.textureClipAreas[this.animationIndex],
                _ => null,
            };
        }

        private bool TryCheckDeath()
        {
            if (this.brokenBlockCount >= this.playerEnergy || ++this.lifespanFrameCounter >= this.lifespanFrameDelay)
            {
                AudioEngine.Play("sound_hit_5");
                this.entityManager.DestroyEntity(this);
                return true;
            }

            return false;
        }

        private void TryMoveOrBreak()
        {
            DPoint targetPosition = new(this.Position.X + this.horizontalDirectionDelta, this.Position.Y);
            Tile targetTile = this.worldTilemap.GetTile(targetPosition);

            if (targetTile == null ||
                targetTile.Type == TileType.Empty ||
                targetTile.Type == TileType.Platform ||
                targetTile.Type == TileType.Stair ||
                targetTile.Type == TileType.SpikeTrap)
            {
                this.Position = targetPosition;
                AudioEngine.Play("sound_blip_4");
                return;
            }

            if (!targetTile.IsDestructible)
            {
                this.entityManager.DestroyEntity(this);
                return;
            }

            if (this.playerPower >= targetTile.Resistance)
            {
                this.worldTilemap.DamageTile(targetPosition, this.playerDamage);
                AudioEngine.Play("sound_blip_7");

                if (targetTile.Health <= 0)
                {
                    AudioEngine.Play("sound_blip_3");
                    this.brokenBlockCount++;
                }
            }
        }
    }
}
