using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Interfaces.General;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World
{
    internal sealed class DWorld : IDResettable
    {
        internal DTilemap Tilemap { get; private set; }

        internal DWorld(DAssetDatabase assetDatabase, DEntityManager entityManager, DGameInformation gameInformation)
        {
            this.Tilemap = new(DTilemapMath.GetTotalTileCount(DWorldConstants.WORLD_WIDTH, DWorldConstants.WORLD_HEIGHT), assetDatabase, entityManager, gameInformation);
        }

        internal void Update()
        {
            this.Tilemap.Update();
        }

        internal void Draw(SpriteBatch spriteBatch, DCameraManager cameraManager)
        {
            this.Tilemap.Draw(spriteBatch, cameraManager);
        }

        internal void DrawAll(SpriteBatch spriteBatch)
        {
            this.Tilemap.DrawAll(spriteBatch);
        }

        public void Reset()
        {
            this.Tilemap.Reset();
        }
    }
}
