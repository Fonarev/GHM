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
        private AudioSource audioBG => GetComponent<AudioSource>();

        public void Initialize()
        {
            audioBG.loop = false;
            audioBG.clip = effectClip;
            effects = effectClips.ToDictionary(i => i.name, i => i);
        }

        public void Play(string name) => audioBG.PlayOneShot(effects[name]);
        public void Pause() => audioBG.Pause();

    }
}