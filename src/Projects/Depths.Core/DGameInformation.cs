using Depths.Core.Constants;
using Depths.Core.Entities.Common;

using Microsoft.Xna.Framework;

using System;

namespace Depths.Core
{
    internal sealed class DGameInformation
    {
        internal DPlayerEntity PlayerEntity { get; set; }
        internal DTruckEntity TruckEntity { get; set; }

        internal bool IsPlayerOnSurface { get; private set; }
        internal bool IsPlayerInUnderground { get; private set; }
        internal bool IsPlayerInDepth { get; private set; }

        internal delegate void PlayerReachedTheDepth();
        internal delegate void PlayerReachedTheUnderground();
        internal delegate void PlayerReachedTheSurface();

        internal event PlayerReachedTheDepth OnPlayerReachedTheDepth;
        internal event PlayerReachedTheUnderground OnPlayerReachedTheUnderground;
        internal event PlayerReachedTheSurface OnPlayerReachedTheSurface;

        internal DGameInformation()
        {
            this.IsPlayerOnSurface = true;
            this.IsPlayerInUnderground = false;
            this.IsPlayerInDepth = false;
        }

        internal void Update()
        {
            UpdatePlayerInfos();
        }

        private void UpdatePlayerInfos()
        {
            if (this.PlayerEntity == null)
            {
                return;
            }

            Point position = this.PlayerEntity.Position;

            // Surface
            if (CheckIfPlayerYAxisIsInRange(position.Y, new(new(0), new(DWorldConstants.TILES_PER_CHUNK_HEIGHT))))
            {
                if (!this.IsPlayerOnSurface)
                {
                    this.OnPlayerReachedTheSurface?.Invoke();
                    this.IsPlayerOnSurface = true;
                }
            }
            else
            {
                this.IsPlayerOnSurface = false;
            }

            // Underground
            if (CheckIfPlayerYAxisIsInRange(position.Y, new(new(DWorldConstants.TILES_PER_CHUNK_HEIGHT), new((DWorldConstants.WORLD_HEIGHT - 1) * DWorldConstants.TILES_PER_CHUNK_HEIGHT))))
            {
                if (!this.IsPlayerInUnderground)
                {
                    this.OnPlayerReachedTheUnderground?.Invoke();
                    this.IsPlayerInUnderground = true;
                }
            }
            else
            {
                this.IsPlayerInUnderground = false;
            }

            // Depth
            if (CheckIfPlayerYAxisIsInRange(position.Y, new(new((DWorldConstants.WORLD_HEIGHT-  1) * DWorldConstants.TILES_PER_CHUNK_HEIGHT), new(DWorldConstants.WORLD_HEIGHT * DWorldConstants.TILES_PER_CHUNK_HEIGHT))))
            {
                if (!this.IsPlayerInDepth)
                {
                    this.OnPlayerReachedTheDepth?.Invoke();
                    this.IsPlayerInDepth = true;
                }
            }
            else
            {
                this.IsPlayerInDepth = false;
            }
        }

        private static bool CheckIfPlayerYAxisIsInRange(int yPosition, Range yRange)
        {
            if (yPosition >= yRange.Start.Value && yPosition < yRange.End.Value)
            {
                return true;
            }

            return false;
        }
    }
}
