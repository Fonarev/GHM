using Assets.AssetLoaders;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioBase : MonoBehaviour
    {
        [SerializeField] protected AudioClip audioClip;
        [SerializeField] protected AudioClip[] audioClips;
        protected Dictionary<string, AudioClip> audioClipsMap = new();

        public AudioSource AudioSource => GetComponent<AudioSource>();

        public virtual void Initialize()
        {
            AudioSource.loop = true;
            AudioSource.clip = audioClip;

            if (audioClips.Length > 0)
                audioClipsMap = audioClips.ToDictionary(i => i.name, i => i);
        }

        public IEnumerator LoadData(string path)
        {
            yield return StartCoroutine(LoaderAsset.LoadList<AudioClip>(path, op =>
            {
                if (op != null)
                {
                    audioClipsMap[op.name] = op;
                }

            }));

        }

        public virtual void Play(string name, bool checkClip, float volume = 1, float pitch = 1)
        {
            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
        }

        public virtual void Pause() => AudioSource.Pause();

        protected AudioClip Get(string name)
        {
            if (audioClipsMap.TryGetValue(name, out AudioClip clip))
                return clip;

            Debug.Log($"No AudioClip in container {name}");
            return null;
        }
    }
}