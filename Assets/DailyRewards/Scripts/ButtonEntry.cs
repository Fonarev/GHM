using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts
{
    [RequireComponent(typeof(Button))]
    public class ButtonEntry : MonoBehaviour
    {
        [SerializeField] private GameObject trigerClaimReward;
        private DailyRewardsService rewardsService;

        private Button button => GetComponent<Button>();

        private void OnDisable()
        {
            //rewardsService.OnClaimReward -= RewardsService_OnClaimReward;
        }

        private void Init(DailyRewardsService rewardsService)
        {
            this.rewardsService = rewardsService;
            trigerClaimReward.SetActive(false);
            //rewardsService.OnClaimReward += RewardsService_OnClaimReward;
            button.onClick.AddListener(OnClick);
        }

        private void RewardsService_OnClaimReward(bool isClaim)
        {
            trigerClaimReward.SetActive(isClaim);
        }

        private void OnClick()
        {
            rewardsService.OpenWin();
        }
    }
}