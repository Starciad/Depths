using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;

using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DMainMenuGUI : DGUI
    {
        private sealed class DButton(string name, Action onClickCallback)
        {
            internal string Name => name;
            internal Action OnClickCallback => onClickCallback;
        }

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
        private readonly DMusicManager musicManager;

        internal DMainMenuGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DMusicManager musicManager, DTextManager textManager) : base(identifier)
        {
            this.gameInformation = gameInformation;
            this.inputManager = inputManager;
            this.musicManager = musicManager;

            this.buttonNameElement = new(textManager, new()
            {
                CharacterSpacing = -1,
                FontType = DFontType.Dark,
                HorizontalAlignment = DTextAlignment.Center,
            })
            {
                Position = new(21, 29)
            };

            this.backgroundElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_6"),
            };

            this.buttons = [
                new("Start", () =>
                {
                    guiManager.Close(identifier);
                    gameInformation.Start();
                }),
                new("Help", () =>
                {
                    guiManager.Close(identifier);
                    guiManager.Open("About");
                }),
                new("Cast", () =>
                {
                    guiManager.Close(identifier);
                    guiManager.Open("Credits");
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
            this.musicManager.SetMusic("Main Menu");
            this.musicManager.PlayMusic();

            this.buttonIndex = 0;

            this.gameInformation.IsGameStarted = false;
            this.gameInformation.IsWorldActive = false;
            this.gameInformation.IsWorldVisible = false;

            SyncButtonElement();
        }

        internal override void Update()
        {
            HandleUserInput();
            UpdateBackgroundAnimation();
        }

        private void HandleUserInput()
        {
            if (this.inputManager.Started(DKeyMappingConstant.Confirm))
            {
                this.buttons[this.buttonIndex].OnClickCallback?.Invoke();
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Up))
            {
                UpButton();
                SyncButtonElement();
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Down))
            {
                DownButton();
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

        private void UpButton()
        {
            if (--this.buttonIndex < 0)
            {
                this.buttonIndex = (sbyte)(this.buttons.Length - 1);
            }
        }

        private void DownButton()
        {
            if (++this.buttonIndex > this.buttons.Length - 1)
            {
                this.buttonIndex = 0;
            }
        }
    }
}
