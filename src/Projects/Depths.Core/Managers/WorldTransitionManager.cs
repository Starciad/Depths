using Depths.Core.Audio;
using Depths.Core.Constants;
using Depths.Core.Extensions;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

namespace Depths.Core.Managers
{
    internal sealed class WorldTransitionManager
    {
        private DPoint targetPosition;
        private bool isTransitioning;

        private readonly byte transitionSpeed = 3;
        private readonly CameraManager cameraManager;

        internal WorldTransitionManager(CameraManager cameraManager)
        {
            this.cameraManager = cameraManager;
            this.targetPosition = cameraManager.Position.ToDPoint();
        }

        internal void Update(DPoint playerPosition)
        {
            if (this.isTransitioning)
            {
                AudioEngine.Play("sound_blip_10");
                MoveCameraToTarget();
                return;
            }

            if (HasPlayerMovedToNewScreen(playerPosition))
            {
                StartTransition(playerPosition);
            }
        }

        private bool HasPlayerMovedToNewScreen(DPoint playerPosition)
        {
            int cameraX = this.cameraManager.Position.ToDPoint().X;
            int cameraWorldY = -this.cameraManager.Position.ToDPoint().Y;

            return playerPosition.X < cameraX || playerPosition.X >= cameraX + ScreenConstants.GAME_WIDTH ||
                   playerPosition.Y < cameraWorldY || playerPosition.Y >= cameraWorldY + ScreenConstants.GAME_HEIGHT;
        }

        private void StartTransition(DPoint playerPosition)
        {
            int newX = playerPosition.X / ScreenConstants.GAME_WIDTH * ScreenConstants.GAME_WIDTH;
            int newY = playerPosition.Y / (ScreenConstants.GAME_HEIGHT + 1) * (ScreenConstants.GAME_HEIGHT + 1);

            this.targetPosition = new(newX, -newY);
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
