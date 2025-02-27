using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

using System;

using static System.Collections.Specialized.BitVector32;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DTruckGUI : DGUI
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

            internal void Buy()
            {
                UpdatePrice();
                this.OnBuyCallback?.Invoke(this);
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
                Position = new(25, 18),
            };
            this.previewTextElement = new(textManager)
            {
                FontType = DFontType.Dark,
                TextAlignment = DTextAlignment.Center,
                Position = new(60, 18),
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
            UpdateElements();
        }

        private void HandleUserInputs()
        {
            switch (this.selectedSection)
            {
                case DSection.Main:
                    HandleUserMainSectionInputs();
                    break;

                case DSection.Upgrades:
                    HandleUserUpgradeSectionInputs();
                    break;

                case DSection.Items:
                    HandleUserItemSectionInputs();
                    break;

                default:
                    break;
            }
        }

        private void HandleUserMainSectionInputs()
        {
            if (this.inputManager.Started(DKeyMappingConstant.Cancel))
            {
                this.guiManager.Close(this.Identifier);
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Confirm))
            {
                this.currentPageIndex = 0;

                switch (this.selectedButton)
                {
                    case DButton.Upgrades:
                        this.selectedSection = DSection.Upgrades;
                        SetPageInfos(DSection.Upgrades);
                        break;

                    case DButton.Items:
                        this.selectedSection = DSection.Items;
                        SetPageInfos(DSection.Items);
                        break;

                    default:
                        break;
                }

                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Up) || this.inputManager.Started(DKeyMappingConstant.Down))
            {
                switch (this.selectedButton)
                {
                    case DButton.Upgrades:
                        this.selectedButton = DButton.Items;
                        break;

                    case DButton.Items:
                        this.selectedButton = DButton.Upgrades;
                        break;

                    default:
                        break;
                }

                return;
            }
        }

        private void HandleUserUpgradeSectionInputs()
        {
            if (this.inputManager.Started(DKeyMappingConstant.Cancel))
            {
                this.selectedSection = DSection.Main;
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Confirm))
            {
                this.purchasableUpgrades[this.currentPageIndex].Buy();
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Left))
            {
                PreviousPage(DSection.Upgrades);
                SetPageInfos(DSection.Upgrades);
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Right))
            {
                NextPage(DSection.Upgrades);
                SetPageInfos(DSection.Upgrades);
                return;
            }
        }

        private void HandleUserItemSectionInputs()
        {
            if (this.inputManager.Started(DKeyMappingConstant.Cancel))
            {
                this.selectedSection = DSection.Main;
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Confirm))
            {
                this.purchasableItems[this.currentPageIndex].Buy();
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Left))
            {
                PreviousPage(DSection.Items);
                SetPageInfos(DSection.Items);
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Right))
            {
                NextPage(DSection.Items);
                SetPageInfos(DSection.Items);
                return;
            }
        }

        private void UpdateElements()
        {
            UpdateMoneyElement();
            UpdateElementVisibility();
            UpdateButtonElement();
            UpdatePanelElements();
        }

        private void UpdateMoneyElement()
        {
            this.currentMoneyTextElement.SetValue(this.gameInformation.PlayerEntity.Money.ToString());

            if (this.selectedSection == DSection.Main)
            {
                this.currentMoneyTextElement.Position = this.moneyElementPositions[0];
            }
            else
            {
                this.currentMoneyTextElement.Position = this.moneyElementPositions[1];
            }
        }

        private void UpdateElementVisibility()
        {
            UpdateTextElementVisibility();
        }

        private void UpdateTextElementVisibility()
        {
            if (this.selectedSection == DSection.Main)
            {
                this.previewTextElement.IsVisible = false;
                this.priceTextElement.IsVisible = false;
                this.pageTitleTextElement.IsVisible = false;
                return;
            }

            this.previewTextElement.IsVisible = true;
            this.priceTextElement.IsVisible = true;
            this.pageTitleTextElement.IsVisible = true;
        }

        private void UpdateButtonElement()
        {
            if (this.selectedSection != DSection.Main)
            {
                this.buttonElement.IsVisible = false;
                return;
            }

            this.buttonElement.IsVisible = true;

            this.buttonElement.TextureClipArea = this.selectedButton switch
            {
                DButton.Upgrades => (Rectangle?)this.buttonSourceRectangles[0],
                DButton.Items => (Rectangle?)this.buttonSourceRectangles[1],
                _ => null,
            };
        }

        private void UpdatePanelElements()
        {
            switch (this.selectedSection)
            {
                case DSection.Main:
                    this.mainPanelElement.IsVisible = true;
                    this.upgradePanelElement.IsVisible = false;
                    this.itemPanelElement.IsVisible = false;
                    break;

                case DSection.Upgrades:
                    this.mainPanelElement.IsVisible = false;
                    this.upgradePanelElement.IsVisible = true;
                    this.itemPanelElement.IsVisible = false;
                    break;

                case DSection.Items:
                    this.mainPanelElement.IsVisible = false;
                    this.upgradePanelElement.IsVisible = false;
                    this.itemPanelElement.IsVisible = true;
                    break;

                default:
                    this.mainPanelElement.IsVisible = false;
                    this.upgradePanelElement.IsVisible = false;
                    this.itemPanelElement.IsVisible = false;
                    break;
            }
        }

        private void NextPage(DSection section)
        {
            if (++this.currentPageIndex > GetTotalPages(section))
            {
                this.currentPageIndex = 0;
            }
        }

        private void PreviousPage(DSection section)
        {
            if (--this.currentPageIndex < 0)
            {
                this.currentPageIndex = GetTotalPages(section);
            }
        }

        private int GetTotalPages(DSection section)
        {
            return section switch
            {
                DSection.Upgrades => this.purchasableUpgrades.Length - 1,
                DSection.Items => this.purchasableItems.Length - 1,
                _ => 0,
            };
        }

        private void SetPageInfos(DSection section)
        {
            DPurchasableItem item = section switch
            {
                DSection.Upgrades => this.purchasableUpgrades[this.currentPageIndex],
                DSection.Items => this.purchasableItems[this.currentPageIndex],
                _ => null,
            };

            if (item == null)
            {
                return;
            }

            this.pageTitleTextElement.SetValue(item.Name);
            this.previewTextElement.SetValue(string.Concat(item.CurrentPreviewValue, '>', item.NextPreviewValue));
            this.priceTextElement.SetValue(string.Concat('$', item.Price));
        }
    }
}
