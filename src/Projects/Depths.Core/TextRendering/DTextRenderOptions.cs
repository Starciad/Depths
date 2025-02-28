using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.Mathematics.Primitives;

namespace Depths.Core.TextRendering
{
    internal sealed class DTextRenderOptions
    {
        internal DFontType FontType { get; set; }
        internal DTextAlignment HorizontalAlignment { get; set; }
        internal DVerticalTextAlignment VerticalAlignment { get; set; }
        internal sbyte CharacterSpacing { get; set; }
        internal sbyte LineSpacing { get; set; }
        internal bool WrapText { get; set; }
        internal DPoint? MaxDimensions { get; set; }

        internal DTextRenderOptions()
        {
            this.FontType = DFontType.Light;
            this.HorizontalAlignment = DTextAlignment.Left;
            this.VerticalAlignment = DVerticalTextAlignment.Top;
            this.CharacterSpacing = 0;
            this.LineSpacing = 0;
            this.WrapText = false;
            this.MaxDimensions = null;
        }
    }
}
