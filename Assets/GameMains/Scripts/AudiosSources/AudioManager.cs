using Assets.YG.Scripts;

using System;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool singleton;

        public static AudioManager instance;
        [SerializeField] private MusicSourceBackground sourceBackground;
        [SerializeField] private MusicSourceEffect sourceEffect;

        private void Awake()
        {
            #region singleton
            if (singleton)
            {
                if (instance != null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                instance = null;
                DontDestroyOnLoad(gameObject);
            }
            #endregion
        }

        public void Initialize(PauseController pauseController)
        {
            sourceBackground.Initialize();
            sourceEffect.Initialize();

            pauseController.OnPause += Pause;
            YandexGame.Instance.OnNowAdsShow += Pause;
        }

        public void Play() => sourceBackground.Play();
        public void PlayEffect(string name) => sourceEffect.Play(name);

        private void Pause(bool isSilence)
        {
            if (!isSilence)
            {
                if (YandexGame.Instance.nowAdsShow)
                {
                    //this.isSilence = isSilence;
                    //audioBG.Play();
                  
                }

            }
            else
            {
                //this.isSilence = isSilence;
                //audioBG.Pause();
                AudioListener.pause = isSilence;
            }

        }

        internal void PlayEffect(AudioClip triggerSound)
        {
            throw new NotImplementedException();
        }
    }
}