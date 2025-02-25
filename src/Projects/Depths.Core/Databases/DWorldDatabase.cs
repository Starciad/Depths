using Depths.Core.Enums.General;
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

        private readonly DOre[] ores;
        private readonly DWorldChunk[] chunks;

        internal DWorldDatabase()
        {
            this.ores = DOreLoader.Initialize();
            this.chunks = DChunkLoader.Initialize();
        }
    }
}
