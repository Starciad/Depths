using Depths.Core.Constants;
using Depths.Core.Entities.Common;
using Depths.Core.Interfaces.General;
using Depths.Core.Mathematics;
using Depths.Core.Mathematics.Primitives;

using System;

namespace Depths.Core
{
    internal sealed class DGameInformation : IDResettable
    {
        internal DPlayerEntity PlayerEntity { get; private set; }
        internal DTruckEntity TruckEntity { get; private set; }
        internal DIdolHeadEntity IdolHeadEntity { get; private set; }
        internal byte IdolHeadSpriteIndex { get; private set; }

        internal bool TransitionIsDisabled { get; set; }

        internal bool IsCutsceneRunning => this.IsIdolCutsceneRunning || this.IsTruckCutsceneRunning || this.IsPlayerCutsceneRunning;
        internal bool IsIdolCutsceneRunning { get; set; }
        internal bool IsTruckCutsceneRunning { get; set; }
        internal bool IsPlayerCutsceneRunning { get; set; }

        internal bool IsGameStarted { get; set; }
        internal bool IsGameCrucialMenuOpen { get; set; }

#if DESKTOP
        internal bool IsGameFocused { get; set; }
#endif

        internal bool IsWorldActive { get; set; }
        internal bool IsWorldVisible { get; set; }

        internal bool IsPlayerOnSurface { get; private set; }
        internal bool IsPlayerInUnderground { get; private set; }
        internal bool IsPlayerInDepth { get; private set; }

        internal delegate void GameStarted();
        internal delegate void GameOver();
        internal delegate void GameWon();
        internal delegate void PlayerReachedTheDepth();
        internal delegate void PlayerReachedTheUnderground();
        internal delegate void PlayerReachedTheSurface();

        internal event GameStarted OnGameStarted;
        internal event GameOver OnGameOver;
        internal event GameWon OnGameWon;
        internal event PlayerReachedTheDepth OnPlayerReachedTheDepth;
        internal event PlayerReachedTheUnderground OnPlayerReachedTheUnderground;
        internal event PlayerReachedTheSurface OnPlayerReachedTheSurface;

        internal void SetPlayerEntity(DPlayerEntity playerEntity)
        {
            this.PlayerEntity = playerEntity;

            this.PlayerEntity.OnDied += () =>
            {
                this.OnGameOver?.Invoke();
            };
        }

        internal void SetTruckEntity(DTruckEntity truckEntity)
        {
            this.TruckEntity = truckEntity;
        }

        internal void SetIdolHeadEntity(DIdolHeadEntity idolHeadEntity)
        {
            this.IdolHeadEntity = idolHeadEntity;

            this.IdolHeadEntity.OnCollected += () =>
            {
                this.OnGameWon?.Invoke();
            };
        }

        internal void Start()
        {
            if (this.IsGameStarted)
            {
                return;
            }

            this.IsGameStarted = true;
            this.OnGameStarted?.Invoke();
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

            DPoint position = this.PlayerEntity.Position;

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
            if (CheckIfPlayerYAxisIsInRange(position.Y, new(new((DWorldConstants.WORLD_HEIGHT - 1) * DWorldConstants.TILES_PER_CHUNK_HEIGHT), new(DWorldConstants.WORLD_HEIGHT * DWorldConstants.TILES_PER_CHUNK_HEIGHT))))
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
            return yPosition >= yRange.Start.Value && yPosition < yRange.End.Value;
        }

        public void Reset()
        {
            this.PlayerEntity = null;
            this.TruckEntity = null;
            this.IdolHeadEntity = null;
            this.IdolHeadSpriteIndex = (byte)DRandomMath.Range(0, DSpriteConstants.IDOL_HEAD_VARIATIONS - 1);

            this.IsPlayerOnSurface = true;
            this.IsPlayerInUnderground = false;
            this.IsPlayerInDepth = false;

            this.IsGameCrucialMenuOpen = false;

#if DESKTOP
            this.IsGameFocused = true;
#endif

            this.IsIdolCutsceneRunning = true;
            this.IsTruckCutsceneRunning = false;
            this.IsPlayerCutsceneRunning = false;

            this.TransitionIsDisabled = true;

            this.IsWorldActive = true;
            this.IsWorldVisible = true;
        }
    }
}
