using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Bank;
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
        private Wallet _wallet;
        private DailyRewardsService _dailyRewards;
        private AudioManager _audioManager;
        private WheelOfLuckService wheelOfLuckService;
        private void OnDisable()
        {
            GlobalMediator.instance.OnSelectedLevel -= SelectedLevel;
        }

        public void Initialize(AudioManager audioManager, LoaderScenes loaderScenes,Wallet wallet,DailyRewardsService dailyRewards)
        {
            YandexGame.Instance.GameReady();
            YandexGame.Instance.FullAdShow();
           _audioManager = audioManager;
            _loaderScenes = loaderScenes;
            _wallet = wallet;
            _dailyRewards = dailyRewards;
            wheelOfLuckService = new(wallet);
         
            GlobalMediator.instance.OnSelectedLevel += SelectedLevel;

            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Bubbles_P"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BegraundLogo"));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIMenu>("UIMenu", null, op =>
            { 
                 op.Initialize(_wallet, _dailyRewards,wheelOfLuckService); 

            }));
            wheelOfLuckService.LoadData();
            wheelOfLuckService.TryState();
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Prefab_PortraitCamera"));

            _audioManager.Play();

        }

        private void SelectedLevel(int level)
        {
            _loaderScenes.LoadLevel(Scenes.game);
        }
    }
}