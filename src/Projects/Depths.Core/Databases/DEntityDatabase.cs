using Depths.Core.Entities;
using Depths.Core.Entities.Common;

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

        internal void Initialize()
        {
            RegisterEntityDescriptor(new DPlayerEntityDescriptor("Player", this.assetDatabase.GetTexture("texture_entity_1")));
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