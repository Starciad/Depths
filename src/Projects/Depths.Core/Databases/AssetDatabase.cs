using Depths.Core.Constants;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.IO;

namespace Depths.Core.Databases
{
    internal sealed class AssetDatabase
    {
        private enum DAssetType : byte
        {
            Texture = 0,
            SoundEffect = 1
        }

        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SoundEffect> soundEffects = [];

        internal void Initialize(ContentManager contentManager)
        {
            LoadGraphics(contentManager);
            LoadSoundEffects(contentManager);
        }

        internal Texture2D GetTexture(string name)
        {
            return this.textures[name];
        }

        internal SoundEffect GetSoundEffect(string name)
        {
            return this.soundEffects[name];
        }

        private void LoadGraphics(ContentManager contentManager)
        {
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_ENTITY_LENGTH, "texture_entity_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_ENTITY_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_FONT_LENGTH, "texture_font_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_FONT_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_TILE_LENGTH, "texture_tile_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_TILE_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_ORE_LENGTH, "texture_ore_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_ORE_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_GUI_LENGTH, "texture_gui_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_GUI_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_BUTTON_LENGTH, "texture_button_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_BUTTON_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, AssetConstants.TEXTURE_BACKGROUND_LENGTH, "texture_background_", Path.Combine(DirectoryConstants.TEXTURE_ASSETS, DirectoryConstants.TEXTURE_BACKGROUND_ASSETS));
        }

        private void LoadSoundEffects(ContentManager contentManager)
        {
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.BLIP_SOUND_LENGTH, "sound_blip_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.BLIP_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.GOOD_SOUND_LENGTH, "sound_good_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.GOOD_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.HIT_SOUND_LENGTH, "sound_hit_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.HIT_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.JINGLE_SOUND_LENGTH, "sound_jingle_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.JINGLE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.NEGATIVE_SOUND_LENGTH, "sound_negative_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.NEGATIVE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.NOTE_SOUND_LENGTH, "sound_note_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.NOTE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.ODD_SOUND_LENGTH, "sound_odd_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.ODD_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, AssetConstants.RING_SOUND_LENGTH, "sound_ring_", Path.Combine(DirectoryConstants.SOUND_ASSETS, DirectoryConstants.RING_SOUND_ASSETS));
        }

        private void AssetLoader(ContentManager contentManager, DAssetType assetType, byte length, string prefix, string path)
        {
            uint targetId;
            string targetName;
            string targetPath;

            for (uint i = 0; i < length; i++)
            {
                targetId = i + 1;
                targetName = string.Concat(prefix, targetId);
                targetPath = Path.Combine(path, targetName);

                switch (assetType)
                {
                    case DAssetType.Texture:
                        this.textures.Add(targetName, contentManager.Load<Texture2D>(targetPath));
                        break;

                    case DAssetType.SoundEffect:
                        this.soundEffects.Add(targetName, contentManager.Load<SoundEffect>(targetPath));
                        break;

                    default:
                        return;
                }
            }
        }
    }
}