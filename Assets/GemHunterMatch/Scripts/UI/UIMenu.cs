using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private UISelectLocation locationLevels;
        [SerializeField] private Image rootTitle;
        public void Initialize()
        {
            locationLevels.Init();
           StartCoroutine( Load());
        }
        private IEnumerator Load()
        {
            var handle = Addressables.LoadAssetAsync<Sprite>("NameGame");
            yield return handle;
            rootTitle.sprite = handle.Result;
            Addressables.Release(handle);
            yield return null;
        }
    }
}