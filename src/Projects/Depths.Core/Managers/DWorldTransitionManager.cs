using Depths.Core.Constants;

using Microsoft.Xna.Framework;

namespace Depths.Core.Managers
{
    internal sealed class DWorldTransitionManager
    {
        private Point targetPosition;
        private bool isTransitioning;

        private readonly int transitionSpeed = 2;
        private readonly DCameraManager cameraManager;

        internal DWorldTransitionManager(DCameraManager cameraManager)
        {
            this.cameraManager = cameraManager;
            this.targetPosition = cameraManager.Position.ToPoint();
        }

        internal void Update(Point playerPosition)
        {
            if (this.isTransitioning)
            {
                MoveCameraToTarget();
                return;
            }

            if (HasPlayerMovedToNewScreen(playerPosition))
            {
                StartTransition(playerPosition);
            }
        }

        private bool HasPlayerMovedToNewScreen(Point playerPosition)
        {
            int cameraX = this.cameraManager.Position.ToPoint().X;
            int cameraY = this.cameraManager.Position.ToPoint().Y;

            return playerPosition.X < cameraX || playerPosition.X >= cameraX + DScreenConstants.GAME_WIDTH ||
                   playerPosition.Y < cameraY || playerPosition.Y >= cameraY + DScreenConstants.GAME_HEIGHT;
        }

        private void StartTransition(Point playerPosition)
        {
            int newX = playerPosition.X / DScreenConstants.GAME_WIDTH * DScreenConstants.GAME_WIDTH;
            int newY = playerPosition.Y / DScreenConstants.GAME_HEIGHT * DScreenConstants.GAME_HEIGHT * -1;

            this.targetPosition = new Point(newX, newY);
            this.isTransitioning = true;
        }

        private void MoveCameraToTarget()
        {
            Vector2 currentPos = this.cameraManager.Position;
            Vector2 targetPos = this.targetPosition.ToVector2();

            Vector2 direction = targetPos - currentPos;
            float distance = direction.Length();

            if (distance <= this.transitionSpeed)
            {
                this.cameraManager.Position = targetPos;
                this.isTransitioning = false;
            }
            else
            {
                direction.Normalize();
                this.cameraManager.Position += direction * this.transitionSpeed;
            }
        }

        internal bool IsTransitioning()
        {
            return this.isTransitioning;
        }
    }
}
