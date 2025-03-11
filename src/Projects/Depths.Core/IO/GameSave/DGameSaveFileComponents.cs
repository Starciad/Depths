#if DESKTOP
using Depths.Core.Entities.Common;
using Depths.Core.World.Ores;

using MessagePack;

using System.Collections.Generic;

namespace Depths.Core.IO.GameSave
{
    [MessagePackObject]
    public sealed partial class DPlayerInfo
    {
        [Key(00)] public byte Damage { get; private set; }
        [Key(01)] public byte BackpackSize { get; private set; }
        [Key(02)] public sbyte HorizontalDirectionDelta { get; private set; }
        [Key(03)] public byte Energy { get; private set; }
        [Key(04)] public bool IsDead { get; private set; }
        [Key(05)] public byte MaximumEnergy { get; private set; }
        [Key(06)] public uint Money { get; private set; }
        [Key(07)] public byte Power { get; private set; }
        [Key(08)] public uint StairCount { get; private set; }
        [Key(09)] public uint PlataformCount { get; private set; }
        [Key(10)] public uint RobotCount { get; private set; }
        [Key(11)] public Queue<string> CollectedMinerals { get; private set; }

        public DPlayerInfo()
        {

        }

        internal DPlayerInfo(DPlayerEntity playerEntity)
        {
            this.Damage = playerEntity.Damage;
            this.BackpackSize = playerEntity.BackpackSize;
            this.HorizontalDirectionDelta = playerEntity.HorizontalDirectionDelta;
            this.Energy = playerEntity.Energy;
            this.IsDead = playerEntity.IsDead;
            this.MaximumEnergy = playerEntity.MaximumEnergy;
            this.Money = playerEntity.Money;
            this.Power = playerEntity.Power;
            this.StairCount = playerEntity.StairCount;
            this.PlataformCount = playerEntity.PlataformCount;
            this.RobotCount = playerEntity.RobotCount;

            foreach (DOre ore in playerEntity.CollectedMinerals)
            {
                this.CollectedMinerals.Enqueue(ore.Name);
            }
        }
    }
}
#endif