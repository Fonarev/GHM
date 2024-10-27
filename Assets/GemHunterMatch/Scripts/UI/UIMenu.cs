using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private UISelectLocation locationLevels;
        [SerializeField] private Image logo;

        public void Initialize()
        {
            locationLevels.Init();
            CoroutineHandler.StartRoutine(LoaderAsset.Load<Sprite>("Logo", op => { logo.sprite = op; Addressables.Release(op); }));
        }

    }
}