using Depths.Core.Constants;
using Depths.Core.Enums.World.Chunks;
using Depths.Core.IO;
using Depths.Core.World.Chunks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Depths.Core.Loaders
{
    internal static class DChunkLoader
    {
        internal static DWorldChunk[] Initialize()
        {
            IEnumerable<XElement> groupingElements = XDocument.Parse(File.ReadAllText(Path.Combine(DDirectory.Resources, "Chunks.xml"))).Root.Elements("grouping");

            List<DWorldChunk> chunks = [];

            foreach (XElement groupingElement in groupingElements)
            {
                foreach (DWorldChunk worldChunk in ParseChunks(groupingElement, ParseGroupingType(groupingElement)))
                {
                    chunks.Add(worldChunk);
                }
            }

            return [.. chunks];
        }

        private static DWorldChunkType ParseGroupingType(XElement groupingElement)
        {
            return (DWorldChunkType)Enum.Parse(typeof(DWorldChunkType), groupingElement.Element("information").Element("type").Value.Trim(), true);
        }
        
        private static IEnumerable<DWorldChunk> ParseChunks(XElement groupingElement, DWorldChunkType chunkType)
        {
            foreach (XElement chunkElement in groupingElement.Element("content").Elements("chunk"))
            {
                yield return new(chunkType, ParseContentMatrix(chunkElement));
            }
        }

        private static string[,] ParseContentMatrix(XElement chunkElement)
        {
            List<string[]> contents = ParseMapping(chunkElement.Element("mapping"));

            string[,] matrix = new string[DWorldConstants.TILES_PER_CHUNK_WIDTH, DWorldConstants.TILES_PER_CHUNK_HEIGHT];

            // Allocate elements from the string array to the chunk's 2d array.
            for (byte y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (byte x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    matrix[x, y] = contents[y][x];
                }
            }

            return matrix;
        }

        private static List<string[]> ParseMapping(XElement contentsElement)
        {
            List<string[]> contents = [];

            foreach (XElement rowElement in contentsElement.Elements("row"))
            {
                contents.Add(rowElement.Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            }

            return contents;
        }
    }
}
