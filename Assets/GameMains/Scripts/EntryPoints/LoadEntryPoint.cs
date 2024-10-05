using Assets.GameMains.Scripts.Bank;
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
        private Wallet _wallet;

        public void Initialize(LoaderScenes loaderScenes,Wallet wallet)
        {
            _loaderScenes = loaderScenes;
            _wallet = wallet;

            StartCoroutine(Load());
        }
        private IEnumerator Load()
       {
            yield return CoroutineHandler.StartRoutine(LevelDatabase.Load());
            YandexGame.Instance.Load();
            yield return YandexGame.Instance.isLoading = true;

            _wallet.Initialize(YandexGame.Instance.progressData.coins);
            _loaderScenes.LoadLevel(Scenes.menu);
            yield return null;
       }
    }
}