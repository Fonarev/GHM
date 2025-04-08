using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;
using Assets.WheelOfLuck.Scripts;
using Assets.YG.Scripts;

using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private LoaderScenes _loaderScenes;
        private DailyRewardsService _dailyRewards;
        private AudioManager _audioManager;
        private WheelOfLuckService wheelOfLuckService;
        private GlobalMediator mediator;

        private void OnDisable()
        {
            GlobalMediator.OnSelectedLevel -= SelectedLevel;
        }

        public void Initialize(GlobalMediator mediator, AudioManager audioManager, LoaderScenes loaderScenes,DailyRewardsService dailyRewards)
        {
            this.mediator = mediator;
            YandexGame.Instance.GameReady();
            YandexGame.Instance.FullAdShow();
           _audioManager = audioManager;
            _loaderScenes = loaderScenes;
            _dailyRewards = dailyRewards;
            //wheelOfLuckService = new(wallet);

            GlobalMediator.OnSelectedLevel += SelectedLevel;
            YandexGame.Instance.OnRewardedVideo += Reward;
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            YandexGame.Instance.GetLeaderboard("Score", 5, 1, 2, "small");
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Bubbles_P"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BegraundLogo"));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIMenu>("UIMenu", null, op =>
            { 
                 op.Initialize(mediator, _dailyRewards,wheelOfLuckService); 

            }));
            //wheelOfLuckService.LoadData();
            //wheelOfLuckService.TryState();
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Prefab_PortraitCamera"));
            Curtain.Instance.Hide();
            _audioManager.PlayBackgroundMusic("harp");

        }
        private void Reward(int id)
        {
            if (id == 1) mediator.Add(100);
        }

        private void SelectedLevel(int level)
        {
            Curtain.Instance.Show();
            _loaderScenes.LoadLevel(Scenes.game);
        }
    }
}