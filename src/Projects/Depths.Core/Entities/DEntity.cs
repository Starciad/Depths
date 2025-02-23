using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Entities
{
    internal abstract class DEntity
    {
        internal Texture2D Texture { get; set; }
        internal Point Position { get; set; }
    }
}
