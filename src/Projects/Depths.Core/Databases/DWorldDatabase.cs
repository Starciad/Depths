/*

DEPTHS
Copyright (c) 2025 Davi "Starciad" Fernandes

============================================

[ CHUNKS ]

TILES

T0 - Empty
T1 - Dirt
T2 - Stone
T3 - Stair
T4 - Box
T5 - Spike Trap
T6 - Arrow Trap
T7 - Wall
T8 - Boulder Trap
T9 - Plataform

RANDOM

R0 - Enemy (random)

============================================

*/

using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DWorldDatabase
    {
        internal IEnumerable<DOre> Ores => this.ores;
        internal IEnumerable<DWorldChunk> Chunks => this.chunks;

        private DOre[] ores;
        private DWorldChunk[] chunks;

        internal void Initialize(DAssetDatabase assetDatabase)
        {
            this.ores = [
                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_1"),
                    LayerRange = new(new(1), new(4)),
                    Name = "Coal",
                    Rarity = DRarity.Common,
                    Resistance = 1,
                    Value = 1
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_2"),
                    LayerRange = new(new(1), new(4)),
                    Name = "Limestone",
                    Rarity = DRarity.Common,
                    Resistance = 1,
                    Value = 1
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_3"),
                    LayerRange = new(new(1), new(3)),
                    Name = "Quartz",
                    Rarity = DRarity.Common,
                    Resistance = 2,
                    Value = 2
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_4"),
                    LayerRange = new(new(2), new(4)),
                    Name = "Ambar",
                    Rarity = DRarity.Uncommon,
                    Resistance = 3,
                    Value = 4
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_5"),
                    LayerRange = new(new(2), new(6)),
                    Name = "Iron",
                    Rarity = DRarity.Uncommon,
                    Resistance = 3,
                    Value = 3
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_6"),
                    LayerRange = new(new(3), new(7)),
                    Name = "Sulfur",
                    Rarity = DRarity.Common,
                    Resistance = 2,
                    Value = 3
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_7"),
                    LayerRange = new(new(4), new(7)),
                    Name = "Emerald",
                    Rarity = DRarity.Rare,
                    Resistance = 5,
                    Value = 7
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_8"),
                    LayerRange = new(new(5), new(8)),
                    Name = "Gold",
                    Rarity = DRarity.Rare,
                    Resistance = 4,
                    Value = 6
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_9"),
                    LayerRange = new(new(6), new(9)),
                    Name = "Lazuli",
                    Rarity = DRarity.Rare,
                    Resistance = 4,
                    Value = 5
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_10"),
                    LayerRange = new(new(5), new(8)),
                    Name = "Sapphire",
                    Rarity = DRarity.VeryRare,
                    Resistance = 7,
                    Value = 10
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_11"),
                    LayerRange = new(new(7), new(10)),
                    Name = "Diamond",
                    Rarity = DRarity.VeryRare,
                    Resistance = 8,
                    Value = 12
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_12"),
                    LayerRange = new(new(7), new(10)),
                    Name = "Ruby",
                    Rarity = DRarity.ExtremelyRare,
                    Resistance = 9,
                    Value = 15
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_13"),
                    LayerRange = new(new(9), new(10)),
                    Name = "Onyx",
                    Rarity = DRarity.Legendary,
                    Resistance = 10,
                    Value = 20
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_14"),
                    LayerRange = new(new(9), new(10)),
                    Name = "Amethyst",
                    Rarity = DRarity.Legendary,
                    Resistance = 10,
                    Value = 22
                },
            ];

            this.chunks = [
                #region SURFACE
                new()
                {
                    Type = DWorldChunkType.Surface,
                    Mapping = new string[,]
                    {
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1" },
                        { "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1" },
                    },
                },
                #endregion

                #region UNDERGROUND
                new()
                {
                    Type = DWorldChunkType.Underground,
                    Mapping = new string[,]
                    {
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                        { "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2", "T2" },
                    },
                },
                #endregion

                #region DEPTHS
                new()
                {
                    Type = DWorldChunkType.Depth,
                    Mapping = new string[,]
                    {
                        { "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0", "T0" },
                        { "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1", "T1" },
                    },
                },
                #endregion
            ];
        }
    }
}
