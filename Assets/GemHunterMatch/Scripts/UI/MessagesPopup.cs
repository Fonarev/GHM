using Assets.YG.Scripts;

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
            messages.text = Languages.GetContent(message);
        }

        public void ActionDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    
    }
}