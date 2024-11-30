using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.UI;

using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform rootBackground;
        private LoaderScenes loaderScenes;
        private GamePlay gamePlay;
        private Wallet wallet;

        private void OnDisable()
        {
            GlobalMediator.instance.OnExitMenu -= Exit;
            GlobalMediator.instance.OnSelectedLevel -= SelecteLevel;
        }

        public void Initialize(LoaderScenes loaderScenes,GamePlay gamePlay, Wallet wallet)
        {
            this.loaderScenes = loaderScenes;
            this.gamePlay = gamePlay;
            this.wallet = wallet;

            GlobalMediator.instance.OnExitMenu += Exit;
            GlobalMediator.instance.OnSelectedLevel += SelecteLevel;

            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG", rootBackground));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Bubbles_P", rootBackground));

            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGamePlay>("UIGamePlay", null,op=>
            { 
                op.Initialize(gamePlay, wallet, LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel)); 
            }));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("Prefab_PortraitCamera"));
        }

        private void Exit()
        {
            loaderScenes.LoadLevel(Scenes.menu);
        }

        private void SelecteLevel(int level)
        {
            loaderScenes.LoadLevel(Scenes.game);
        }
    }
}