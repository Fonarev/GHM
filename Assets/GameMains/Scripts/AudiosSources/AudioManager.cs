using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool singleton;
        [SerializeField] private BackgroundMusic backgroundMusic;
        [SerializeField] private AudioEffects audioEffects;

        private GlobalMediator mediator;
        private static AudioManager _instance;

        public bool IsBackground => mediator.IsEnableBackgroundMusic;

        public bool IsEffect => mediator.IsEnableAudioEffect;

        public static AudioManager instance => _instance;

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
                    _instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                _instance = null;
                DontDestroyOnLoad(gameObject);
            }
            #endregion
        }

        public void Initialize(GlobalMediator mediator)
        {
            this.mediator = mediator;

            backgroundMusic.Initialize();
            audioEffects.Initialize();

            mediator.OnApplicationFocused += Pause;
            mediator.OnNowAdsShow += Pause;
        }

        public IEnumerator LoadDatas()
        {
            StartCoroutine(backgroundMusic.LoadData("bgMusic"));
            yield return StartCoroutine(audioEffects.LoadData("audioEffect"));
        }

        public void PlayBackgroundMusic(string name, bool checkClip = true, float volume = 1, float pitch = 1)
        {
            if (IsBackground)
                backgroundMusic.Play(name, checkClip, volume, pitch);
            else
                backgroundMusic.Pause();
        }

        public void PlayEffect(string name, bool checkClip = true, float volume = 1, float pitch = 1)
        {
            if (IsEffect)
            {
                audioEffects.Play(name, checkClip, volume, pitch);
            }
        }

        private void Pause(bool isSilence)
        {
            if (!isSilence)
            {
                AudioListener.pause = mediator.StateNowAdsShow;
            }
            else
            {
                AudioListener.pause = true;
            }
        }

    }
}