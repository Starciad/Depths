using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.World.Chunks;
using Depths.Core.World.Ores;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class WorldDatabase
    {
        internal IEnumerable<Ore> Ores => this.ores;
        internal IEnumerable<WorldChunk> Chunks => this.chunks;

        private Ore[] ores;
        private WorldChunk[] chunks;

        internal void Initialize(AssetDatabase assetDatabase)
        {
            this.ores = [
                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_1"),
                    LayerRange = new(new(1), new(4)),
                    Name = "Coal",
                    Rarity = Rarity.Common,
                    Resistance = 1,
                    Value = 2
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_2"),
                    LayerRange = new(new(1), new(4)),
                    Name = "Limestone",
                    Rarity = Rarity.Common,
                    Resistance = 1,
                    Value = 3
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_3"),
                    LayerRange = new(new(1), new(3)),
                    Name = "Quartz",
                    Rarity = Rarity.Common,
                    Resistance = 1,
                    Value = 3
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_4"),
                    LayerRange = new(new(2), new(4)),
                    Name = "Ambar",
                    Rarity = Rarity.Uncommon,
                    Resistance = 2,
                    Value = 5
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_5"),
                    LayerRange = new(new(2), new(6)),
                    Name = "Iron",
                    Rarity = Rarity.Uncommon,
                    Resistance = 3,
                    Value = 4
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_6"),
                    LayerRange = new(new(3), new(7)),
                    Name = "Sulfur",
                    Rarity = Rarity.Common,
                    Resistance = 2,
                    Value = 4
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_7"),
                    LayerRange = new(new(4), new(7)),
                    Name = "Emerald",
                    Rarity = Rarity.Rare,
                    Resistance = 5,
                    Value = 8
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_8"),
                    LayerRange = new(new(5), new(8)),
                    Name = "Gold",
                    Rarity = Rarity.Rare,
                    Resistance = 4,
                    Value = 7
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_9"),
                    LayerRange = new(new(6), new(9)),
                    Name = "Lapis Lazuli",
                    Rarity = Rarity.Rare,
                    Resistance = 4,
                    Value = 6
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_10"),
                    LayerRange = new(new(5), new(8)),
                    Name = "Sapphire",
                    Rarity = Rarity.VeryRare,
                    Resistance = 7,
                    Value = 11
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_11"),
                    LayerRange = new(new(7), new(10)),
                    Name = "Diamond",
                    Rarity = Rarity.VeryRare,
                    Resistance = 8,
                    Value = 13
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_12"),
                    LayerRange = new(new(7), new(10)),
                    Name = "Ruby",
                    Rarity = Rarity.ExtremelyRare,
                    Resistance = 9,
                    Value = 16
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_13"),
                    LayerRange = new(new(9), new(10)),
                    Name = "Onyx",
                    Rarity = Rarity.Legendary,
                    Resistance = 10,
                    Value = 21
                },

                new()
                {
                    IconTexture = assetDatabase.GetTexture("texture_ore_14"),
                    LayerRange = new(new(9), new(10)),
                    Name = "Amethyst",
                    Rarity = Rarity.Legendary,
                    Resistance = 10,
                    Value = 23
                },
            ];

            this.chunks = [
                #region SURFACE
                new()
                {
                    Type = WorldChunkType.Surface,
                    Mapping = new CI[,]
                    {
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1 },
                        { CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1 },
                    },
                },
                #endregion

                #region UNDERGROUND

                #region GROUP 01

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T3, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T3, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T4, CI.T0, CI.T0, CI.T4, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T3, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T9, CI.T2, CI.T0, CI.T0, CI.T2, CI.T9, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T1, CI.T2, CI.T0, CI.T0, CI.T2, CI.T1, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T1, CI.T1, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T1, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T5, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T5, CI.T0, CI.T2, CI.T5, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T5, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T4, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T2, CI.T0, CI.T0, CI.T4, CI.T0, CI.T2, CI.T0, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T0, CI.T4, CI.T4, CI.T4, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T5, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T1, CI.T0, CI.T5, CI.T0, CI.T2, CI.T8, CI.T8, CI.T2, CI.T0, CI.T5, CI.T0, CI.T1 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T4, CI.T8, CI.T8, CI.T4, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T1, CI.T0, CI.T0, CI.T0, CI.T2, CI.T8, CI.T8, CI.T2, CI.T0, CI.T0, CI.T0, CI.T1 },
                        { CI.T2, CI.T0, CI.T5, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T5, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T4, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T5, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T5, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T8, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T8, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T5, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T4, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T3, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T3, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T9, CI.T9, CI.T3, CI.T9, CI.T9, CI.T9, CI.T9, CI.T3, CI.T9, CI.T9, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    }
                },

                #endregion

                #region GROUP 02

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T6, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T4, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T9, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T5, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T4, CI.T4, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T9, CI.T9, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T1, CI.T2, CI.T1, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T1, CI.T2 },
                        { CI.T1, CI.T2, CI.T1, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T1, CI.T2 },
                        { CI.T1, CI.T2, CI.T1, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T1, CI.T2 },
                        { CI.T1, CI.T2, CI.T1, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T1, CI.T2 },
                        { CI.T1, CI.T2, CI.T1, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T2, CI.T1, CI.T1, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T3, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T3, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T3, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T3, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T3, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T8, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T8, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T8, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T8, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T8, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T8, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T6, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T6, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T6, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T6, CI.T2, CI.T2, CI.T0, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T6 },
                        { CI.T6, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T6 },
                        { CI.T6, CI.T2, CI.T6, CI.T0, CI.T0, CI.T5, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T6 },
                        { CI.T6, CI.T2, CI.T6, CI.T0, CI.T5, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T2, CI.T6 },
                        { CI.T6, CI.T2, CI.T6, CI.T0, CI.T6, CI.T0, CI.T6, CI.T0, CI.T0, CI.T2, CI.T2, CI.T6 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T6, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T6, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T6, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T6, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T6, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T2, CI.T8, CI.T2, CI.T0, CI.T0, CI.T2, CI.T8, CI.T2, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T6, CI.T0, CI.T0, CI.T0, CI.T0, CI.T6, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T9, CI.T9, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T9, CI.T9, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T0, CI.T2 },
                        { CI.T2, CI.T2, CI.T0, CI.T0, CI.T2, CI.T2, CI.T2, CI.T0, CI.T0, CI.T0, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                new()
                {
                    Type = WorldChunkType.Underground,
                    Mapping = new CI[,]
                    {
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 },
                        { CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T4, CI.T2, CI.T2 },
                        { CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2, CI.T2 }
                    },
                },

                #endregion

                #endregion

                #region DEPTHS
                new()
                {
                    Type = WorldChunkType.Depth,
                    Mapping = new CI[,]
                    {
                        { CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0, CI.T0 },
                        { CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1, CI.T1 },
                    },
                },
                #endregion
            ];
        }
    }
}
