using Assets.YG.Scripts;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool singleton;

        public static AudioManager instance;
        [SerializeField] private MusicSourceBackground sourceBackground;
        [SerializeField] private MusicSourceEffect sourceEffect;

        public bool isBGMusic
        {
            get => YandexGame.Instance.progressData.music;

            set
            {
                YandexGame.Instance.progressData.music = value;
                Play();
            }
        }

        public bool isEffectAudio
        {
            get => YandexGame.Instance.progressData.effectAudio;

            set
            {
                YandexGame.Instance.progressData.effectAudio = value;
            }
        }
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

        public void Play()
        {
            if (isBGMusic)
                sourceBackground.Play();
            else
                sourceBackground.Pause();
        }

        public void PlayEffect(string name) 
        {
            if (isEffectAudio)
                sourceEffect.Play(name);
        }

        private void Pause(bool isSilence)
        {
            if (!isSilence)
            {
                if (YandexGame.Instance.nowAdsShow)
                {
                    //this.isSilence = isSilence;
                    //audioSourse.Play();
                  
                }

            }
            else
            {
                //this.isSilence = isSilence;
                //audioSourse.Pause();
                AudioListener.pause = isSilence;
            }

        }

        public void PlayEffect(AudioClip triggerSound)
        {
            if (isEffectAudio)
                sourceEffect.Play(triggerSound);
        }
    }
}