using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts;
using Assets.YG.Scripts;

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
            StartCoroutine(Load());
        }
        private IEnumerator Load()
       {
            yield return CoroutineHandler.StartRoutine(LevelDatabase.Load());
            YandexGame.Instance.Load();
            _loaderScenes.LoadLevel(Scenes.menu);
            yield return null;
       }
    }
}