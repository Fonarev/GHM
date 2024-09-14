using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private LoaderScenes loaderScenes;

        public void Initialize(AudioManager audio,LoaderScenes loaderScenes)
        {
            audio.Play();
            this.loaderScenes = loaderScenes;
            GlobalMediator.instance.OnSelectedLevel += SelectedLevel;
        }

        private void SelectedLevel(int level)
        {
            loaderScenes.LoadLevel(Scenes.game);
        }
    }
}