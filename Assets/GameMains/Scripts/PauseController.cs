using Assets.YG.Scripts;

using System;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class PauseController : MonoBehaviour
    {
        public event Action<bool> OnPause;
        public event Action<bool> OnPauseButton;

        [SerializeField] private bool singleton;
        [SerializeField] private bool isMessage;

        public static PauseController instance;

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

        public void Initialize()
        {
            YandexGame.Instance.OnNowAdsShow += Pause;
        }

        public void PauseButton(bool isPause) => OnPauseButton?.Invoke(isPause);
        public void Pause(bool isPause) => OnPause?.Invoke(isPause);

        private void OnApplicationFocus(bool hasFocus)
        {
            Pause(!hasFocus);
            Message($"Focus {!hasFocus}");
        }

        private void OnApplicationPause(bool isPaused)
        {
            Pause(isPaused);
            Message($"ApplicationPause {isPaused}");
        }
        private void Message(string message)
        {
            if (isMessage) Debug.Log(message);
        }
    }
}