using System;
using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class GlobalMediator : MonoBehaviour
    {
        public event Action<int> OnSelectedLevel;
        public int SelectLevel { get => selectLevel; private set { selectLevel = value; OnSelectedLevel.Invoke(selectLevel); } }
        public static GlobalMediator instance;
        private int selectLevel;

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
        public void SelectedLevel(int level) => SelectLevel = level;
    }
}