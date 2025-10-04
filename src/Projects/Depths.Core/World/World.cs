using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Interfaces.General;
using Depths.Core.Managers;
using Depths.Core.Mathematics;
using Depths.Core.World.Tiles;

using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.World
{
    internal sealed class World : IResettable
    {
        internal Tilemap Tilemap { get; private set; }

        internal World(AssetDatabase assetDatabase, EntityManager entityManager, GameInformation gameInformation)
        {
            this.Tilemap = new(TilemapMath.GetTotalTileCount(WorldConstants.WORLD_WIDTH, WorldConstants.WORLD_HEIGHT), assetDatabase, entityManager, gameInformation);
        }

        internal void Update()
        {
            this.Tilemap.Update();
        }

        internal void Draw(SpriteBatch spriteBatch, CameraManager cameraManager)
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
