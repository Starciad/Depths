using Depths.Core.Enums.World;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities.Common
{
    internal sealed class DRobotEntityDescriptor : DEntityDescriptor
    {
        private readonly DEntityManager entityManager;
        private readonly DGameInformation gameInformation;

        internal DRobotEntityDescriptor(string identifier, Texture2D texture, DWorld world, DEntityManager entityManager, DGameInformation gameInformation) : base(identifier, texture, world)
        {
            this.entityManager = entityManager;
            this.gameInformation = gameInformation;
        }

        internal override DEntity CreateEntity()
        {
            return new DRobotEntity(this, this.World, this.entityManager, this.gameInformation);
        }
    }

    internal sealed class DRobotEntity : DEntity
    {
        private byte playerDamage;
        private byte playerPower;

        private byte brokenBlockCount;
        private sbyte horizontalDirectionDelta;
        private byte animationIndex;

        private byte animationFrameCounter;
        private byte movementFrameCounter;
        private byte lifespanFrameCounter;

        private readonly ushort lifespanFrameDelay = 320;
        private readonly byte movementFrameDelay = 16;
        private readonly byte animationFrameDelay = 10;
        private readonly byte limitBlocksToBreak = 10;

        private readonly Texture2D texture;
        private readonly DEntityManager entityManager;
        private readonly DTilemap worldTilemap;
        private readonly DGameInformation gameInformation;

        private readonly Rectangle[] textureClipAreas =
        [
            new Rectangle(new Point(0, 0), new Point(7)), // Right Sprite [1]
            new Rectangle(new Point(0, 7), new Point(7)), // Right Sprite [2]
            new Rectangle(new Point(0, 14), new Point(7)), // Left Sprite [1]
            new Rectangle(new Point(0, 21), new Point(7)), // Left Sprite [2]
        ];

        internal DRobotEntity(DEntityDescriptor descriptor, DWorld world, DEntityManager entityManager, DGameInformation gameInformation) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.entityManager = entityManager;
            this.worldTilemap = world.Tilemap;
            this.gameInformation = gameInformation;

            OnReset();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (++this.lifespanFrameCounter >= this.lifespanFrameDelay)
            {
                this.entityManager.DestroyEntity(this);
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

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, DTilemapMath.ToGlobalPosition(this.Position).ToVector2(), GetCurrentSpriteRectangle(), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        protected override void OnReset()
        {
            this.horizontalDirectionDelta = this.gameInformation.PlayerEntity.HorizontalDirectionDelta;
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

        private void TryMoveOrBreak()
        {
            DPoint targetPosition = new(this.Position.X + this.horizontalDirectionDelta, this.Position.Y);
            DTile targetTile = this.worldTilemap.GetTile(targetPosition);

            if (targetTile == null || targetTile.Type == DTileType.Empty)
            {
                this.Position = targetPosition;
                return;
            }

            if (this.playerPower > targetTile.Resistance)
            {
                targetTile.Health -= this.playerDamage;

                if (targetTile.Health <= 0)
                {
                    this.brokenBlockCount++;

                    if (this.brokenBlockCount >= this.limitBlocksToBreak)
                    {
                        this.entityManager.DestroyEntity(this);
                    }
                }
            }
        }
    }
}
