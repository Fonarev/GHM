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

        public void PlayEffect(string name, float volume = 1, float pitch = 1) 
        {
            if (isEffectAudio)
            {
                sourceEffect.Play(name, volume, pitch);
            }
        }

        private void Pause(bool isSilence)
        {
            if (!isSilence)
            {
                AudioListener.pause = YandexGame.Instance.nowAdsShow ? true : false;
            }
            else
            {
                AudioListener.pause = true;
            }
        }

        public void PlayEffect(AudioClip triggerSound)
        {
            if (isEffectAudio)
                sourceEffect.Play(triggerSound);
        }
    }
}