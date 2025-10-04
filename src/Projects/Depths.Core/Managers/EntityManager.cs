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
    internal sealed class EntityManager : IResettable
    {
        internal IEnumerable<Entity> ActiveEntities => this.instantiatedEntities;

        private readonly List<Entity> instantiatedEntities = [];
        private readonly Dictionary<string, ObjectPool> entityPools = [];

        private readonly EntityDatabase entityDatabase;

        internal EntityManager(EntityDatabase entityDatabase)
        {
            this.entityDatabase = entityDatabase;
        }

        internal void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                Entity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    return;
                }

                entity.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.instantiatedEntities.Count; i++)
            {
                Entity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    continue;
                }

                entity.Draw(spriteBatch);
            }
        }

        internal Entity InstantiateEntity(string entityIdentifier, Action<Entity> entityConfigurationAction)
        {
            if (!this.entityPools.TryGetValue(entityIdentifier, out ObjectPool objectPool))
            {
                objectPool = new();
                this.entityPools[entityIdentifier] = objectPool;
            }

            if (!objectPool.TryGet(out IPoolableObject value))
            {
                EntityDescriptor entityDescriptor = this.entityDatabase.GetEntityDescriptorByIdentifier(entityIdentifier);
                value = entityDescriptor.CreateEntity();
                value.Reset();
            }

            if (value is not Entity entity)
            {
                return null;
            }

            this.instantiatedEntities.Add(entity);

            entityConfigurationAction?.Invoke(entity);
            entity.Initialize();

            return entity;
        }

        internal void RemoveEntity(Entity entity)
        {
            _ = this.instantiatedEntities.Remove(entity);
            this.entityPools[entity.Descriptor.Identifier].Add(entity);
        }

        internal void DestroyEntity(Entity entity)
        {
            RemoveEntity(entity);
            entity.Destroy();
        }

        internal void RemoveAllEntities()
        {
            foreach (Entity entity in this.ActiveEntities.ToList())
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
            foreach (Entity entity in this.ActiveEntities.ToList())
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
