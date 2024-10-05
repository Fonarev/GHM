using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UIItemEntry : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountView;

        public Button Button => GetComponent<Button>();
        private RectTransform Rect => GetComponent<RectTransform>();

        private Vector3 selectedView = new Vector3(1.3f, 1.3f, 0.0f);

        public void Init(Sprite displaySprite, int amount)
        {
            icon.sprite = displaySprite;
            amountView.text = amount.ToString();
        }

        public void SwitchView(bool isSelect)
        {
            Rect.localScale = isSelect == true ? selectedView : Vector3.one;
        }

        public void ChangedAmount(int amount)
        {
            SwitchView(false);
            amountView.text = amount.ToString();
        }
        
    }
}