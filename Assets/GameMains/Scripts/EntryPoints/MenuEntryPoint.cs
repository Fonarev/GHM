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
        public void Initialize(AudioManager audio,LoaderScenes loaderScenes)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
            CoroutineHandler.StartRoutine(LoaderAsset.Load<Sprite>("Logo",op=> { logo.sprite = op; Addressables.Release(op); }));
            audio.Play();
            this.loaderScenes = loaderScenes;
            GlobalMediator.instance.OnSelectedLevel += SelectedLevel;
          
        }
        private IEnumerator Load()
        {
            var handle = Addressables.LoadAssetAsync<Sprite>("Logo");
            yield return handle;
            logo.sprite = handle.Result;
            Addressables.Release(handle);

        }
        private void SelectedLevel(int level)
        {
            loaderScenes.LoadLevel(Scenes.game);
        }
    }
}