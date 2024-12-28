using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class PopupWinRewardPreview : MonoBehaviour
    {
        [SerializeField] private Button onButton;
        [SerializeField] private Image sprite;
        [SerializeField] private TextMeshProUGUI nameType;
        [SerializeField] private TextMeshProUGUI amount;
        private DailyRewardsPreview dailyRewardsPreview;

        private void OnEnable()
        {
            onButton.onClick.AddListener(OnReward);
        }

        private void OnDisable()
        {
            onButton.onClick.RemoveAllListeners();
        }

        public void Open(Reward reward, DailyRewardsPreview dailyRewardsPreview)
        {
            nameType.text = reward.type.ToString();
            amount.text = reward.amount.ToString();
            sprite.sprite = reward.sprite;
            this.gameObject.SetActive(true);
            this.dailyRewardsPreview = dailyRewardsPreview;
        }

        private void OnReward()
        {
            dailyRewardsPreview.UpdatePreview();
            this.gameObject.SetActive(false);
        }
    }
}