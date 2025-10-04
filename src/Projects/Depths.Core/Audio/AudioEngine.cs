using Depths.Core.Collections;
using Depths.Core.Databases;
using Depths.Core.Interfaces.Collections;

using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace Depths.Core.Audio
{
    internal static class AudioEngine
    {
        private sealed class PoolableSoundEffect : IPoolableObject
        {
            internal SoundEffect SoundEffect => this.soundEffect;
            internal SoundEffectInstance Instance => this.instance;

            private readonly SoundEffect soundEffect;
            private readonly SoundEffectInstance instance;

            internal PoolableSoundEffect(SoundEffect soundEffect, SoundEffectInstance instance)
            {
                this.soundEffect = soundEffect;
                this.instance = instance;

                Reset();
            }

            public void Reset()
            {
                this.instance.Stop();
                this.instance.Volume = 1f;
                this.instance.Pitch = 0.0f;
                this.instance.Pan = 0.0f;
            }
        }

        private static PoolableSoundEffect activeInstance = null;

        private static readonly Dictionary<SoundEffect, ObjectPool> soundEffectPools = [];
        private static AssetDatabase assetDatabase;

        internal static void Initialize(AssetDatabase assetDatabase)
        {
            AudioEngine.assetDatabase = assetDatabase;
        }

        internal static void Play(string identifier)
        {
            ReleaseInstance();

            PoolableSoundEffect poolableSoundEffect = GetOrCreateInstance(assetDatabase.GetSoundEffect(identifier));

            if (poolableSoundEffect == null)
            {
                return;
            }

            poolableSoundEffect.Instance.Play();

            activeInstance = poolableSoundEffect;
        }

        private static void ReleaseInstance()
        {
            if (activeInstance == null)
            {
                return;
            }

            soundEffectPools[activeInstance.SoundEffect].Add(activeInstance);

            activeInstance.Instance.Stop();
            activeInstance = null;
        }

        private static PoolableSoundEffect GetOrCreateInstance(SoundEffect soundEffect)
        {
            if (!soundEffectPools.TryGetValue(soundEffect, out ObjectPool pool))
            {
                pool = new();
                soundEffectPools[soundEffect] = pool;
            }

            PoolableSoundEffect result = pool.TryGet(out IPoolableObject poolableObject)
                ? (PoolableSoundEffect)poolableObject
                : new(soundEffect, soundEffect.CreateInstance());

            return result;
        }
    }
}
