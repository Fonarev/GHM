using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    public class AudioEffects : AudioBase
    {
        public override void Initialize()
        {
            base.Initialize();

            AudioSource.loop = false;
        }

        public override void Play(string name, bool checkClip = true, float volume = 1, float pitch = 1)
        {
            base.Play(name, checkClip, volume, pitch);

            AudioSource.PlayOneShot(Get(name));
        }

    }
}