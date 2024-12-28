using Assets.DailyRewards.Scripts;
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
        private DailyRewardsService _dailyRewards;

        public void Initialize(LoaderScenes loaderScenes,Wallet wallet,DailyRewardsService dailyRewards)
        {
            _loaderScenes = loaderScenes;
            _wallet = wallet;
            _dailyRewards = dailyRewards;
            StartCoroutine(Load());
        }
        private IEnumerator Load()
       {
            yield return CoroutineHandler.StartRoutine(LevelDatabase.Load());
            YandexGame.Instance.Load();
            yield return YandexGame.Instance.isLoading = true;
            _dailyRewards.LoadDate();
            _wallet.Initialize(YandexGame.Instance.progressData.coins);
            _loaderScenes.LoadLevel(Scenes.menu);
            yield return null;
       }
    }
}