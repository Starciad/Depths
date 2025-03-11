using Depths.Core.Constants;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Items;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World.Ores;
using Depths.Core.World.Tiles;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DHudGUI : DGUI
    {
        // ========================= //

        #region Counters
        private byte topTextMovementFrameCounter;
        private byte topTextVisibilityFrameCounter;

        private byte centerTextMovementFrameCounter;
        private byte centerTextVisibilityFrameCounter;

        private byte bottomTextMovementFrameCounter;
        private byte bottomTextVisibilityFrameCounter;
        #endregion

        #region Delays
        private readonly byte topTextMovementFrameDelay = 5;
        private readonly byte topTextVisibilityFrameDelay = 32;

        private readonly byte centerTextMovementFrameDelay = 5;
        private readonly byte centerTextVisibilityFrameDelay = 32;

        private readonly byte bottomTextMovementFrameDelay = 5;
        private readonly byte bottomTextVisibilityFrameDelay = 32;
        #endregion

        // ========================= //

        private readonly byte topTextSpeed = 1;
        private readonly byte centerTextSpeed = 1;
        private readonly byte bottomTextSpeed = 1;

        private readonly int topTextYAnchorPosition = 0;
        private readonly int centerTextYAnchorPosition = DScreenConstants.GAME_HEIGHT / 2;
        private readonly int bottomTextYAnchorPosition = DScreenConstants.GAME_HEIGHT - DFontConstants.HEIGHT;

        private readonly DGUITextElement topTextElement;
        private readonly DGUITextElement centerTextElement;
        private readonly DGUITextElement bottomTextElement;

        private readonly DTextManager textManager;
        private readonly DGUIManager guiManager;
        private readonly DGameInformation gameInformation;

        internal DHudGUI(string identifier, DTextManager textManager, DGUIManager guiManager, DGameInformation gameInformation) : base(identifier)
        {
            this.textManager = textManager;
            this.guiManager = guiManager;

            this.gameInformation = gameInformation;

            this.topTextElement = new(this.textManager, new()
            {
                FontType = DFontType.DarkOutline,
                HorizontalAlignment = DTextAlignment.Center,
                WrapText = true,
                CharacterSpacing = -1,
            })
            {
                IsVisible = false,
                Position = new(DScreenConstants.GAME_WIDTH / 2, 0),
            };

            this.centerTextElement = new(this.textManager, new()
            {
                FontType = DFontType.DarkOutline,
                HorizontalAlignment = DTextAlignment.Center,
                WrapText = true,
                CharacterSpacing = -1,
            })
            {
                IsVisible = false,
                Position = new(DScreenConstants.GAME_WIDTH / 2, 0),
            };

            this.bottomTextElement = new(this.textManager, new()
            {
                FontType = DFontType.DarkOutline,
                HorizontalAlignment = DTextAlignment.Center,
                WrapText = true,
                CharacterSpacing = -1,
            })
            {
                IsVisible = false,
                Position = new(DScreenConstants.GAME_WIDTH / 2, 0),
            };
        }

        protected override void OnBuild()
        {
            AddElement(this.topTextElement);
            AddElement(this.centerTextElement);
            AddElement(this.bottomTextElement);
        }

        internal override void Load()
        {
            this.gameInformation.OnPlayerReachedTheSurface += GameInformation_PlayerReachedTheSurface;
            this.gameInformation.OnPlayerReachedTheUnderground += GameInformation_PlayerReachedTheUnderground;
            this.gameInformation.OnPlayerReachedTheDepth += GameInformation_PlayerReachedTheDepth;

            this.gameInformation.PlayerEntity.OnEnergyDepleted += Player_OnEnergyDepleted;
            this.gameInformation.PlayerEntity.OnCollectedOre += Player_OnCollectedOre;
            this.gameInformation.PlayerEntity.OnFullBackpack += Player_OnFullBackpack;
            this.gameInformation.PlayerEntity.OnTriedMineToughBlock += Player_OnTriedMineToughBlock;
            this.gameInformation.PlayerEntity.OnTriedMineIndestructibleBlock += Player_OnTriedMineIndestructibleBlock;
            this.gameInformation.PlayerEntity.OnCollectedItemFromBox += Player_OnCollectedItemFromBox;
        }

        internal override void Unload()
        {
            this.gameInformation.OnPlayerReachedTheSurface -= GameInformation_PlayerReachedTheSurface;
            this.gameInformation.OnPlayerReachedTheUnderground -= GameInformation_PlayerReachedTheUnderground;
            this.gameInformation.OnPlayerReachedTheDepth -= GameInformation_PlayerReachedTheDepth;

            this.gameInformation.PlayerEntity.OnEnergyDepleted -= Player_OnEnergyDepleted;
            this.gameInformation.PlayerEntity.OnCollectedOre -= Player_OnCollectedOre;
            this.gameInformation.PlayerEntity.OnFullBackpack -= Player_OnFullBackpack;
            this.gameInformation.PlayerEntity.OnTriedMineToughBlock -= Player_OnTriedMineToughBlock;
            this.gameInformation.PlayerEntity.OnTriedMineIndestructibleBlock -= Player_OnTriedMineIndestructibleBlock;
            this.gameInformation.PlayerEntity.OnCollectedItemFromBox -= Player_OnCollectedItemFromBox;
        }

        internal override void Update()
        {
            UpdateTextAnimations();
        }

        private void UpdateTextAnimations()
        {
            UpdateTextMovementAndVisibility(
                this.topTextElement,
                ref this.topTextMovementFrameCounter,
                ref this.topTextVisibilityFrameCounter,
                this.topTextMovementFrameDelay,
                this.topTextVisibilityFrameDelay,
                1,
                this.topTextSpeed
            );

            UpdateTextMovementAndVisibility(
                this.centerTextElement,
                ref this.centerTextMovementFrameCounter,
                ref this.centerTextVisibilityFrameCounter,
                this.centerTextMovementFrameDelay,
                this.centerTextVisibilityFrameDelay,
                -1,
                this.centerTextSpeed
            );

            UpdateTextMovementAndVisibility(
                this.bottomTextElement,
                ref this.bottomTextMovementFrameCounter,
                ref this.bottomTextVisibilityFrameCounter,
                this.bottomTextMovementFrameDelay,
                this.bottomTextVisibilityFrameDelay,
                -1,
                this.bottomTextSpeed
            );
        }

        private static void UpdateTextMovementAndVisibility(
            DGUITextElement textElement,
            ref byte movementFrameCounter,
            ref byte visibilityFrameCounter,
            byte movementFrameDelay,
            byte visibilityFrameDelay,
            sbyte deltaY,
            byte speed
        )
        {
            if (!textElement.IsVisible)
            {
                return;
            }

            if (++visibilityFrameCounter > visibilityFrameDelay)
            {
                textElement.IsVisible = false;
                return;
            }

            if (++movementFrameCounter < movementFrameDelay)
            {
                return;
            }

            movementFrameCounter = 0;
            textElement.Position += new DPoint(0, speed * deltaY);
        }

        // ====================================== //

        private static void ResetTextElement(DGUITextElement textElement, int yPosition, ref byte textVisibilityFrameCounter)
        {
            textVisibilityFrameCounter = 0;
            textElement.Position = new(textElement.Position.X, yPosition);
            textElement.IsVisible = true;
        }

        // ====================================== //

        private void GameInformation_PlayerReachedTheSurface()
        {
            ResetTextElement(this.topTextElement, this.topTextYAnchorPosition, ref this.topTextVisibilityFrameCounter);
            this.topTextElement.SetValue("Surface");

            this.gameInformation.PlayerEntity.Energy = this.gameInformation.PlayerEntity.MaximumEnergy;

            if (this.gameInformation.PlayerEntity.CollectedMinerals.Count > 0)
            {
                this.guiManager.Open("Surface Stats");
            }
        }

        private void GameInformation_PlayerReachedTheUnderground()
        {
            ResetTextElement(this.topTextElement, this.topTextYAnchorPosition, ref this.topTextVisibilityFrameCounter);
            this.topTextElement.SetValue("Cave");
        }

        private void GameInformation_PlayerReachedTheDepth()
        {
            ResetTextElement(this.topTextElement, this.topTextYAnchorPosition, ref this.topTextVisibilityFrameCounter);
            this.topTextElement.SetValue("Depth");
        }

        /* -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- */

        private void Player_OnEnergyDepleted()
        {
            ResetTextElement(this.bottomTextElement, this.bottomTextYAnchorPosition, ref this.bottomTextVisibilityFrameCounter);
            this.bottomTextElement.SetValue("Exhausted!");
        }

        private void Player_OnFullBackpack()
        {
            ResetTextElement(this.bottomTextElement, this.bottomTextYAnchorPosition, ref this.bottomTextVisibilityFrameCounter);
            this.bottomTextElement.SetValue("Full Bag!");
        }

        private void Player_OnCollectedOre(DOre ore)
        {
            ResetTextElement(this.centerTextElement, this.centerTextYAnchorPosition, ref this.centerTextVisibilityFrameCounter);
            this.centerTextElement.SetValue(ore.Name);
        }

        private void Player_OnTriedMineToughBlock(DTile tile)
        {
            ResetTextElement(this.bottomTextElement, this.bottomTextYAnchorPosition, ref this.bottomTextVisibilityFrameCounter);
            this.bottomTextElement.SetValue(string.Concat("Req. Power ", tile.Resistance));
        }

        private void Player_OnTriedMineIndestructibleBlock(DTile tile)
        {
            ResetTextElement(this.bottomTextElement, this.bottomTextYAnchorPosition, ref this.bottomTextVisibilityFrameCounter);
            this.bottomTextElement.SetValue("Unbreak Block");
        }

        private void Player_OnCollectedItemFromBox(DBoxItem boxItem, uint quantityObtained)
        {
            ResetTextElement(this.centerTextElement, this.centerTextYAnchorPosition, ref this.centerTextVisibilityFrameCounter);
            ResetTextElement(this.bottomTextElement, this.bottomTextYAnchorPosition, ref this.bottomTextVisibilityFrameCounter);

            this.centerTextElement.SetValue("Box Broken!");
            this.bottomTextElement.SetValue(string.Concat('+', quantityObtained, " ", boxItem.Name));
        }
    }
}
