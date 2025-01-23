using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class DailyRewardsPreview : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI state;
        [SerializeField] private Button claimButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private RewardPreview rewardPrefab;
        [SerializeField] private PopupWinRewardPreview popupPrefab;
        [SerializeField] private List<RewardPreview> rewards;
        [SerializeField] private Transform gridRewards;

        private DailyRewardsService service;

        private void OnEnable()
        {
            //if (service != null)
            //    ViewState();
        }

        private void OnDisable()
        {
            service.OnTimeSpan -= Service_OnTimeSpan;
            service.OnClaimReward -= ViewState;
        }


        public void Init(DailyRewardsService service, RewardsConfig data)
        {
            this.service = service;
            service.OnTimeSpan += Service_OnTimeSpan;
            service.OnClaimReward += ViewState;

            for (int i = 0; i < data.rewards.Count; i++)
            {
                RewardPreview prefab = Instantiate(rewardPrefab, gridRewards);

                prefab.Init(data.rewards[i], i + 1, service.currentTarget == i);

                rewards.Add(prefab);
            }

            claimButton.interactable = service.ClaimReward;
            closeButton.onClick.AddListener(Close);
            claimButton.onClick.AddListener(OnClick);
        }

        private void Service_OnTimeSpan(TimeSpan time)
        {
            state.text = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        private void ViewState(bool claimState)
        {
            if (claimState)
                state.text = $"Claim Reward";
          
            claimButton.interactable = claimState;
        }

        private void OnClick()
        {
            var reward = service.GetReward();
            Debug.Log(reward.amount);

            UpdatePreview();

            popupPrefab.Open(reward, () => 
            {
                popupPrefab.gameObject.SetActive(false);
                Close();
            });

        }

        public void UpdatePreview()
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                rewards[i].UpdatePreview(service.currentTarget == i);
            }

        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}