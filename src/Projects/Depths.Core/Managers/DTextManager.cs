using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.Mathematics.Primitives;
using Depths.Core.TextRendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.Managers
{
    internal sealed class DTextManager
    {
        private Texture2D lightFontTexture;
        private Texture2D darkFontTexture;
        private Texture2D darkOutlineFontTexture;
        private Texture2D lightOutlineFontTexture;
        private Dictionary<char, Rectangle> characterMap;

        private readonly DAssetDatabase assetDatabase;

        internal DTextManager(DAssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;
        }

        internal void Initialize()
        {
            this.lightFontTexture = this.assetDatabase.GetTexture("texture_font_1");
            this.darkFontTexture = this.assetDatabase.GetTexture("texture_font_2");
            this.darkOutlineFontTexture = this.assetDatabase.GetTexture("texture_font_3");
            this.lightOutlineFontTexture = this.assetDatabase.GetTexture("texture_font_4");
            this.characterMap = GenerateCharacterMap();
        }

        internal void DrawText(SpriteBatch spriteBatch, string value, DPoint position, DTextRenderOptions options)
        {
            List<string> lines = [];

            // Determines the maximum width for line breaks.
            int maxWidth = options.MaxDimensions?.X ?? DScreenConstants.GAME_WIDTH;

            if (options.WrapText)
            {
                int charWidthWithSpacing = DFontConstants.WIDTH + options.CharacterSpacing;
                int charsPerLine = maxWidth / charWidthWithSpacing;

                if (charsPerLine < 1)
                {
                    charsPerLine = 1;
                }

                // Divide the text into lines according to the number of characters.
                for (int i = 0; i < value.Length; i += charsPerLine)
                {
                    int length = (i + charsPerLine > value.Length) ? value.Length - i : charsPerLine;
                    lines.Add(value.Substring(i, length));
                }
            }
            else
            {
                // No wrapping, text is rendered on a single line.
                lines.Add(value);
            }

            // Calculates the total height of the text block (including line spacing).
            int totalHeight = (lines.Count * DFontConstants.HEIGHT) + ((lines.Count - 1) * options.LineSpacing);

            // Adjust the start Y position based on the vertical alignment.
            int startY = position.Y;

            switch (options.VerticalAlignment)
            {
                case DVerticalTextAlignment.Center:
                    startY = position.Y - (totalHeight / 2);
                    break;

                case DVerticalTextAlignment.Bottom:
                    startY = position.Y - totalHeight;
                    break;

                case DVerticalTextAlignment.Top:
                default:
                    break;
            }

            // Render each line.
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                string line = lines[lineIndex];

                // Calculate the width of the current line.
                int lineWidth = (line.Length * (DFontConstants.WIDTH + options.CharacterSpacing)) - options.CharacterSpacing;

                // Adjust the start X position based on the horizontal alignment.
                int startX = position.X;

                switch (options.HorizontalAlignment)
                {
                    case DTextAlignment.Center:
                        startX = position.X - (lineWidth / 2);
                        break;

                    case DTextAlignment.Right:
                        startX = position.X - lineWidth;
                        break;

                    case DTextAlignment.Left:
                    default:
                        break;
                }

                // Draw each character on the line.
                int currentX = startX;
                int currentY = startY + (lineIndex * (DFontConstants.HEIGHT + options.LineSpacing));

                foreach (char c in line)
                {
                    DrawCharacter(spriteBatch, c, new DPoint(currentX, currentY), options.FontType);
                    currentX += DFontConstants.WIDTH + options.CharacterSpacing;
                }
            }
        }

        internal void DrawCharacter(SpriteBatch spriteBatch, char character, DPoint position, DFontType fontType)
        {
            if (!this.characterMap.TryGetValue(character, out Rectangle sourceRectangle))
            {
                return; // If the character is not mapped, it is not drawn.
            }

            spriteBatch.Draw(GetFontTypeTexture(fontType), new Vector2(position.X, position.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private static Dictionary<char, Rectangle> GenerateCharacterMap()
        {
            Dictionary<char, Rectangle> map = [];

            for (byte i = 0; i < DFontConstants.MAPPED_CHARACTERS.Length; i++)
            {
                map[DFontConstants.MAPPED_CHARACTERS[i]] =
                    new Rectangle(DFontConstants.WIDTH * i, 0, DFontConstants.WIDTH, DFontConstants.HEIGHT);
            }

            return map;
        }

        private Texture2D GetFontTypeTexture(DFontType type)
        {
            return type switch
            {
                DFontType.Light => this.lightFontTexture,
                DFontType.Dark => this.darkFontTexture,
                DFontType.DarkOutline => this.darkOutlineFontTexture,
                DFontType.LightOutline => this.lightOutlineFontTexture,
                _ => null,
            };
        }
    }
}
