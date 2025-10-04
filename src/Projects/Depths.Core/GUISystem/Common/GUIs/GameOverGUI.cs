using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class GameOverGUI : GUI
    {
        private enum DGUIState : byte
        {
            Disable = 0,
            Appearing = 1,
            Leaving = 2
        }

        private sbyte currentYGuiPanelPosition;

        private DGUIState state = DGUIState.Disable;

        private byte movementUpdateFrameCounter = 0;
        private byte appearingUpdateFrameCounter = 0;
        private byte leavingUpdateFrameCounter = 0;

        private readonly GUIImageElement panelElement;

        private readonly byte updateFrameDelay = 2;
        private readonly byte appearingUpdateFrameDelay = 16;
        private readonly byte leavingUpdateFrameDelay = 16;

        private readonly byte yStartingPosition = ScreenConstants.GAME_HEIGHT;
        private readonly sbyte yMiddlePosition = -2;
        private readonly sbyte yFinalPosition = -(ScreenConstants.GAME_HEIGHT + 2);

        private readonly byte movementSpeed = 2;

        private readonly GUIManager guiManager;
        private readonly GameInformation gameInformation;

        internal GameOverGUI(string identifier, AssetDatabase assetDatabase, GUIManager guiManager, GameInformation gameInformation) : base(identifier)
        {
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;

            this.panelElement = new()
            {
                IsVisible = false,
                Position = new(0, this.yStartingPosition),
                Texture = assetDatabase.GetTexture("texture_gui_2")
            };
        }

        protected override void OnBuild()
        {
            AddElement(this.panelElement);
        }

        internal override void Load()
        {
            this.currentYGuiPanelPosition = (sbyte)this.yStartingPosition;

            this.state = DGUIState.Appearing;

            this.movementUpdateFrameCounter = 0;
            this.leavingUpdateFrameCounter = 0;
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
                    break;

                case DGUIState.Appearing:
                    this.panelElement.IsVisible = true;
                    UpdateAppearanceAnimation();
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
        }

        private void UpdateAppearanceAnimation()
        {
            if (++this.appearingUpdateFrameCounter < this.appearingUpdateFrameDelay)
            {
                return;
            }

            if (this.currentYGuiPanelPosition != this.yMiddlePosition)
            {
                this.currentYGuiPanelPosition -= (sbyte)this.movementSpeed;
                return;
            }

            this.currentYGuiPanelPosition = this.yMiddlePosition;

            if (++this.leavingUpdateFrameCounter < this.leavingUpdateFrameDelay)
            {
                return;
            }

            this.state = DGUIState.Leaving;
            this.gameInformation.IsWorldVisible = false;
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
            this.guiManager.Open("Main Menu");
        }
    }
}
