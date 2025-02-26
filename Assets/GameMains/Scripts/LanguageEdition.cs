using Assets.YG.Scripts;

using TMPro;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class LanguageEdition : MonoBehaviour
    {
        private TextMeshProUGUI content;

        public void OnEnable()
        {
            if (YandexGame.Instance.Language == "ru")
            {
                if(content == null) content = gameObject.GetComponent<TextMeshProUGUI>();
                content.text = Languages.GetContent(content.text);
            }
        }
    }
}