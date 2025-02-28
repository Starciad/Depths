using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;

using Microsoft.Xna.Framework;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DCreditsGUI : DGUI
    {
        private bool exitFlag;

        private byte currentTextIndex;
        private byte truckAnimationIndex;
        private byte backgroundAnimationIndex;

        private byte displayedTextChangeFrameCounter;
        private byte truckAnimationFrameCounter;
        private byte backgroundAnimationFrameCounter;

        private readonly byte displayedTextChangeFrameDelay = 96;
        private readonly byte truckAnimationFrameDelay = 3;
        private readonly byte backgroundAnimationFrameDelay = 2;

        private readonly DGUITextElement textElement;
        private readonly DGUIImageElement backgroundImageElement;
        private readonly DGUIImageElement truckImageElement;
        private readonly DGUIImageElement idolImageElement;

        private readonly string[] texts =
        [
            "DEPTHS",
            "By Starciad",
            "Nokia Jam",
            "7th Edition",
            "Thx 4 Play!",
            "Press Any Key",
        ];

        private readonly DGameInformation gameInformation;
        private readonly DGUIManager guiManager;
        private readonly DInputManager inputManager;
        private readonly DMusicManager musicManager;

        private readonly Rectangle[] backgroundSourceRectangles;

        internal DCreditsGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DMusicManager musicManager, DTextManager textManager) : base(identifier)
        {
            this.textElement = new(textManager, new()
            {
                FontType = DFontType.Dark,
                CharacterSpacing = -1,
                HorizontalAlignment = DTextAlignment.Center,
                WrapText = true,
            })
            {
                Position = new(42, 2),
            };

            this.backgroundImageElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_8"),
            };
            this.truckImageElement = new()
            {
                Position = new(4, 17),
                Texture = assetDatabase.GetTexture("texture_entity_2"),
                TextureClipArea = DSpriteConstants.TRUCK_SOURCE_RECTANGLES[0],
            };
            this.idolImageElement = new()
            {
                Position = new(59, 13),
                Texture = assetDatabase.GetTexture("texture_entity_4"),
            };

            this.backgroundSourceRectangles = new Rectangle[8];
            for (int i = 0; i < this.backgroundSourceRectangles.Length; i++)
            {
                this.backgroundSourceRectangles[i] = new(new(0, i * DScreenConstants.GAME_HEIGHT), new(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT));
            }

            this.gameInformation = gameInformation;
            this.guiManager = guiManager;
            this.inputManager = inputManager;
            this.musicManager = musicManager;
        }

        protected override void OnBuild()
        {
            AddElement(this.backgroundImageElement);
            AddElement(this.truckImageElement);
            AddElement(this.idolImageElement);
            AddElement(this.textElement);
        }

        internal override void Load()
        {
            this.exitFlag = false;

            this.currentTextIndex = 0;
            this.truckAnimationIndex = 0;
            this.backgroundAnimationIndex = 0;

            this.displayedTextChangeFrameCounter = 0;
            this.truckAnimationFrameCounter = 0;
            this.backgroundAnimationFrameCounter = 0;

            this.textElement.SetValue(string.Empty);

            this.musicManager.SetMusic("Credits");
            this.musicManager.PlayMusic();

            this.idolImageElement.TextureClipArea = DSpriteConstants.IDOL_SOURCE_RECTANGLES[this.gameInformation.IdolHeadSpriteIndex];
        }

        internal override void Update()
        {
            HandleUserInput();
            UpdateAnimations();
        }

        private void HandleUserInput()
        {
            if (!this.exitFlag)
            {
                return;
            }

            if (this.inputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.guiManager.Close(this.Identifier);
                this.guiManager.Open("Main Menu");
            }
        }

        private void UpdateAnimations()
        {
            // --- Text animation update ---
            if (this.currentTextIndex < this.texts.Length)
            {
                if (++this.displayedTextChangeFrameCounter >= this.displayedTextChangeFrameDelay)
                {
                    this.textElement.SetValue(this.texts[this.currentTextIndex]);
                    this.displayedTextChangeFrameCounter = 0;
                    this.currentTextIndex++;
                }
            }
            else
            {
                if (!this.exitFlag)
                {
                    this.exitFlag = true;
                }
            }

            // --- Truck animation update ---
            if (++this.truckAnimationFrameCounter >= this.truckAnimationFrameDelay)
            {
                this.truckAnimationIndex = (byte)((this.truckAnimationIndex + 1) % DSpriteConstants.TRUCK_SOURCE_RECTANGLES.Length);
                this.truckImageElement.TextureClipArea = DSpriteConstants.TRUCK_SOURCE_RECTANGLES[this.truckAnimationIndex];
                this.truckAnimationFrameCounter = 0;
            }

            // --- Truck animation update ---
            if (++this.backgroundAnimationFrameCounter >= this.backgroundAnimationFrameDelay)
            {
                this.backgroundAnimationIndex = (byte)((this.backgroundAnimationIndex + 1) % (this.backgroundSourceRectangles.Length - 1));
                this.backgroundImageElement.TextureClipArea = this.backgroundSourceRectangles[this.backgroundAnimationIndex];
                this.backgroundAnimationFrameCounter = 0;
            }
        }
    }
}
