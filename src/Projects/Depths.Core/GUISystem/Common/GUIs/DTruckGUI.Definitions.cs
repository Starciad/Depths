using Depths.Core.Audio;
using Depths.Core.Entities.Common;
using Depths.Core.Interfaces.General;

using System;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed partial class DTruckGUI
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

        private sealed class DPurchasableItem : IDResettable
        {
            internal string Name { get; private set; }
            internal uint Price { get; private set; }
            internal bool HasPriceIncrease { get; private set; }
            internal float PriceIncreaseFactor { get; private set; }

            internal int CurrentPreviewValue { get; set; }
            internal int NextPreviewValue { get; set; }

            internal Action<DPurchasableItem> OnSyncPreviewValuesCallback { get; init; }
            internal Action<DPurchasableItem> OnBuyCallback { get; init; }

            private readonly uint originalPrice;

            internal DPurchasableItem(string name, uint originalPrice, bool hasPriceIncrease, float priceIncreaseFactor)
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

            public void Reset()
            {
                this.Price = this.originalPrice;
            }
        }
    }
}
