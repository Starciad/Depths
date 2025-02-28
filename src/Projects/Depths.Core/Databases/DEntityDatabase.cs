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

        internal void Initialize(DWorld world, DEntityManager entityManager, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DMusicManager musicManager)
        {
            RegisterEntityDescriptor(new DPlayerEntityDescriptor("Player", this.assetDatabase.GetTexture("texture_entity_1"), world, entityManager, gameInformation, guiManager, inputManager, musicManager));
            RegisterEntityDescriptor(new DTruckEntityDescriptor("Truck Store", this.assetDatabase.GetTexture("texture_entity_2"), world));
            RegisterEntityDescriptor(new DRobotEntityDescriptor("Robot", this.assetDatabase.GetTexture("texture_entity_3"), world, entityManager, gameInformation));
            RegisterEntityDescriptor(new DIdolHeadEntityDescriptor("Idol Head", this.assetDatabase.GetTexture("texture_entity_4"), world, entityManager, gameInformation));
            RegisterEntityDescriptor(new DStarEntityDescriptor("Star", this.assetDatabase.GetTexture("texture_entity_5"), world));
        }

        private void RegisterEntityDescriptor(DEntityDescriptor descriptor)
        {
            this.registeredDescriptors.Add(descriptor.Identifier, descriptor);
        }

        internal DEntityDescriptor GetEntityDescriptorByIdentifier(string entityIdentifier)
        {
            return this.registeredDescriptors[entityIdentifier];
        }
    }
}