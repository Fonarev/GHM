using Assets.GameMains.Scripts.AudiosSources;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class BackgroundMusic : AudioBase
    {
        public override void Play(string name, bool checkClip = true, float volume = 1, float pitch = 1)
        {
            base.Play(name, checkClip, volume, pitch);

            if (checkClip)
            {
                var currentClip = AudioSource.clip;
                var newClip = Get(name);
                if (currentClip != newClip)
                {
                    AudioSource.clip = newClip;
                    AudioSource.Play();
                }
            }
            else
            {
                AudioSource.clip = Get(name);
                AudioSource.Play();
            }
        }

        public void Play()
        {
            if (AudioSource.clip != null)
                AudioSource.Play();
        }
    }
}