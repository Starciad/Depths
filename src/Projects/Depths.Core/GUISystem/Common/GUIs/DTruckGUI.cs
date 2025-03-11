using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.Shop;

using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI : DGUI
    {
        private DSection selectedSection;
        private DButton selectedButton;

        private int currentPageIndex = 0;

        private bool pageAnimationState;
        private byte pageAnimationFrameCounter;

        private readonly byte pageAnimationFrameDelay = 10;

        private readonly DGUITextElement currentMoneyTextElement;
        private readonly DGUITextElement pageTitleTextElement;
        private readonly DGUITextElement priceTextElement;
        private readonly DGUITextElement previewTextElement;

        private readonly DGUIImageElement buttonElement;
        private readonly DGUIImageElement mainPanelElement;
        private readonly DGUIImageElement upgradePanelElement;
        private readonly DGUIImageElement itemPanelElement;

        private readonly DPoint[] moneyElementPositions;

        private readonly Rectangle[] pageSourceRectangles = [
            new(new(0, 0), new(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT)),
            new(new(DScreenConstants.GAME_WIDTH, 0), new(DScreenConstants.GAME_WIDTH, DScreenConstants.GAME_HEIGHT)),
        ];

        private readonly Rectangle[] buttonSourceRectangles = [
            new(new(0, 0), new(67, 25)),
            new(new(67, 0), new(67, 25)),
        ];

        private readonly DInputManager inputManager;
        private readonly DGUIManager guiManager;
        private readonly DGameInformation gameInformation;
        private readonly DShopDatabase shopDatabase;

        internal DTruckGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DShopDatabase shopDatabase, DTextManager textManager) : base(identifier)
        {
            this.inputManager = inputManager;
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;
            this.shopDatabase = shopDatabase;

            // ============================ //

            // Texts
            this.currentMoneyTextElement = new(textManager, new());

            this.pageTitleTextElement = new(textManager, new()
            {
                FontType = DFontType.Dark,
                HorizontalAlignment = DTextAlignment.Center,
                CharacterSpacing = -1
            })
            {
                Position = new(42, 4),
            };

            this.priceTextElement = new(textManager, new()
            {
                FontType = DFontType.Dark,
                HorizontalAlignment = DTextAlignment.Center,
            })
            {
                Position = new(24, 18),
            };

            this.previewTextElement = new(textManager, new()
            {
                FontType = DFontType.Dark,
                HorizontalAlignment = DTextAlignment.Center,
                CharacterSpacing = -1
            })
            {
                Position = new(59, 18),
            };

            // Elements
            this.buttonElement = new()
            {
                Position = new(13, 4),
                Texture = assetDatabase.GetTexture("texture_button_1")
            };
            this.mainPanelElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_3")
            };
            this.upgradePanelElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_4")
            };
            this.itemPanelElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_4")
            };

            // ============================ //

            this.moneyElementPositions = [
                new(20, 36),
                new(16, 36),
            ];
        }

        protected override void OnBuild()
        {
            AddElement(this.mainPanelElement);
            AddElement(this.upgradePanelElement);
            AddElement(this.itemPanelElement);
            AddElement(this.buttonElement);

            AddElement(this.currentMoneyTextElement);
            AddElement(this.pageTitleTextElement);
            AddElement(this.priceTextElement);
            AddElement(this.previewTextElement);
        }

        internal override void Load()
        {
            this.selectedSection = DSection.Main;
            this.selectedButton = DButton.Upgrades;

            this.gameInformation.IsWorldActive = false;
        }

        internal override void Unload()
        {
            this.gameInformation.IsWorldActive = true;
        }

        internal override void Update()
        {
            UpdateBackgroundAnimation();
            HandleUserInputs();
            UpdateGUI();
        }

        public override void Reset()
        {
            foreach (DPurchasableItem purchasableItem in this.shopDatabase.PurchasableUpgrades)
            {
                purchasableItem.Reset();
            }

            foreach (DPurchasableItem purchasableItem in this.shopDatabase.PurchasableItems)
            {
                purchasableItem.Reset();
            }
        }

        private void UpdateBackgroundAnimation()
        {
            if (++this.pageAnimationFrameCounter > this.pageAnimationFrameDelay)
            {
                this.pageAnimationFrameCounter = 0;
                this.pageAnimationState = !this.pageAnimationState;

                this.itemPanelElement.TextureClipArea = this.pageSourceRectangles[Convert.ToByte(this.pageAnimationState)];
                this.upgradePanelElement.TextureClipArea = this.pageSourceRectangles[Convert.ToByte(this.pageAnimationState)];
            }
        }
    }
}
