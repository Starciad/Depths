using Depths.Core.Levels;
using Depths.Core.Levels.Common;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DLevelDatabase
    {
        private readonly Dictionary<string, DLevel> levels;

        internal DLevelDatabase(DAssetDatabase assetDatabase)
        {
            this.levels = new()
            {
                ["Surface"] = new DSurfaceLevel(assetDatabase),
            };
        }

        internal DLevel GetLevelByIdentifier(string identifier)
        {
            return this.levels[identifier];
        }
    }
}
