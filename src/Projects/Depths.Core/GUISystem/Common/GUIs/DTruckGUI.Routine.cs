using Depths.Core.Shop;

using Microsoft.Xna.Framework;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI
    {
        private void UpdateGUI()
        {
            SyncItems();
            UpdateMoneyElement();
            UpdateElementVisibility();
            UpdateButtonElement();
            UpdatePage();
        }

        private void SyncItems()
        {
            foreach (DPurchasableItem item in this.shopDatabase.PurchasableUpgrades)
            {
                item.Sync();
            }

            foreach (DPurchasableItem item in this.shopDatabase.PurchasableItems)
            {
                item.Sync();
            }
        }

        private void UpdateMoneyElement()
        {
            this.currentMoneyTextElement.SetValue(this.gameInformation.PlayerEntity.Money.ToString());
            this.currentMoneyTextElement.Position = this.selectedSection == DSection.Main ? this.moneyElementPositions[0] : this.moneyElementPositions[1];
        }

        private void UpdatePage()
        {
            this.currentMoneyTextElement.SetValue(this.gameInformation.PlayerEntity.Money.ToString());
            UpdatePageInfos(this.selectedSection);
        }

        private void UpdateElementVisibility()
        {
            UpdateTextElementVisibility();
            UpdatePanelElementVisibility();
        }

        private void UpdateTextElementVisibility()
        {
            if (this.selectedSection == DSection.Main)
            {
                SetTextVisibility(false);
                return;
            }

            SetTextVisibility(true);
        }

        private void UpdatePanelElementVisibility()
        {
            switch (this.selectedSection)
            {
                case DSection.Main:
                    SetPanelVisibility(true, false, false);
                    break;

                case DSection.Upgrades:
                    SetPanelVisibility(false, true, false);
                    break;

                case DSection.Items:
                    SetPanelVisibility(false, false, true);
                    break;

                default:
                    SetPanelVisibility(false, false, false);
                    break;
            }
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

        // ====================================== //

        private void SetTextVisibility(bool isVisible)
        {
            this.previewTextElement.IsVisible = isVisible;
            this.priceTextElement.IsVisible = isVisible;
            this.pageTitleTextElement.IsVisible = isVisible;
        }

        private void SetPanelVisibility(bool main, bool items, bool upgrades)
        {
            this.mainPanelElement.IsVisible = main;
            this.upgradePanelElement.IsVisible = items;
            this.itemPanelElement.IsVisible = upgrades;
        }
    }
}
