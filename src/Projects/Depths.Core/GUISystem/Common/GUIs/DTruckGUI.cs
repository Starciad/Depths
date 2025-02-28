using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI : DGUI
    {
        private DSection selectedSection;
        private DButton selectedButton;

        private int currentPageIndex = 0;

        private readonly DPurchasableItem[] purchasableUpgrades;
        private readonly DPurchasableItem[] purchasableItems;

        private readonly DGUITextElement currentMoneyTextElement;
        private readonly DGUITextElement pageTitleTextElement;
        private readonly DGUITextElement priceTextElement;
        private readonly DGUITextElement previewTextElement;

        private readonly DGUIImageElement buttonElement;
        private readonly DGUIImageElement mainPanelElement;
        private readonly DGUIImageElement upgradePanelElement;
        private readonly DGUIImageElement itemPanelElement;

        private readonly DPoint[] moneyElementPositions;

        private readonly Rectangle[] buttonSourceRectangles = [
            new(new(0, 0), new(67, 25)),
            new(new(67, 0), new(67, 25)),
        ];

        private readonly DInputManager inputManager;
        private readonly DGUIManager guiManager;
        private readonly DGameInformation gameInformation;

        internal DTruckGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DTextManager textManager) : base(identifier)
        {
            this.inputManager = inputManager;
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;

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

            #region ITEMS
            // Upgrades
            this.purchasableUpgrades = [
                new("Energy", 4, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.MaximumEnergy;
                        item.NextPreviewValue = gameInformation.PlayerEntity.MaximumEnergy + DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.MaximumEnergy = (byte)item.NextPreviewValue;
                    },
                },

                new("Power", 6, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.Power;
                        item.NextPreviewValue = gameInformation.PlayerEntity.Power + DPlayerConstants.DEFAULT_STARTING_POWER;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.Power = (byte)item.NextPreviewValue;
                    },
                },

                new("Damage", 5, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.Damage;
                        item.NextPreviewValue = gameInformation.PlayerEntity.Damage + DPlayerConstants.DEFAULT_STARTING_DAMAGE;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.Damage = (byte)item.NextPreviewValue;
                    },
                },

                new("Bag Size", 10, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.BackpackSize;
                        item.NextPreviewValue = gameInformation.PlayerEntity.BackpackSize + DPlayerConstants.DEFAULT_STARTING_BAG_SIZE;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.BackpackSize = (byte)item.NextPreviewValue;
                    },
                },
            ];

            // Items
            this.purchasableItems = [
                new("Stairs", 5, false, 0)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.StairCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.StairCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.StairCount = (uint)item.NextPreviewValue;
                    },
                },

                new("Plataforms", 8, false, 0)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.PlataformCount = (uint)item.NextPreviewValue;
                    },
                },

                new("Miners", 25, false, 0)
                {
                    OnSyncPreviewValuesCallback = (DPurchasableItem item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.RobotCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.RobotCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS;
                    },

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.RobotCount = (uint)item.NextPreviewValue;
                    },
                },
            ];
            #endregion
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
            HandleUserInputs();
            UpdateGUI();
        }
    }
}
