using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class RewardPopupUI : MonoBehaviour
    {
        [SerializeField] private Button onButton;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameType;
        [SerializeField] private TextMeshProUGUI amount;

        //private Action close;

        private void OnEnable()
        {
            //onButton.onClick.AddListener(OnReward);
        }

        private void OnDisable()
        {
            //onButton.onClick.RemoveAllListeners();
        }

        public void Init(Reward reward, Action close = null )
        {
            nameType.text = reward.type.ToString();
            amount.text = reward.amount.ToString();
            icon.sprite = reward.sprite;
            onButton.onClick.AddListener(()=> close.Invoke());
        }
        
        //private void OnReward() => close.Invoke();
      
    }
}