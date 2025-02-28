namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI
    {
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

        private void UpdatePageInfos(DSection section)
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
