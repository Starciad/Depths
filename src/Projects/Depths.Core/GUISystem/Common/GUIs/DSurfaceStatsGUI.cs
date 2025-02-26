using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private DGUIState state;

        private byte updateFrameCounter;
        private byte moneyRaised = 0;

        private readonly DGUIImageElement panelElement;
        private readonly DGUITextElement moneyTextElement;
        private readonly DGUITextElement oreCountingTextElement;

        private readonly Texture2D guiTexture;

        private readonly DAssetDatabase assetDatabase;
        private readonly DTextManager textManager;
        private readonly DGameInformation gameInformation;

        private readonly byte updateFrameDelay = 2;

        private readonly byte yStartingPosition;
        private readonly sbyte yMiddlePosition;
        private readonly sbyte yFinalPosition;

        private readonly byte movementSpeed = 2;
        
        internal DSurfaceStatsGUI(DAssetDatabase assetDatabase, DTextManager textManager, DGameInformation gameInformation) : base()
        {
            this.assetDatabase = assetDatabase;
            this.textManager = textManager;
            this.gameInformation = gameInformation;

            this.panelElement = new()
            {
                IsVisible = false,
                Position = new(0, yStartingPosition),
            };

            this.moneyTextElement = new(textManager)
            {
                Position = new(17, 36),
            };

            this.panelElement.SetTexture(assetDatabase.GetTexture("texture_gui_1"));

            this.yStartingPosition = DScreenConstants.GAME_HEIGHT;
            this.yMiddlePosition = 0;
            this.yFinalPosition = -DScreenConstants.GAME_HEIGHT;
        }

        protected override void OnBuild()
        {
            AddElement(this.panelElement);
            AddElement(this.moneyTextElement);
        }

        internal override void Load()
        {
            this.currentYGuiPanelPosition = (sbyte)this.yStartingPosition;
            this.moneyRaised = 0;
            this.state = DGUIState.Appearing;
        }

        internal override void Unload()
        {

        }

        internal override void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            this.updateFrameCounter++;

            if (this.updateFrameCounter < this.updateFrameDelay)
            {
                return;
            }

            this.updateFrameCounter = 0;

            switch (this.state)
            {
                case DGUIState.Disable:
                    this.panelElement.IsVisible = false;
                    break;

                case DGUIState.Appearing:
                    this.panelElement.IsVisible = true;
                    UpdateAppearanceAnimation();
                    break;

                case DGUIState.Showing:
                    this.panelElement.IsVisible = true;
                    UpdateScoreAnimation();
                    break;

                case DGUIState.Leaving:
                    this.panelElement.IsVisible = true;
                    UpdateDisappearanceAnimation();
                    break;

                default:
                    this.panelElement.IsVisible = false;
                    break;
            }

            this.panelElement.Position = new(0, this.currentYGuiPanelPosition);
            this.moneyTextElement.SetValue(this.moneyRaised.ToString());
        }

        private void UpdateAppearanceAnimation()
        {
            if (this.currentYGuiPanelPosition != this.yMiddlePosition)
            {
                this.currentYGuiPanelPosition -= (sbyte)this.movementSpeed;
                return;
            }

            this.currentYGuiPanelPosition = this.yMiddlePosition;
            this.state = DGUIState.Showing;
        }

        private void UpdateScoreAnimation()
        {
            this.state = DGUIState.Leaving;
        }

        private void UpdateDisappearanceAnimation()
        {
            // this.state = DGUIState.Disable;
        }
    }
}
