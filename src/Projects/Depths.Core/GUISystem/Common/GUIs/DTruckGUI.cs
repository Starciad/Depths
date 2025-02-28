using Depths.Core.Audio;
using Depths.Core.Databases;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI : DGUI
    {
        private enum DSection : byte
        {
            Main = 0,
            Upgrades = 1,
            Items = 2
        }

        private enum DButton : byte
        {
            Upgrades = 0,
            Items = 1
        }

        private sealed class DPurchasableItem
        {
            internal string Name { get; private set; }
            internal uint Price { get; private set; }
            internal bool HasPriceIncrease { get; private set; }
            internal float PriceIncreaseFactor { get; private set; }

            internal int CurrentPreviewValue { get; set; }
            internal int NextPreviewValue { get; set; }

            internal Action<DPurchasableItem> OnBuyCallback { get; init; }

            internal DPurchasableItem(string name, uint basePrice, bool hasPriceIncrease, float priceIncreaseFactor)
            {
                this.Name = name;
                this.Price = basePrice;
                this.HasPriceIncrease = hasPriceIncrease;
                this.PriceIncreaseFactor = priceIncreaseFactor;
            }

            internal bool TryBuy(DPlayerEntity playerEntity)
            {
                if (playerEntity.Money >= this.Price)
                {
                    playerEntity.Money -= this.Price;

                    UpdatePrice();
                    this.OnBuyCallback?.Invoke(this);

                    DAudioEngine.Play("sound_good_2");

                    return true;
                }

                DAudioEngine.Play("sound_odd_1");

                return false;
            }

            private void UpdatePrice()
            {
                if (this.HasPriceIncrease)
                {
                    this.Price = (uint)(this.Price * this.PriceIncreaseFactor);
                }
            }
        }

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
            this.currentMoneyTextElement = new(textManager);

            this.pageTitleTextElement = new(textManager)
            {
                FontType = DFontType.Dark,
                TextAlignment = DTextAlignment.Center,
                Position = new(42, 4),
                Spacing = -1
            };

            this.priceTextElement = new(textManager)
            {
                FontType = DFontType.Dark,
                TextAlignment = DTextAlignment.Center,
                Position = new(24, 18),
            };

            this.previewTextElement = new(textManager)
            {
                FontType = DFontType.Dark,
                TextAlignment = DTextAlignment.Center,
                Position = new(59, 18),
                Spacing = -1
            };

            // Elements
            this.buttonElement = new()
            {
                Position = new(13, 4),
            };
            this.mainPanelElement = new();
            this.upgradePanelElement = new();
            this.itemPanelElement = new();

            // ============================ //

            this.buttonElement.SetTexture(assetDatabase.GetTexture("texture_button_1"));
            this.mainPanelElement.SetTexture(assetDatabase.GetTexture("texture_gui_3"));
            this.upgradePanelElement.SetTexture(assetDatabase.GetTexture("texture_gui_4"));
            this.itemPanelElement.SetTexture(assetDatabase.GetTexture("texture_gui_4"));

            // ============================ //

            this.moneyElementPositions = [
                new(20, 36),
                new(16, 36),
            ];

            #region ITEMS
            // Upgrades
            this.purchasableUpgrades = [
                new("Energy", 10, true, 2)
                {
                    CurrentPreviewValue = gameInformation.PlayerEntity.MaximumEnergy,
                    NextPreviewValue = gameInformation.PlayerEntity.MaximumEnergy + DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.MaximumEnergy = (byte)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
                    },
                },

                new("Power", 15, true, 4.5f)
                {
                    CurrentPreviewValue = gameInformation.PlayerEntity.Power,
                    NextPreviewValue = gameInformation.PlayerEntity.Power + DPlayerConstants.DEFAULT_STARTING_POWER,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.Power = (byte)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_POWER;
                    },
                },

                new("Damage", 25, true, 5)
                {
                    CurrentPreviewValue = gameInformation.PlayerEntity.Damage,
                    NextPreviewValue = gameInformation.PlayerEntity.Damage + DPlayerConstants.DEFAULT_STARTING_DAMAGE,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.Damage = (byte)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_DAMAGE;
                    },
                },

                new("Bag Size", 50, true, 3.5f)
                {
                    CurrentPreviewValue = gameInformation.PlayerEntity.BackpackSize,
                    NextPreviewValue = gameInformation.PlayerEntity.BackpackSize + DPlayerConstants.DEFAULT_STARTING_BAG_SIZE,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.BackpackSize = (byte)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_BAG_SIZE;
                    },
                },
            ];

            // Items
            this.purchasableItems = [
                new("Stairs", 8, false, 0)
                {
                    CurrentPreviewValue = (int)gameInformation.PlayerEntity.StairCount,
                    NextPreviewValue = (int)gameInformation.PlayerEntity.StairCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.StairCount = (uint)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS;
                    },
                },

                new("Plataforms", 12, false, 0)
                {
                    CurrentPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount,
                    NextPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.PlataformCount = (uint)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS;
                    },
                },

                new("Miners", 32, false, 0)
                {
                    CurrentPreviewValue = (int)gameInformation.PlayerEntity.RobotCount,
                    NextPreviewValue = (int)gameInformation.PlayerEntity.RobotCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS,

                    OnBuyCallback = (DPurchasableItem item) =>
                    {
                        gameInformation.PlayerEntity.RobotCount = (uint)item.NextPreviewValue;

                        item.CurrentPreviewValue = item.NextPreviewValue;
                        item.NextPreviewValue += DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS;
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
