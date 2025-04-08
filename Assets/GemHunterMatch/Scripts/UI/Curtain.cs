using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class Curtain : MonoBehaviour
    {
        public GameObject curtain;
        public static Curtain Instance => instance;
        private static Curtain instance;
        private void Awake() { instance = this;DontDestroyOnLoad(gameObject); }
       
        public void Show()
        {
          curtain.SetActive(true);
        }
        public void Hide()
        {
            curtain.SetActive(false);
        }
    }
}