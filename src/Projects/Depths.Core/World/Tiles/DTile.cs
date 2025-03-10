using Depths.Core.Enums.General;
using Depths.Core.Enums.World;
using Depths.Core.Interfaces.General;
using Depths.Core.World.Ores;

namespace Depths.Core.World.Tiles
{
    internal sealed class DTile : IDResettable
    {
        internal bool IsDestroyed => this.Health <= 0;
        internal DDirection Direction { get; set; }
        internal bool HasGravity { get; set; }
        internal int Health { get; private set; }
        internal bool IsDestructible { get; set; }
        internal bool IsSolid { get; set; }
        internal DOre Ore { get; set; }
        internal byte Resistance { get; set; }
        internal DTileType Type { get; set; }
        internal DUpdateCycleFlag UpdateCycleFlag { get; set; }

        internal Destroyed OnDestroyed { get; set; }

        internal delegate void Destroyed();

        internal void Copy(DTile tile)
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
            this.Direction = DDirection.None;
            this.HasGravity = false;
            this.Health = 0;
            this.IsDestructible = false;
            this.IsSolid = false;
            this.Ore = null;
            this.Resistance = 0;
            this.Type = DTileType.Empty;
            this.UpdateCycleFlag = DUpdateCycleFlag.None;
            this.OnDestroyed = null;
        }
    }
}
