using UnityEngine;

namespace Assets.GameMains.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSourceBackground : MonoBehaviour
    {
        [SerializeField] private AudioClip musicClip;
        private AudioSource audioBG => GetComponent<AudioSource>();   

        public void Initialize()
        {
            audioBG.loop = true;
            audioBG.clip = musicClip;
        }
        public void Play() => audioBG.Play();
        public void Pause() => audioBG.Pause();
    }
}