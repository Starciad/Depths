using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Inputs;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.GUISystem.Helpers;
using Depths.Core.Managers;

using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DPauseGUI : DGUI
    {
        private bool backgroundAnimationState;
        private byte backgroundAnimationFrameCounter;
        private sbyte buttonIndex;

        private readonly byte backgroundAnimationFrameDelay = 10;

        private readonly DGUIImageElement backgroundElement;
        private readonly DGUITextElement buttonNameElement;
        private readonly DButton[] buttons;

        private readonly Rectangle[] backgroundSourceRectangles = [
            new(new(0, 0), new(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT)),
            new(new(DScreenConstants.GAME_WIDTH, 0), new(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT)),
        ];

        private readonly DGameInformation gameInformation;
        private readonly DInputManager inputManager;

        internal DPauseGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DTextManager textManager) : base(identifier)
        {
            this.gameInformation = gameInformation;
            this.inputManager = inputManager;

            this.buttonNameElement = new(textManager, new()
            {
                CharacterSpacing = -1,
                FontType = DFontType.Light,
                HorizontalAlignment = DTextAlignment.Center,
            })
            {
                Position = new(42, 35)
            };

            this.backgroundElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_9"),
            };

            this.buttons = [
                new("Resume", () =>
                {
                    guiManager.Close(identifier);
                }),

                new("Exit", () =>
                {
                    guiManager.Close(identifier);
                    guiManager.Open("Main Menu");
                }),
            ];
        }

        protected override void OnBuild()
        {
            AddElement(this.backgroundElement);
            AddElement(this.buttonNameElement);
        }

        internal override void Load()
        {
            this.buttonIndex = 0;
            this.gameInformation.IsWorldActive = false;
            SyncButtonElement();
        }

        internal override void Unload()
        {
            this.gameInformation.IsWorldActive = true;
        }

        internal override void Update()
        {
            HandleUserInput();
            UpdateBackgroundAnimation();
        }

        private void HandleUserInput()
        {
            if (this.inputManager.Started(DCommandType.Cancel))
            {
                return;
            }

            if (this.inputManager.Started(DCommandType.Confirm))
            {
                this.buttons[this.buttonIndex].OnClickCallback?.Invoke();
                return;
            }

            if (this.inputManager.Started(DCommandType.Right))
            {
                RightButton();
                SyncButtonElement();
                return;
            }

            if (this.inputManager.Started(DCommandType.Left))
            {
                LeftButton();
                SyncButtonElement();
                return;
            }
        }

        private void UpdateBackgroundAnimation()
        {
            if (++this.backgroundAnimationFrameCounter > this.backgroundAnimationFrameDelay)
            {
                this.backgroundAnimationFrameCounter = 0;
                this.backgroundAnimationState = !this.backgroundAnimationState;

                this.backgroundElement.TextureClipArea = this.backgroundSourceRectangles[Convert.ToByte(this.backgroundAnimationState)];
            }
        }

        private void SyncButtonElement()
        {
            this.buttonNameElement.SetValue(this.buttons[this.buttonIndex].Name);
        }

        private void RightButton()
        {
            if (++this.buttonIndex > this.buttons.Length - 1)
            {
                this.buttonIndex = 0;
            }
        }

        private void LeftButton()
        {
            if (--this.buttonIndex < 0)
            {
                this.buttonIndex = (sbyte)(this.buttons.Length - 1);
            }
        }
    }
}
