using System;
using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class GlobalMediator : MonoBehaviour
    {
        public event Action<int> OnSelectedLevel;
        public static GlobalMediator instance;
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
        public void SelectedLevel(int level) => OnSelectedLevel.Invoke(level);
    }
}