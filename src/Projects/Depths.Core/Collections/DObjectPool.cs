using Depths.Core.Interfaces.Collections;

using System.Collections.Generic;

namespace Depths.Core.Collections
{
    internal sealed class DObjectPool
    {
        private readonly Queue<IDPoolableObject> pool = [];

        internal IDPoolableObject Get()
        {
            _ = TryGet(out IDPoolableObject value);
            return value;
        }

        internal bool TryGet(out IDPoolableObject value)
        {
            value = null;

            if (this.pool.TryDequeue(out IDPoolableObject result))
            {
                result.Reset();
                value = result;

                return true;
            }

            return false;
        }

        internal void Add(IDPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}