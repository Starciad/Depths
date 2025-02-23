using Depths.Core.Enums.General;
using Depths.Core.World.Ores;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DWorldDatabase
    {
        private readonly Dictionary<string, DOre> ores;

        internal DWorldDatabase(DAssetDatabase assetDatabase)
        {
            this.ores = new()
            {
                // Common
                ["Coal"] = new()
                {
                    DisplayName = "Coal",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 1,
                },

                ["Limestone"] = new()
                {
                    DisplayName = "Limestone",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 1,
                },

                ["Iron"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 3,
                },

                ["Sulfur"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 3,
                },

                ["Gold"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 6,
                },

                ["Lapis Lazuli"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 5,
                },

                ["Diamond"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.VeryRare,
                    Value = 12,
                },

                // Gemstone
                ["Quartz"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 2,
                },

                ["Ambar"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 4,
                },

                ["Emerald"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 7,
                },

                ["Sapphire"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.VeryRare,
                    Value = 10,
                },

                ["Ruby"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.ExtremelyRare,
                    Value = 15,
                },

                ["Onyx"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Legendary,
                    Value = 20,
                },

                ["Amethyst"] = new()
                {
                    DisplayName = "",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Legendary,
                    Value = 22,
                },
            };
        }
    }
}
