using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
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
        private AudioManager _audioManager;
        private DailyRewardsService _dailyRewards;

        public void Initialize(LoaderScenes loaderScenes,AudioManager audioManager,DailyRewardsService dailyRewards)
        {
            _loaderScenes = loaderScenes;
            _audioManager = audioManager;
            _dailyRewards = dailyRewards;

            StartCoroutine(Load());
        }
        private IEnumerator Load()
        {
            StartCoroutine(LevelDatabase.Load());
            StartCoroutine(_audioManager.LoadDatas());
            StartCoroutine(YandexGame.Instance.LoadDatas());
            _dailyRewards.LoadDate();

            while (!YandexGame.Instance.isLoading)
                yield return new WaitForSeconds(0.1f);
            _loaderScenes.LoadLevel(Scenes.menu);
        }
    }
}