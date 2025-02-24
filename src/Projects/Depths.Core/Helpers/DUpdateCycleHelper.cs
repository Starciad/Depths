using Depths.Core.Enums.General;

namespace Depths.Core.Helpers
{
    internal static class DUpdateCycleHelper
    {
        public static DUpdateCycleFlag GetNextCycle(this DUpdateCycleFlag currentCycle)
        {
            return currentCycle switch
            {
                DUpdateCycleFlag.None => DUpdateCycleFlag.Primary,
                DUpdateCycleFlag.Primary => DUpdateCycleFlag.Secondary,
                DUpdateCycleFlag.Secondary => DUpdateCycleFlag.Primary,
                _ => DUpdateCycleFlag.None,
            };
        }
    }
}
