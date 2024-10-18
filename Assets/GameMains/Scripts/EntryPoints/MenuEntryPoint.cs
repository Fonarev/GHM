using Assets.AssetLoaders;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private LoaderScenes loaderScenes;
        [SerializeField] private SpriteRenderer logo;

        private void OnDisable()
        {
            GlobalMediator.instance.OnSelectedLevel -= SelectedLevel;
        }

        public void Initialize(AudioManager audio,LoaderScenes loaderScenes)
        {
            this.loaderScenes = loaderScenes;

            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("VFX_Bubbles"));
            GlobalMediator.instance.OnSelectedLevel += SelectedLevel;
            audio.Play();
        }
       
        private void SelectedLevel(int level)
        {
            loaderScenes.LoadLevel(Scenes.game);
        }
    }
}