using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSourceEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip effectClip;
        [SerializeField] private AudioClip[] effectClips;
        private AudioSource audioBG => GetComponent<AudioSource>();

        public void Initialize()
        {
            audioBG.loop = false;
            audioBG.clip = effectClip;
        }

        public void Play() => audioBG.PlayOneShot(effectClip);
        public void Pause() => audioBG.Pause();

    }
}