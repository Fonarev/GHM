using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class RewardEntryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountRevard;
        [SerializeField] private TextMeshProUGUI numberDaily;
        [SerializeField] private Sprite sprite;

        private Image CurrentTarget => GetComponent<Image>();

        public void Init(Reward reward, int number, bool target)
        {
            amountRevard.text = reward.amount.ToString();
            numberDaily.text = Languages.GetContent("Daily ") + number.ToString();
            sprite = reward.sprite;
            UpdatePreview(target);
        }

        public void UpdatePreview(bool target)
        {
            CurrentTarget.color = target ? Color.green : Color.white;
        }
    }
}