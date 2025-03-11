using Depths.Core.Constants;
using Depths.Core.Shop;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DShopDatabase
    {
        internal IEnumerable<DPurchasableItem> PurchasableUpgrades => this.purchasableUpgrades;
        internal IEnumerable<DPurchasableItem> PurchasableItems => this.purchasableItems;

        private readonly DPurchasableItem[] purchasableUpgrades;
        private readonly DPurchasableItem[] purchasableItems;

        internal DShopDatabase(DGameInformation gameInformation)
        {
            #region ITEMS
            // Upgrades
            this.purchasableUpgrades = [
                new("Energy", 8, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.MaximumEnergy;
                        item.NextPreviewValue = gameInformation.PlayerEntity.MaximumEnergy + DPlayerConstants.DEFAULT_MAXIMUM_STARTING_ENERGY;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.MaximumEnergy = (byte)item.NextPreviewValue;
                    },
                },

                new("Power", 6, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.Power;
                        item.NextPreviewValue = gameInformation.PlayerEntity.Power + DPlayerConstants.DEFAULT_STARTING_POWER;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.Power = (byte)item.NextPreviewValue;
                    },
                },

                new("Damage", 6, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.Damage;
                        item.NextPreviewValue = gameInformation.PlayerEntity.Damage + DPlayerConstants.DEFAULT_STARTING_DAMAGE;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.Damage = (byte)item.NextPreviewValue;
                    },
                },

                new("Bag Size", 10, true, 2f)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = gameInformation.PlayerEntity.BackpackSize;
                        item.NextPreviewValue = gameInformation.PlayerEntity.BackpackSize + DPlayerConstants.DEFAULT_STARTING_BAG_SIZE;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.BackpackSize = (byte)item.NextPreviewValue;
                    },
                },
            ];

            // Items
            this.purchasableItems = [
                new("Stairs", 5, false, 0)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.StairCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.StairCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_STAIRS;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.StairCount = (uint)item.NextPreviewValue;
                    },
                },

                new("Plataforms", 8, false, 0)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.PlataformCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_PLATAFORMS;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.PlataformCount = (uint)item.NextPreviewValue;
                    },
                },

                new("Miners", 25, false, 0)
                {
                    OnSyncPreviewValuesCallback = (item) =>
                    {
                        item.CurrentPreviewValue = (int)gameInformation.PlayerEntity.RobotCount;
                        item.NextPreviewValue = (int)gameInformation.PlayerEntity.RobotCount + DPlayerConstants.DEFAULT_STARTING_NUMBER_OF_ROBOTS;
                    },

                    OnBuyCallback = (item) =>
                    {
                        gameInformation.PlayerEntity.RobotCount = (uint)item.NextPreviewValue;
                    },
                },
            ];
            #endregion
        }
    }
}
