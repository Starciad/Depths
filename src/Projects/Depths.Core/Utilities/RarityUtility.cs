using Depths.Core.Enums.General;

namespace Depths.Core.Utilities
{
    internal static class RarityUtility
    {
        internal static byte GetOreNumericalChance(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 40,
                Rarity.Uncommon => 25,
                Rarity.Rare => 15,
                Rarity.VeryRare => 10,
                Rarity.ExtremelyRare => 7,
                Rarity.Legendary => 3,
                _ => 0,
            };
        }
    }
}
