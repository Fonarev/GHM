using System;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class GlobalMediator : MonoBehaviour
    {
        public event Action<int> OnSelectedLevel;
        public event Action OnExitMenu;

        public int SelectLevel
        {
            get => selectLevel;
            private set
            {
                selectLevel = value;
                OnSelectedLevel.Invoke(selectLevel);
                Debug.Log($"Level {selectLevel}");
            }
        }
        public int SelectLocation
        {
            get => selectLocation;
            private set
            {
                selectLocation = value;
                Debug.Log($"Location {selectLocation}");
            }
        }

        public static GlobalMediator instance;
        private int selectLevel;
        private int selectLocation;

        private void Awake()
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

        public void SelectedLevel(int location, int level)
        {
            SelectLevel = level;
            SelectLocation = location;
        }

        public void ExitMenu() => OnExitMenu.Invoke();
       
    }
}