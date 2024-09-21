using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.GameMains.Scripts.Expansion
{
    public class CoroutineHandler : MonoBehaviour
    {
        #region Single
        private static CoroutineHandler _instance;
        private static CoroutineHandler instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[COROUTINE_SINGLE]");
                    _instance = go.AddComponent<CoroutineHandler>();
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