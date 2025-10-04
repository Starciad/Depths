﻿using Depths.Core.Enums.General;

namespace Depths.Core.Extensions
{
    internal static class UpdateCycleFlagExtensions
    {
        internal static UpdateCycleFlag GetNextCycle(this UpdateCycleFlag currentCycle)
        {
            return currentCycle switch
            {
                UpdateCycleFlag.None => UpdateCycleFlag.Primary,
                UpdateCycleFlag.Primary => UpdateCycleFlag.Secondary,
                UpdateCycleFlag.Secondary => UpdateCycleFlag.Primary,
                _ => UpdateCycleFlag.None,
            };
        }
    }
}
