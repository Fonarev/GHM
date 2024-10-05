using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class UIPopupEntry : MonoBehaviour
    {
        public Image image => GetComponent<Image>();
        public RectTransform rectTransform;

        public void Set(Sprite sprite, Vector3 position)
        {
            image.sprite = sprite;
            rectTransform.position = position;
        }
    }
}