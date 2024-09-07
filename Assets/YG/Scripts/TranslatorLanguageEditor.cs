using TMPro;

using UnityEngine;

namespace Assets.YG.Scripts
{
    public class TranslatorLanguageEditor : MonoBehaviour
    {
        public string textRu;
        private TextMeshProUGUI currentText;
        public void Start()
        {
            if(YandexGame.Instance.Language == "ru")
            {
                currentText = GetComponent<TextMeshProUGUI>();
                currentText.text = textRu;
            }
        }
    }
}