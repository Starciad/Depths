using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Managers;
using Depths.Core.World;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DEntityDatabase
    {
        private readonly Dictionary<string, DEntityDescriptor> registeredDescriptors = [];

        private readonly DAssetDatabase assetDatabase;

        internal DEntityDatabase(DAssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;
        }

        internal void Initialize(DWorld world, DAssetDatabase assetDatabase, DInputManager inputManager, DMusicManager musicManager)
        {
            RegisterEntityDescriptor(new DPlayerEntityDescriptor("Player", this.assetDatabase.GetTexture("texture_entity_1"), world, assetDatabase, inputManager, musicManager));
            RegisterEntityDescriptor(new DTruckEntityDescriptor("Truck", this.assetDatabase.GetTexture("texture_entity_2"), world));
        }

        internal void RegisterEntityDescriptor(DEntityDescriptor descriptor)
        {
            this.registeredDescriptors.Add(descriptor.Identifier, descriptor);
        }

        internal DEntityDescriptor GetEntityDescriptorByIdentifier(string entityIdentifier)
        {
            return this.registeredDescriptors[entityIdentifier];
        }
    }
}