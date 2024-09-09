using Assets.YG.Scripts;

using UnityEngine;

namespace Assets.GameMains.Scripts.AudiosSources
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool singleton;

        public static AudioManager instance;
        [SerializeField] private MusicSourceBackground MusicSourceBackground;
        [SerializeField] private MusicSourceEffect MusicSourceEffect;

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
            pauseController.OnPause += Pause;
            YandexGame.Instance.OnNowAdsShow += Pause;
        }

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

    }
}