using Assets.YG.Scripts;

using System;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class PauseController : MonoBehaviour
    {
        public event Action<bool> OnApplicationFocused;

        [SerializeField] private bool singleton;
        [SerializeField] private bool isMessage;

        public static PauseController Instance => instance;
        private static PauseController instance;
        private GlobalMediator mediator;

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

        public void Initialize(GlobalMediator mediator)
        {
            this.mediator = mediator;
        }

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

        private void Pause(bool isPause)
        {
            mediator?.ApplicationFocus(isPause);
            OnApplicationFocused?.Invoke(isPause);
        }

        private void Message(string message)
        {
            if (isMessage) Debug.Log(message);
        }
    }
}