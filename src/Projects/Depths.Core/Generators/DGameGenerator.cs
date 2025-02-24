using Depths.Core.Databases;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;
using Depths.Core.World.Tiles;

namespace Depths.Core.Generators
{
    internal sealed class DGameGenerator
    {
        internal required DAssetDatabase AssetDatabase { get; init; }
        internal required DEntityDatabase EntityDatabase { get; init; }
        internal required DWorldDatabase WorldDatabase { get; init; }
        internal required DEntityManager EntityManager { get; init; }
        internal required DWorld World { get; init; }

        private DTilemap worldTilemap;
        private DSize2 worldSize;

        internal void Initialize()
        {
            this.worldTilemap = this.World.Tilemap;
            this.worldSize = this.worldTilemap.Size;
        }
    }
}
