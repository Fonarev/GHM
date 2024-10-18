using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.UI;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform rootBackground;
        public UIGamePlay ui;
        private LoaderScenes loaderScenes;

        private void OnDisable()
        {
            GlobalMediator.instance.OnExitMenu -= Exit;
            GlobalMediator.instance.OnSelectedLevel -= SelecteLevel;
        }

        public void Initialize(LoaderScenes loaderScenes,GamePlay gamePlay, Wallet wallet)
        {
            this.loaderScenes = loaderScenes;
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG", rootBackground));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("VFX_Bubbles", rootBackground));
            GlobalMediator.instance.OnExitMenu += Exit;
            GlobalMediator.instance.OnSelectedLevel += SelecteLevel;
            ui.Initialize(gamePlay, wallet,LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel));
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