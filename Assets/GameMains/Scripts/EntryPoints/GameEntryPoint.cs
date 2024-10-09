using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform rootBackground;
        private void OnDisable()
        {
            GlobalMediator.instance.OnExitMenu -= () => { };
            GlobalMediator.instance.OnSelectedLevel -= (lvl) => { };
        }
        public void Initialize(LoaderScenes loaderScenes)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG", rootBackground));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("VFX_Bubbles", rootBackground));
            GlobalMediator.instance.OnExitMenu += () => { loaderScenes.LoadLevel(Scenes.menu); };
            GlobalMediator.instance.OnSelectedLevel += (lvl) => { loaderScenes.LoadLevel(Scenes.game); };
        }
       
    }
}