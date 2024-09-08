using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class LoadEntryPoint : MonoBehaviour
    {
        private LoaderScenes _loaderScenes;

        public void Initialize(LoaderScenes loaderScenes)
        {
            _loaderScenes = loaderScenes;
            //StartCoroutine(Load());
        }
        private IEnumerator Load()
       {
            _loaderScenes.LoadLevel(1);
            yield return null;
       }
    }
}