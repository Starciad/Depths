using Depths.Core.Interfaces.Collections;

using System.Collections.Generic;

namespace Depths.Core.Collections
{
    internal sealed class ObjectPool
    {
        private readonly Queue<IPoolableObject> pool = [];

        internal IPoolableObject Get()
        {
            _ = TryGet(out IPoolableObject value);
            return value;
        }

        internal bool TryGet(out IPoolableObject value)
        {
            value = null;

            if (this.pool.TryDequeue(out IPoolableObject result))
            {
                result.Reset();
                value = result;

                return true;
            }

            return false;
        }

        internal void Add(IPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}