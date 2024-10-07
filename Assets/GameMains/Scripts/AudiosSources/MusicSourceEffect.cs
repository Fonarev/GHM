using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSourceEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip effectClip;
        [SerializeField] private AudioClip[] effectClips;
        private Dictionary<string, AudioClip> effects;
        private AudioSource audioSourse => GetComponent<AudioSource>();

        public void Initialize()
        {
            audioSourse.loop = false;
            audioSourse.clip = effectClip;
            effects = effectClips.ToDictionary(i => i.name, i => i);
        }

        public void Play(string name) => audioSourse.PlayOneShot(effects[name]);
        public void Play(AudioClip name) => audioSourse.PlayOneShot(name);
        public void Pause() => audioSourse.Pause();

    }
}