using Depths.Core.Enums.InputSystem;

using System.Linq;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class TruckGUI
    {
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
            if (this.inputManager.Started(CommandType.Cancel))
            {
                this.guiManager.Close(this.Identifier);
                return;
            }

            if (this.inputManager.Started(CommandType.Confirm))
            {
                this.currentPageIndex = 0;

                switch (this.selectedButton)
                {
                    case DButton.Upgrades:
                        this.selectedSection = DSection.Upgrades;
                        break;

                    case DButton.Items:
                        this.selectedSection = DSection.Items;
                        break;

                    default:
                        break;
                }

                return;
            }

            if (this.inputManager.Started(CommandType.Up) || this.inputManager.Started(CommandType.Down))
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
            if (this.inputManager.Started(CommandType.Cancel))
            {
                this.selectedSection = DSection.Main;
            }
            else if (this.inputManager.Started(CommandType.Confirm))
            {
                _ = this.shopDatabase.PurchasableUpgrades.ElementAt(this.currentPageIndex).TryBuy(this.gameInformation.PlayerEntity);
            }
            else if (this.inputManager.Started(CommandType.Left))
            {
                PreviousPage(DSection.Upgrades);
            }
            else if (this.inputManager.Started(CommandType.Right))
            {
                NextPage(DSection.Upgrades);
            }
        }

        private void HandleUserItemSectionInputs()
        {
            if (this.inputManager.Started(CommandType.Cancel))
            {
                this.selectedSection = DSection.Main;
            }
            else if (this.inputManager.Started(CommandType.Confirm))
            {
                _ = this.shopDatabase.PurchasableItems.ElementAt(this.currentPageIndex).TryBuy(this.gameInformation.PlayerEntity);
            }
            else if (this.inputManager.Started(CommandType.Left))
            {
                PreviousPage(DSection.Items);
            }
            else if (this.inputManager.Started(CommandType.Right))
            {
                NextPage(DSection.Items);
            }
        }
    }
}
