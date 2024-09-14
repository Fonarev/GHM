using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GemHunterMatch.Scripts
{
    public class InputService : MonoBehaviour
    {
        public InputAction ClickAction;
        public InputAction ClickPosition;

        public static InputService instance;

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
            ClickAction.Enable();
            ClickPosition.Enable();
        }
    }
}