using Depths.Core.Collections;
using Depths.Core.Databases;
using Depths.Core.Entities;
using Depths.Core.Interfaces.Collections;
using Depths.Core.Interfaces.General;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Depths.Core.Managers
{
    internal sealed class DEntityManager : IDResettable
    {
        internal IEnumerable<DEntity> ActiveEntities => this.instantiatedEntities;

        private readonly List<DEntity> instantiatedEntities = [];
        private readonly Dictionary<string, DObjectPool> entityPools = [];

        private readonly DEntityDatabase entityDatabase;

        internal DEntityManager(DEntityDatabase entityDatabase)
        {
            this.entityDatabase = entityDatabase;
        }

        internal void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                DEntity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    return;
                }

                entity.Update(gameTime);
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                DEntity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    continue;
                }

                entity.Draw(gameTime, spriteBatch);
            }
        }

        internal DEntity InstantiateEntity(string entityIdentifier, Action<DEntity> entityConfigurationAction)
        {
            if (!this.entityPools.TryGetValue(entityIdentifier, out DObjectPool objectPool))
            {
                objectPool = new();
                this.entityPools[entityIdentifier] = objectPool;
            }

            if (!objectPool.TryGet(out IDPoolableObject value))
            {
                DEntityDescriptor entityDescriptor = this.entityDatabase.GetEntityDescriptorByIdentifier(entityIdentifier);
                value = entityDescriptor.CreateEntity();
                value.Reset();
            }

            if (value is not DEntity entity)
            {
                return null;
            }

            this.instantiatedEntities.Add(entity);

            entityConfigurationAction?.Invoke(entity);
            entity.Initialize();

            return entity;
        }

        internal void RemoveEntity(DEntity entity)
        {
            _ = this.instantiatedEntities.Remove(entity);
            this.entityPools[entity.Descriptor.Identifier].Add(entity);
        }

        internal void DestroyEntity(DEntity entity)
        {
            RemoveEntity(entity);
            entity.Destroy();
        }

        internal void RemoveAllEntities()
        {
            foreach (DEntity entity in this.ActiveEntities.ToList())
            {
                if (entity == null)
                {
                    continue;
                }

                RemoveEntity(entity);
            }
        }

        internal void DestroyAllEntities()
        {
            foreach (DEntity entity in this.ActiveEntities.ToList())
            {
                if (entity == null)
                {
                    continue;
                }

                DestroyEntity(entity);
            }
        }

        public void Reset()
        {
            RemoveAllEntities();
        }
    }
}
