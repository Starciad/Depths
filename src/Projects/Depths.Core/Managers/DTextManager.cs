using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Depths.Core.Managers
{
    internal sealed class DTextManager
    {
        private Texture2D lightFontTexture;
        private Texture2D darkFontTexture;
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
            this.characterMap = GenerateCharacterMap();
        }

        internal void DrawText(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Point position, DFontType fontType, int spacing)
        {
            Point currentPos = position;

            for (int i = 0; i < text.Length; i++)
            {
                DrawCharacter(spriteBatch, text[i], currentPos, fontType);
                currentPos.X += DFontConstants.WIDTH + spacing;
            }
        }

        internal void DrawCharacter(SpriteBatch spriteBatch, char character, Point position, DFontType fontType)
        {
            if (!this.characterMap.TryGetValue(character, out Rectangle sourceRectangle))
            {
                return; // If the requested character is not mapped, the code returns.
            }

            spriteBatch.Draw(GetFontTypeTexture(fontType), new(position.X, position.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        private static Dictionary<char, Rectangle> GenerateCharacterMap()
        {
            Dictionary<char, Rectangle> map = [];

            for (int i = 0; i < DFontConstants.MAPPED_CHARACTERS.Length; i++)
            {
                map[DFontConstants.MAPPED_CHARACTERS[i]] = new(new(DFontConstants.WIDTH * i, 0), new(DFontConstants.WIDTH, DFontConstants.HEIGHT));
            }

            return map;
        }

        private Texture2D GetFontTypeTexture(DFontType type)
        {
            return type switch
            {
                DFontType.Light => this.lightFontTexture,
                DFontType.Dark => this.darkFontTexture,
                _ => null,
            };
        }
    }
}
