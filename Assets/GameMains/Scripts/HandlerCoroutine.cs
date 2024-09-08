using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class HandlerCoroutine : MonoBehaviour
    {

        #region Single
        private static HandlerCoroutine _instance;
        private static HandlerCoroutine instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[COROUTINE_SINGLE]");
                    _instance = go.AddComponent<HandlerCoroutine>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        # endregion

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }
    }
}