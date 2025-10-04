using Depths.Core.Audio;
using Depths.Core.Entities.Common;
using Depths.Core.Interfaces.General;

using System;

namespace Depths.Core.Shop
{
    internal sealed class PurchasableItem : IResettable
    {
        internal string Name { get; private set; }
        internal uint Price { get; private set; }
        internal bool HasPriceIncrease { get; private set; }
        internal float PriceIncreaseFactor { get; private set; }

        internal int CurrentPreviewValue { get; set; }
        internal int NextPreviewValue { get; set; }

        internal Action<PurchasableItem> OnSyncPreviewValuesCallback { get; init; }
        internal Action<PurchasableItem> OnBuyCallback { get; init; }

        private readonly uint originalPrice;

        internal PurchasableItem(string name, uint originalPrice, bool hasPriceIncrease, float priceIncreaseFactor)
        {
            this.Name = name;
            this.Price = originalPrice;
            this.HasPriceIncrease = hasPriceIncrease;
            this.PriceIncreaseFactor = priceIncreaseFactor;

            this.originalPrice = originalPrice;
        }

        internal void Sync()
        {
            this.OnSyncPreviewValuesCallback?.Invoke(this);
        }

        internal bool TryBuy(PlayerEntity playerEntity)
        {
            if (playerEntity.Money >= this.Price)
            {
                playerEntity.Money -= this.Price;

                UpdatePrice();
                this.OnBuyCallback?.Invoke(this);

                AudioEngine.Play("sound_good_2");

                return true;
            }

            AudioEngine.Play("sound_odd_1");

            return false;
        }

        private void UpdatePrice()
        {
            if (this.HasPriceIncrease)
            {
                this.Price = (uint)(this.Price * this.PriceIncreaseFactor);
            }
        }

        public void Reset()
        {
            this.Price = this.originalPrice;
        }
    }
}
