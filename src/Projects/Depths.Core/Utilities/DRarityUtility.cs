using Depths.Core.Enums.General;

namespace Depths.Core.Utilities
{
    internal static class DRarityUtility
    {
        internal static byte GetOreNumericalChance(DRarity rarity)
        {
            return rarity switch
            {
                DRarity.Common => 40,
                DRarity.Uncommon => 25,
                DRarity.Rare => 15,
                DRarity.VeryRare => 10,
                DRarity.ExtremelyRare => 7,
                DRarity.Legendary => 3,
                _ => 0,
            };
        }
    }
}
