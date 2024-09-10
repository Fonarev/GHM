using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.Loaders;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
   
    public class UILocationLevels : MonoBehaviour
    {
        public int location;
        private UISelectLevels selectLevels;
        [SerializeField] private Button selectLocation;
        [SerializeField] private RectTransform rootSpawn;
        private LocalAssetLoader loader;
        private void OnDisable()
        {
            if (loader != null) { loader.UnLoadAll(); }
        }
        public void Init()
        {
            selectLocation.onClick.AddListener(OnSelectedLocal);
        }

        private async void OnSelectedLocal()
        {
            if (selectLevels == null)
            {
                loader = new(true);
                selectLevels = await loader.Load<UISelectLevels>("SelectLevels", rootSpawn);
                selectLevels.Init(location);
            }
            else
            {
                selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
            }

            AudioManager.instance.PlayEffect(EffectClip.click);
        }
    }
}