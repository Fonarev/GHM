using Assets.AssetLoaders;
using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class MessagesPopup : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI messages;
        [SerializeField] private Image message;
        [SerializeField] private Sprite sprite_Ru;
        public void Init()
        {
            //messages.text = Languages.GetContent(message);
            message.sprite = sprite_Ru;

        }

        public void ActionDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    
    }
}