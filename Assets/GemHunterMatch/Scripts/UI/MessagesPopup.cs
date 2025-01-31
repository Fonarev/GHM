using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class MessagesPopup : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI messages;

        public void Init(string message)
        {
            messages.text = message;
        }

        public void ActionDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    
    }
}