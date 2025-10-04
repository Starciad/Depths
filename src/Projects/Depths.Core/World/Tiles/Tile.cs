using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.Interfaces.General;
using Depths.Core.World.Ores;

namespace Depths.Core.World.Tiles
{
    internal sealed class Tile : IResettable
    {
        internal bool IsDestroyed => this.Health <= 0;
        internal Direction Direction { get; set; }
        internal bool HasGravity { get; set; }
        internal int Health { get; private set; }
        internal bool IsDestructible { get; set; }
        internal bool IsSolid { get; set; }
        internal Ore Ore { get; set; }
        internal byte Resistance { get; set; }
        internal TileType Type { get; set; }
        internal UpdateCycleFlag UpdateCycleFlag { get; set; }

        internal Destroyed OnDestroyed { get; set; }

        internal delegate void Destroyed();

        internal void Copy(Tile tile)
        {
            this.Direction = tile.Direction;
            this.HasGravity = tile.HasGravity;
            this.Health = tile.Health;
            this.IsDestructible = tile.IsDestructible;
            this.IsSolid = tile.IsSolid;
            this.Ore = tile.Ore;
            this.Resistance = tile.Resistance;
            this.Type = tile.Type;
            this.UpdateCycleFlag = tile.UpdateCycleFlag;
            this.OnDestroyed = tile.OnDestroyed;
        }

        internal void SetHealth(uint value)
        {
            this.Health = (int)value;
        }

        internal bool TryDamage(uint value)
        {
            if (!this.IsDestructible || this.IsDestroyed)
            {
                return false;
            }

            this.Health -= (int)value;

            if (this.Health <= 0)
            {
                this.OnDestroyed?.Invoke();
                this.Health = 0;
            }

            return true;
        }

        public void Reset()
        {
            this.Direction = Direction.None;
            this.HasGravity = false;
            this.Health = 0;
            this.IsDestructible = false;
            this.IsSolid = false;
            this.Ore = null;
            this.Resistance = 0;
            this.Type = TileType.Empty;
            this.UpdateCycleFlag = UpdateCycleFlag.None;
            this.OnDestroyed = null;
        }
    }
}
