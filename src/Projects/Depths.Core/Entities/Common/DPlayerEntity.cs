using Depths.Core.Enums.General;
using Depths.Core.Enums.Tiles;
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
        private readonly DInputManager inputManager;

        internal DPlayerEntityDescriptor(string identifier, Texture2D texture, DWorld world, DInputManager inputManager) : base(identifier, texture, world)
        {
            this.inputManager = inputManager;
        }

        internal override DEntity CreateEntity()
        {
            return new DPlayerEntity(this, this.inputManager);
        }
    }

    internal sealed class DPlayerEntity : DEntity
    {
        private DDirection direction = DDirection.Right;

        private int gravityFrameCounter = 0;

        private readonly Texture2D texture;
        private readonly DTilemap tilemap;
        private readonly DInputManager inputManager;

        private readonly int gravityDelayFrames = 5;

        private readonly Rectangle[] textureClipAreas = [
            new(new(0, 0), new(7)),
            new(new(0, 7), new(7)),
        ];

        internal DPlayerEntity(DEntityDescriptor descriptor, DInputManager inputManager) : base(descriptor)
        {
            this.texture = descriptor.Texture;
            this.tilemap = descriptor.World.Tilemap;
            this.inputManager = inputManager;
        }

        internal override void Update(GameTime gameTime)
        {
            ApplyGravityStep();
            HandleInput();
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, DTilemapMath.ToGlobalPosition(this.Position).ToVector2(), this.direction == DDirection.Right ? this.textureClipAreas[0] : this.textureClipAreas[1], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private void ApplyGravityStep()
        {
            this.gravityFrameCounter++;

            if (this.gravityFrameCounter < this.gravityDelayFrames)
            {
                return;
            }

            this.gravityFrameCounter = 0;

            Point checkPointBottom = new(this.Position.X, this.Position.Y + 1);

            if (IsCollidingAt(checkPointBottom))
            {
                return;
            }

            this.Position = checkPointBottom;
        }

        private void HandleInput()
        {
            HandleHorizontalMovement();
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
            }
        }

        private bool IsCollidingAt(Point position)
        {
            DTile tile = this.tilemap.GetTile(position);

            return tile == null || tile.Type != DTileType.Empty;
        }
    }
}
