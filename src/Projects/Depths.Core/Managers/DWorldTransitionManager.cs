using Depths.Core.Audio;
using Depths.Core.Constants;
using Depths.Core.Databases;

using Microsoft.Xna.Framework;

namespace Depths.Core.Managers
{
    internal sealed class DWorldTransitionManager
    {
        private Point targetPosition;
        private bool isTransitioning;

        private readonly int transitionSpeed = 2;
        private readonly DAssetDatabase assetDatabase;
        private readonly DCameraManager cameraManager;

        internal DWorldTransitionManager(DAssetDatabase assetDatabase, DCameraManager cameraManager)
        {
            this.assetDatabase = assetDatabase;
            this.cameraManager = cameraManager;

            this.targetPosition = cameraManager.Position.ToPoint();
        }

        internal void Update(Point playerPosition)
        {
            if (this.isTransitioning)
            {
                DAudioEngine.Play(this.assetDatabase.GetSoundEffect("sound_blip_10"));
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
            int cameraWorldY = -this.cameraManager.Position.ToPoint().Y;

            return playerPosition.X < cameraX || playerPosition.X >= cameraX + DScreenConstants.GAME_WIDTH ||
                   playerPosition.Y < cameraWorldY || playerPosition.Y >= cameraWorldY + DScreenConstants.GAME_HEIGHT;
        }

        private void StartTransition(Point playerPosition)
        {
            int newX = playerPosition.X / DScreenConstants.GAME_WIDTH * DScreenConstants.GAME_WIDTH;
            int newY = playerPosition.Y / (DScreenConstants.GAME_HEIGHT + 1) * (DScreenConstants.GAME_HEIGHT + 1);

            this.targetPosition = new Point(newX, -newY);
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
