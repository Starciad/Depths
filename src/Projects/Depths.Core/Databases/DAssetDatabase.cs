using Depths.Core.Constants;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.IO;

namespace Depths.Core.Databases
{
    internal sealed class DAssetDatabase
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
            AssetLoader(contentManager, DAssetType.Texture, DAssetConstants.TEXTURE_ENTITY_LENGTH, "texture_entity_", Path.Combine(DDirectoryConstants.TEXTURE_ASSETS, DDirectoryConstants.TEXTURE_ENTITY_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, DAssetConstants.TEXTURE_FONT_LENGTH, "texture_font_", Path.Combine(DDirectoryConstants.TEXTURE_ASSETS, DDirectoryConstants.TEXTURE_FONT_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, DAssetConstants.TEXTURE_TILE_LENGTH, "texture_tile_", Path.Combine(DDirectoryConstants.TEXTURE_ASSETS, DDirectoryConstants.TEXTURE_TILE_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, DAssetConstants.TEXTURE_ORE_LENGTH, "texture_ore_", Path.Combine(DDirectoryConstants.TEXTURE_ASSETS, DDirectoryConstants.TEXTURE_ORE_ASSETS));
            AssetLoader(contentManager, DAssetType.Texture, DAssetConstants.TEXTURE_GUI_LENGTH, "texture_gui_", Path.Combine(DDirectoryConstants.TEXTURE_ASSETS, DDirectoryConstants.TEXTURE_GUI_ASSETS));
        }

        private void LoadSoundEffects(ContentManager contentManager)
        {
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.BLIP_SOUND_LENGTH, "sound_blip_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.BLIP_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.GOOD_SOUND_LENGTH, "sound_good_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.GOOD_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.HIT_SOUND_LENGTH, "sound_hit_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.HIT_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.JINGLE_SOUND_LENGTH, "sound_jingle_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.JINGLE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.NEGATIVE_SOUND_LENGTH, "sound_negative_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.NEGATIVE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.NOTE_SOUND_LENGTH, "sound_note_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.NOTE_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.ODD_SOUND_LENGTH, "sound_odd_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.ODD_SOUND_ASSETS));
            AssetLoader(contentManager, DAssetType.SoundEffect, DAssetConstants.RING_SOUND_LENGTH, "sound_ring_", Path.Combine(DDirectoryConstants.SOUND_ASSETS, DDirectoryConstants.RING_SOUND_ASSETS));
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