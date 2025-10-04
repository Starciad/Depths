using Depths.Core.Entities;
using Depths.Core.Entities.Common;
using Depths.Core.Managers;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class EntityDatabase
    {
        private readonly Dictionary<string, EntityDescriptor> registeredDescriptors = [];

        private readonly AssetDatabase assetDatabase;

        internal EntityDatabase(AssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;
        }

        internal void Initialize(World.World world, EntityManager entityManager, GameInformation gameInformation, GUIManager guiManager, InputManager inputManager, MusicManager musicManager)
        {
            RegisterEntityDescriptor(new PlayerEntityDescriptor("Player", this.assetDatabase.GetTexture("texture_entity_1"), world, entityManager, gameInformation, guiManager, inputManager, musicManager));
            RegisterEntityDescriptor(new TruckEntityDescriptor("Truck Store", this.assetDatabase.GetTexture("texture_entity_2"), world));
            RegisterEntityDescriptor(new RobotEntityDescriptor("Robot", this.assetDatabase.GetTexture("texture_entity_3"), world, entityManager, gameInformation));
            RegisterEntityDescriptor(new IdolHeadEntityDescriptor("Idol Head", this.assetDatabase.GetTexture("texture_entity_4"), world, entityManager, gameInformation));
            RegisterEntityDescriptor(new StarEntityDescriptor("Star", this.assetDatabase.GetTexture("texture_entity_5"), world));
            RegisterEntityDescriptor(new DustEntityDescriptor("Dust", this.assetDatabase.GetTexture("texture_entity_6"), world, entityManager));
        }

        private void RegisterEntityDescriptor(EntityDescriptor descriptor)
        {
            this.registeredDescriptors.Add(descriptor.Identifier, descriptor);
        }

        internal EntityDescriptor GetEntityDescriptorByIdentifier(string entityIdentifier)
        {
            return this.registeredDescriptors[entityIdentifier];
        }
    }
}