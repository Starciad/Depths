using Depths.Core.Databases;
using Depths.Core.Enums.General;
using Depths.Core.IO;
using Depths.Core.World.Ores;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Depths.Core.Loaders
{
    internal static class DOreLoader
    {
        internal static DOre[] Initialize(DAssetDatabase assetDatabase)
        {
            IEnumerable<XElement> oreElements = XDocument.Parse(File.ReadAllText(Path.Combine(DDirectory.Resources, "Ores.xml"))).Root.Elements("ore");

            DOre[] ores = new DOre[oreElements.Count()];

            uint index = 0;
            foreach (XElement oreElement in oreElements)
            {
                ores[index] = ParseOre(oreElement, assetDatabase);
                index++;
            }

            return ores;
        }

        private static DOre ParseOre(XElement oreElement, DAssetDatabase assetDatabase)
        {
            XElement spawnElement = oreElement.Element("spawn");
            XElement propertiesElement = oreElement.Element("properties");

            XElement layerRangeElement = spawnElement.Element("layerRange");

            return new()
            {
                IconTexture = assetDatabase.GetTexture(oreElement.Element("icon").Value),
                LayerRange = new(Convert.ToInt32(layerRangeElement.Attribute("minimum").Value), Convert.ToInt32(layerRangeElement.Attribute("maximum").Value)),
                Name = oreElement.Element("name").Value,
                Rarity = (DRarity)Enum.Parse(typeof(DRarity), spawnElement.Element("rarity").Value.Trim(), true),
                Resistance = Convert.ToByte(propertiesElement.Element("resistance").Value),
                Value = Convert.ToByte(propertiesElement.Element("value").Value)
            };
        }
    }
}
