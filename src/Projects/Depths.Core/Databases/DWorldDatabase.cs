using Depths.Core.Loaders;
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
            this.ores = DOreLoader.Initialize(assetDatabase);
            this.chunks = DChunkLoader.Initialize();
        }
    }
}
