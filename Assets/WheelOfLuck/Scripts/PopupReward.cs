using System;
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.WheelOfLuck.Scripts
{
    public class PopupReward : MonoBehaviour
    {
        [SerializeField] private Button onButton;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameType;
        [SerializeField] private TextMeshProUGUI amount;

        private Action close;

        private void OnEnable()
        {
            onButton.onClick?.AddListener(OnReward);
        }

        private void OnDisable()
        {
            onButton.onClick.RemoveAllListeners();
        }

        public void Init(Reward reward, Action close = null)
        {
            nameType.text = reward.type.ToString();
            amount.text = reward.amount.ToString();
            icon.sprite = reward.sprite;
            this.close = close;
        }

        private void OnReward() => close.Invoke();
    }
}