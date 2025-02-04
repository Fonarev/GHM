using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;

using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class DailyRewardsUI : MonoBehaviour
    {
        [SerializeField] private Button claimButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI state;
        [SerializeField] private RewardEntryUI rewardPrefab;
  
        [SerializeField] private Transform gridRewards;
        [SerializeField] private List<RewardEntryUI> rewards;

        private DailyRewardsService service;

        private void OnEnable()
        {
            if (service != null)
            {
                service.UpdateTime();
                UpdatePreview();
                service.OnTimeSpan += Service_OnTimeSpan;
                service.OnClaimReward += ViewState;
            }
            closeButton.onClick.AddListener(Close);
            claimButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveAllListeners();
            claimButton.onClick.RemoveAllListeners();
            service.OnTimeSpan -= Service_OnTimeSpan;
            service.OnClaimReward -= ViewState;
        }


        public void Init(DailyRewardsService service)
        {
            this.service = service;
            service.OnTimeSpan += Service_OnTimeSpan;
            service.OnClaimReward += ViewState;

            for (int i = 0; i < service.data.rewards.Count; i++)
            {
                RewardEntryUI prefab = Instantiate(rewardPrefab, gridRewards);

                prefab.Init(service.data.rewards[i], i + 1, service.CurrentTarget == i);

                rewards.Add(prefab);
            }
            service.UpdateTime();
            UpdatePreview();
        }

        private void Service_OnTimeSpan(TimeSpan time)
        {
            state.text = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        private void ViewState(OpenButtonType type, bool claimState)
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

        }

        public void UpdatePreview()
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                rewards[i].UpdatePreview(service.CurrentTarget == i);
            }
            ViewState(OpenButtonType.DailyRewards, service.ClaimReward);
        }

        public void Close()
        {
            AudioManager.instance.PlayEffect(EffectClip.click);
            gameObject.SetActive(false);
        }
    }
}