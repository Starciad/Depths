using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.World.Chunks;
using Depths.Core.Extensions;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.World;
using Depths.Core.World.Chunks;
using Depths.Core.World.Tiles;

using System.Collections.Generic;
using System.Linq;

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

            GenerateWorld();
        }

        private void GenerateWorld()
        {
            GenerateWorldSurface();
            GenerateWorldUnderground();
            GenerateWorldDepths();
            GenerateOres();
            GenerateExtraBlocks();
            GenerateTreasures();
            GenerateTraps();
            GenerateEntities();
        }

        private void GenerateWorldSurface()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Surface);

            for (int i = 0; i < DWorldConstants.WORLD_WIDTH; i++)
            {
                chunks.GetRandomItem().ApplyToTilemap(new(i, 0), this.worldTilemap);
            }
        }

        private void GenerateWorldUnderground()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Underground);

            for (int y = 1; y < DWorldConstants.WORLD_HEIGHT - 1; y++)
            {
                for (int x = 0; x < DWorldConstants.WORLD_WIDTH; x++)
                {
                    chunks.GetRandomItem().ApplyToTilemap(new(x, y), this.worldTilemap);
                }
            }
        }

        private void GenerateWorldDepths()
        {
            IEnumerable<DWorldChunk> chunks = this.WorldDatabase.Chunks.Where(x => x.Type == DWorldChunkType.Depth);

            for (int i = 0; i < DWorldConstants.WORLD_WIDTH; i++)
            {
                chunks.GetRandomItem().ApplyToTilemap(new(i, DWorldConstants.WORLD_HEIGHT - 1), this.worldTilemap);
            }
        }

        private void GenerateOres()
        {

        }

        private void GenerateExtraBlocks()
        {

        }

        private void GenerateTreasures()
        {

        }

        private void GenerateTraps()
        {

        }

        private void GenerateEntities()
        {

        }
    }
}
