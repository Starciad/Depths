using Depths.Core.Constants;
using Depths.Core.Enums.World.Chunks;
using Depths.Core.IO;
using Depths.Core.World.Chunks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Depths.Core.Utilities
{
    internal static class DChunkLoaderUtility
    {
        public static DWorldChunk[] Load()
        {
            string xmlContent = File.ReadAllText(Path.Combine(DDirectory.Resources, "generator", "chunks.xml"));
            XDocument xDocument = XDocument.Parse(xmlContent);

            IEnumerable<XElement> elements = xDocument.Root.Elements("chunk");
            DWorldChunk[] chunks = new DWorldChunk[elements.Count()];

            uint index = 0;
            foreach (XElement chunkElement in elements)
            {
                chunks[index] = ParseChunk(chunkElement);
                index++;
            }

            return chunks;
        }

        private static DWorldChunk ParseChunk(XElement chunkElement)
        {
            return new(ParseChunkType(chunkElement), ParseContentMatrix(chunkElement));
        }

        private static DWorldChunkType ParseChunkType(XElement chunkElement)
        {
            XElement headerElement = chunkElement.Element("header");
            XElement typeElement = headerElement.Element("type");

            return (DWorldChunkType)Enum.Parse(typeof(DWorldChunkType), typeElement.Value.Trim(), true);
        }

        private static string[,] ParseContentMatrix(XElement chunkElement)
        {
            XElement contentElement = chunkElement.Element("contents");
            List<string[]> contents = ParseContents(contentElement);

            string[,] matrix = new string[DWorldConstants.TILES_PER_CHUNK_WIDTH, DWorldConstants.TILES_PER_CHUNK_HEIGHT];

            for (byte y = 0; y < DWorldConstants.TILES_PER_CHUNK_HEIGHT; y++)
            {
                for (byte x = 0; x < DWorldConstants.TILES_PER_CHUNK_WIDTH; x++)
                {
                    matrix[x, y] = contents[y][x];
                }
            }

            return matrix;
        }

        private static List<string[]> ParseContents(XElement contentsElement)
        {
            List<string[]> contents = [];
            IEnumerable<XElement> contentElements = contentsElement.Elements("content");

            foreach (XElement contentElement in contentElements)
            {
                contents.Add(contentElement.Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            }

            return contents;
        }
    }
}
