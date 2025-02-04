using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;
using Assets.YG.Scripts;

using System;
using System.Collections;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    public class DailyRewardsService 
    {
        public event Action<Reward> OnReward;
        public event Action<OpenButtonType,bool> OnClaimReward;
        public event Action<TimeSpan> OnTimeSpan;

        public RewardsConfig data;
        private bool claimReward;

        public bool ClaimReward
        {
            get => claimReward;
            private set
            {
                bool oldValue = claimReward;
                claimReward = value;

                if (oldValue != claimReward)
                {
                    OnClaimReward?.Invoke(OpenButtonType.DailyRewards, ClaimReward);
                }
            }
        }

        private readonly Wallet wallet;

        public DailyRewardsService(Wallet wallet)
        {
            this.wallet = wallet;
        }

        private float TimeReset => data.timeReset;
        private float TimeCountDown => data.timeCountDown;
        private int MaxTarget => data.rewards.Count;

        public int CurrentTarget
        {
            get => YandexGame.Instance.progressData.currentTarget;
            set => YandexGame.Instance.progressData.currentTarget = value;
        }
       
        public DateTime? dateTime
        {
            get => YandexGame.Instance.progressData.dataTime;
            set => YandexGame.Instance.progressData.dataTime = value;
        }

        public void LoadDate()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.Load<RewardsConfig>("RewardsConfig", op => data = op));
        }

        public bool TryState()
        {
            ClaimReward = UpdateClaimState();
            return ClaimReward;
        }

        public void UpdateTime()
        {
            CoroutineHandler.StartRoutine(Updater());
        }

        public Reward GetReward(bool isAddRewardds = true)
        {
            Reward reward = data.rewards[CurrentTarget];

            if(reward != null)
            {
                dateTime = System.DateTime.UtcNow;
                CurrentTarget = (CurrentTarget + 1) % MaxTarget;
                UpdateTime();
                YandexGame.Instance.Save();
                OnReward?.Invoke(reward);
                ClaimReward = false;

                if (isAddRewardds)
                    wallet.Add(reward.amount);
            }

            return reward;
        }

        private bool UpdateClaimState()
        {

            if (dateTime.HasValue)
            {
                TimeSpan timeSpan = System.DateTime.UtcNow - dateTime.Value;

                if (timeSpan.TotalHours > TimeReset)
                {
                    dateTime = null;
                    CurrentTarget = 0;
                }

                if (timeSpan.TotalHours < TimeCountDown)
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerator Updater()
        {
            while (true)
            {
                ClaimReward = UpdateClaimState();

                if (!ClaimReward)
                {
                    var dateTimeMax = DateTime.Today.AddHours(TimeCountDown);
                    var time = dateTimeMax - dateTime.Value;
                    var nextClaim = dateTime.Value.AddHours(time.TotalHours);
                    var timeSpan = nextClaim - DateTime.UtcNow;
                    OnTimeSpan?.Invoke(timeSpan);
                    }
            
                yield return new WaitForSeconds(1);
            }
        }
    }
}