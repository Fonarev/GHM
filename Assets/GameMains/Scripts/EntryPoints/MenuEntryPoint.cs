using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;
using Assets.YG.Scripts;

using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private LoaderScenes _loaderScenes;
        private DailyRewardsService _dailyRewards;
        private AudioManager _audioManager;

        private void OnDisable()
        {
            GlobalMediator.instance.OnSelectedLevel -= SelectedLevel;
        }

        public void Initialize(AudioManager audioManager, LoaderScenes loaderScenes,DailyRewardsService dailyRewards)
        {
            YandexGame.Instance.GameReady();
            _audioManager = audioManager;
            _loaderScenes = loaderScenes;
            _dailyRewards = dailyRewards;
            GlobalMediator.instance.OnSelectedLevel += SelectedLevel;

            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Bubbles_P"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BegraundLogo"));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIMenu>("UIMenu", null, op => { op.Initialize(); }));
            _dailyRewards.TryState();
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Prefab_PortraitCamera"));

            _audioManager.Play();

        }

        private void SelectedLevel(int level)
        {
            _loaderScenes.LoadLevel(Scenes.game);
        }
    }
}