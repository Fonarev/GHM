using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts.UI;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using System;
using System.Collections;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    public class DailyRewardsService 
    {
        public event Action<Reward> OnReward;
        public event Action OnClaimReward;
        public event Action<TimeSpan> OnTimeSpan;

        private RewardsConfig config;
        private DailyRewardsPreview dailyRewardsPreview;

        public bool ClaimReward;
       
        private float TimeReset => config.timeReset;
        private float TimeCountDown => config.timeCountDown;
        private int MaxTarget => config.rewards.Count;
      
        public int currentTarget
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
            CoroutineHandler.StartRoutine(LoaderAsset.Load<RewardsConfig>("RewardsConfig", op => config = op));
        }

        public bool TryState()
        {
            ClaimReward = UpdateClaimState();
            if (ClaimReward) OpenWin();
            return ClaimReward;
        }

        public void OpenWin()
        {
            if (dailyRewardsPreview != null && !dailyRewardsPreview.enabled)
            {
                dailyRewardsPreview.gameObject.SetActive(true);
                UpdateTime();
            }
            else
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<DailyRewardsPreview>("DailyRewardsPreview", null, op =>
                {
                    dailyRewardsPreview = op; 
                    dailyRewardsPreview.Init(this, config);
                    UpdateTime();
                }));
               
            }
           
        }

        private bool UpdateClaimState()
        {
            if (dateTime.HasValue)
            {
                TimeSpan timeSpan = DateTime.UtcNow - dateTime.Value;

                if (timeSpan.TotalHours > TimeReset)
                {
                    dateTime = null;
                    currentTarget = 0;
                }
                else
                {
                    if (timeSpan.TotalHours < TimeCountDown)
                    {
                        return false;
                    }
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
                    var nextClaim = dateTime.Value.AddHours(TimeCountDown);
                    var timeSpan = nextClaim - DateTime.UtcNow;
                    OnTimeSpan?.Invoke(timeSpan);
                }
                yield return null;
                if (ClaimReward) OnClaimReward?.Invoke();
                yield return new WaitForSeconds(1);
            }
        }

        public void UpdateTime()
        {
            CoroutineHandler.StartRoutine(Updater());
        }

        public Reward GetReward()
        {
            Reward reward = config.rewards[currentTarget];
 
            dateTime = DateTime.UtcNow;

            OnReward?.Invoke(reward);

            currentTarget = (currentTarget + 1) % MaxTarget;

            TryState();
            UpdateTime();
            return reward;
        }
    }
}