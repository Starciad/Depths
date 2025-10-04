using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.Mathematics.Primitives;

namespace Depths.Core.TextRendering
{
    internal sealed class TextRenderOptions
    {
        internal FontType FontType { get; set; }
        internal TextAlignment HorizontalAlignment { get; set; }
        internal VerticalTextAlignment VerticalAlignment { get; set; }
        internal sbyte CharacterSpacing { get; set; }
        internal sbyte LineSpacing { get; set; }
        internal bool WrapText { get; set; }
        internal DPoint? MaxDimensions { get; set; }

        internal TextRenderOptions()
        {
            this.FontType = FontType.Light;
            this.HorizontalAlignment = TextAlignment.Left;
            this.VerticalAlignment = VerticalTextAlignment.Top;
            this.CharacterSpacing = 0;
            this.LineSpacing = 0;
            this.WrapText = false;
            this.MaxDimensions = null;
        }
    }
}
