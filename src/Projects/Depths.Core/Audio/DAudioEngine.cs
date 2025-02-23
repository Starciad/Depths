using Depths.Core.Collections;
using Depths.Core.Interfaces.Collections;

using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace Depths.Core.Audio
{
    internal static class DAudioEngine
    {
        private sealed class DPoolableSoundEffect : IDPoolableObject
        {
            internal SoundEffect SoundEffect => this.soundEffect;
            internal SoundEffectInstance Instance => this.instance;

            private readonly SoundEffect soundEffect;
            private readonly SoundEffectInstance instance;

            internal DPoolableSoundEffect(SoundEffect soundEffect, SoundEffectInstance instance)
            {
                this.soundEffect = soundEffect;
                this.instance = instance;

                Reset();
            }

            public void Reset()
            {
                this.instance.Stop();
                this.instance.Volume = 0.02f;
                this.instance.Pitch = 0.0f;
                this.instance.Pan = 0.0f;
            }
        }

        private static DPoolableSoundEffect activeInstance = null;

        private static readonly Dictionary<SoundEffect, DObjectPool> soundEffectPools = [];

        internal static void Play(SoundEffect soundEffect)
        {
            ReleaseInstance();

            DPoolableSoundEffect poolableSoundEffect = GetOrCreateInstance(soundEffect);

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

        private static DPoolableSoundEffect GetOrCreateInstance(SoundEffect soundEffect)
        {
            if (!soundEffectPools.TryGetValue(soundEffect, out DObjectPool pool))
            {
                pool = new();
                soundEffectPools[soundEffect] = pool;
            }

            DPoolableSoundEffect result = pool.TryGet(out IDPoolableObject poolableObject)
                ? (DPoolableSoundEffect)poolableObject
                : new(soundEffect, soundEffect.CreateInstance());

            return result;
        }
    }
}
