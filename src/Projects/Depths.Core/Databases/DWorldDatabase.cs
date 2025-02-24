using Depths.Core.Enums.General;
using Depths.Core.Utilities;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DWorldDatabase
    {
        internal IEnumerable<DOre> Ores => this.ores;
        internal IEnumerable<DWorldChunk> Chunks => this.chunks;

        private readonly DOre[] ores;
        private readonly DWorldChunk[] chunks;

        internal DWorldDatabase(DAssetDatabase assetDatabase)
        {
            this.ores =
            [
                // Common
                new()
                {
                    DisplayName = "Coal",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 1,
                },

                new()
                {
                    DisplayName = "Limestone",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 1,
                },

                new()
                {
                    DisplayName = "Iron",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 3,
                },

                new()
                {
                    DisplayName = "Sulfur",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 3,
                },

                new()
                {
                    DisplayName = "Gold",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 6,
                },

                new()
                {
                    DisplayName = "Lapis Lazuli",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 5,
                },

                new()
                {
                    DisplayName = "Diamond",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.VeryRare,
                    Value = 12,
                },

                // Gemstone
                new()
                {
                    DisplayName = "Quartz",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Common,
                    Value = 2,
                },

                new()
                {
                    DisplayName = "Ambar",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Uncommon,
                    Value = 4,
                },

                new()
                {
                    DisplayName = "Emerald",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Rare,
                    Value = 7,
                },

                new()
                {
                    DisplayName = "Sapphire",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.VeryRare,
                    Value = 10,
                },

                new()
                {
                    DisplayName = "Ruby",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.ExtremelyRare,
                    Value = 15,
                },

                new()
                {
                    DisplayName = "Onyx",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Legendary,
                    Value = 20,
                },

                new()
                {
                    DisplayName = "Amethyst",
                    CaveSpawnLevel = new(new(), new()),
                    Rarity = DRarity.Legendary,
                    Value = 22,
                },
            ];

            this.chunks = DChunkLoader.Load();
        }
    }
}
