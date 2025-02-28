using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.World.Ores;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DSurfaceStatsGUI : DGUI
    {
        private enum DGUIState : byte
        {
            Disable = 0,
            Appearing = 1,
            Showing = 2,
            Leaving = 3
        }

        private sbyte currentYGuiPanelPosition;

        private DGUIState state = DGUIState.Disable;

        private byte movementUpdateFrameCounter = 0;
        private byte oreUpdateFrameCounter = 0;
        private byte leavingUpdateFrameCounter = 0;

        private byte moneyRaised = 0;
        private byte countedMinerals = 0;
        private byte currentOreIndex = 0;

        private readonly DGUIImageElement panelElement;
        private readonly DGUIImageElement[] oreIconElements;
        private readonly DGUITextElement moneyTextElement;
        private readonly DGUITextElement oreCountingTextElement;

        private readonly byte updateFrameDelay = 2;
        private readonly byte oreUpdateFrameDelay = 3;
        private readonly byte leavingUpdateFrameDelay = 8;

        private readonly byte yStartingPosition = DScreenConstants.GAME_HEIGHT;
        private readonly byte yMiddlePosition = 0;
        private readonly sbyte yFinalPosition = -DScreenConstants.GAME_HEIGHT;

        private readonly byte movementSpeed = 2;

        private readonly DGameInformation gameInformation;
        private readonly DGUIManager guiManager;

        internal DSurfaceStatsGUI(string identifier, DAssetDatabase assetDatabase, DTextManager textManager, DGUIManager guiManager, DGameInformation gameInformation) : base(identifier)
        {
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;

            this.panelElement = new()
            {
                IsVisible = false,
                Position = new(0, this.yStartingPosition),
            };

            this.moneyTextElement = new(textManager, new())
            {
                IsVisible = false,
                Position = new(17, 37),
            };

            this.oreCountingTextElement = new(textManager, new()
            {
                CharacterSpacing = -1,
            })
            {
                IsVisible = false,
                Position = new(60, 37),
            };

            this.oreIconElements = new DGUIImageElement[5];

            for (byte i = 0; i < this.oreIconElements.Length; i++)
            {
                DGUIImageElement oreIconElement = new()
                {
                    IsVisible = false,
                    Position = new(11 + (i * (DSpriteConstants.ORE_ICON_SIZE + 2)), 16)
                };

                this.oreIconElements[i] = oreIconElement;
            }

            this.panelElement.SetTexture(assetDatabase.GetTexture("texture_gui_1"));
        }

        protected override void OnBuild()
        {
            AddElement(this.panelElement);
            AddElement(this.moneyTextElement);
            AddElement(this.oreCountingTextElement);

            for (byte i = 0; i < this.oreIconElements.Length; i++)
            {
                AddElement(this.oreIconElements[i]);
            }
        }

        internal override void Load()
        {
            this.currentYGuiPanelPosition = (sbyte)this.yStartingPosition;

            this.moneyRaised = 0;
            this.countedMinerals = 0;
            this.currentOreIndex = 0;

            this.state = DGUIState.Appearing;

            this.movementUpdateFrameCounter = 0;
            this.oreUpdateFrameCounter = 0;
            this.leavingUpdateFrameCounter = 0;

            this.gameInformation.IsWorldActive = false;

            HideAllOreIconElements();
        }

        internal override void Unload()
        {
            this.gameInformation.IsWorldActive = true;
        }

        internal override void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            if (++this.movementUpdateFrameCounter < this.updateFrameDelay)
            {
                return;
            }

            this.movementUpdateFrameCounter = 0;

            switch (this.state)
            {
                case DGUIState.Disable:
                    this.panelElement.IsVisible = false;
                    this.moneyTextElement.IsVisible = false;
                    this.oreCountingTextElement.IsVisible = false;
                    break;

                case DGUIState.Appearing:
                    this.panelElement.IsVisible = true;
                    this.moneyTextElement.IsVisible = false;
                    this.oreCountingTextElement.IsVisible = false;
                    UpdateAppearanceAnimation();
                    break;

                case DGUIState.Showing:
                    this.panelElement.IsVisible = true;
                    this.moneyTextElement.IsVisible = true;
                    this.oreCountingTextElement.IsVisible = true;
                    UpdateScoreAnimation();
                    break;

                case DGUIState.Leaving:
                    this.panelElement.IsVisible = true;
                    this.moneyTextElement.IsVisible = false;
                    this.oreCountingTextElement.IsVisible = false;
                    UpdateDisappearanceAnimation();
                    break;

                default:
                    this.panelElement.IsVisible = false;
                    this.moneyTextElement.IsVisible = false;
                    this.oreCountingTextElement.IsVisible = false;
                    break;
            }

            this.panelElement.Position = new(0, this.currentYGuiPanelPosition);

            this.moneyTextElement.SetValue(this.moneyRaised.ToString());
            this.oreCountingTextElement.SetValue(this.countedMinerals.ToString("D2"));
        }

        private void UpdateAppearanceAnimation()
        {
            if (this.currentYGuiPanelPosition != this.yMiddlePosition)
            {
                this.currentYGuiPanelPosition -= (sbyte)this.movementSpeed;
                return;
            }

            this.currentYGuiPanelPosition = (sbyte)this.yMiddlePosition;
            this.state = DGUIState.Showing;
        }

        private void UpdateScoreAnimation()
        {
            if (++this.oreUpdateFrameCounter < this.oreUpdateFrameDelay)
            {
                return;
            }

            this.oreUpdateFrameCounter = 0;

            if (this.gameInformation.PlayerEntity.CollectedMinerals.TryDequeue(out DOre ore))
            {
                this.moneyRaised += ore.Value;
                this.countedMinerals++;
                this.gameInformation.PlayerEntity.Money += ore.Value;

                // Calculates the index adjusted to the size of the array (visibility cycle)
                byte index = Convert.ToByte(this.currentOreIndex % (byte)this.oreIconElements.Length);

                // If the index is 0, it means we have reached a multiple of the array size and we must reset
                this.oreIconElements[index].SetTexture(ore.IconTexture);

                if (index == 0)
                {
                    // Makes all elements invisible
                    HideAllOreIconElements();

                    // Keep only the first one visible
                    this.oreIconElements[0].IsVisible = true;
                }
                else
                {
                    // Sets the icon in the correct slot and makes it visible
                    this.oreIconElements[index].IsVisible = true;
                }

                this.currentOreIndex++;
                return;
            }

            if (++this.leavingUpdateFrameCounter < this.leavingUpdateFrameDelay)
            {
                return;
            }

            HideAllOreIconElements();
            this.state = DGUIState.Leaving;
        }

        private void UpdateDisappearanceAnimation()
        {
            if (this.currentYGuiPanelPosition != this.yFinalPosition)
            {
                this.currentYGuiPanelPosition -= (sbyte)this.movementSpeed;
                return;
            }

            this.currentYGuiPanelPosition = this.yFinalPosition;
            this.state = DGUIState.Disable;

            this.guiManager.Close(this.Identifier);
        }

        private void HideAllOreIconElements()
        {
            foreach (DGUIImageElement oreIconElement in this.oreIconElements)
            {
                oreIconElement.IsVisible = false;
            }
        }
    }
}
