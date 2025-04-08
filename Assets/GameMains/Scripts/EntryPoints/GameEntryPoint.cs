using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.UI;
using Assets.YG.Scripts;

using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform rootBackground;
        private GlobalMediator mediator;
        private LoaderScenes loaderScenes;
        private GamePlay gamePlay;

        private void OnDisable()
        {
            mediator.OnExitMenu -= Exit;
            GlobalMediator.OnSelectedLevel -= SelecteLevel;
            YandexGame.Instance.OnRewardedVideo -= Reward;
        }

        public void Initialize(GlobalMediator mediator,LoaderScenes loaderScenes,GamePlay gamePlay)
        {
            this.mediator = mediator;
            this.loaderScenes = loaderScenes;
            this.gamePlay = gamePlay;

            mediator.OnExitMenu += Exit;
            GlobalMediator.OnSelectedLevel += SelecteLevel;
            YandexGame.Instance.OnRewardedVideo += Reward;
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG", rootBackground));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Bubbles_P", rootBackground));

            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGamePlay>("UIGamePlay", null,(System.Action<UIGamePlay>)(op=>
            { 
                op.Initialize(mediator, gamePlay, LevelDatabase.GetLevel((int)GlobalMediator.SelectLevel)); 
            })));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Prefab_PortraitCamera"));
            Curtain.Instance.Hide();
        }

        private void Exit()
        {
            Curtain.Instance.Show();
            loaderScenes.LoadLevel(Scenes.menu);
        }
        private void Reward(int id)
        {
            if (id == 0) gamePlay.AddMoves(5);
            if (id == 1) mediator.Add(100);
        }
        private void SelecteLevel(int level)
        {
            YandexGame.Instance.FullAdShow();
            Curtain.Instance.Show();
            loaderScenes.LoadLevel(Scenes.game);
        }
    }
}