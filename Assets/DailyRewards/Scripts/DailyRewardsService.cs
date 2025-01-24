using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts.UI;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.DailyRewards.Scripts
{
    public class DailyRewardsService 
    {
        public event Action<Reward> OnReward;
        public event Action<bool> OnClaimReward;
        public event Action<TimeSpan> OnTimeSpan;

        private RewardsConfig config;
        private DailyRewardsPreview dailyRewardsPreview;

        public bool ClaimReward;
        private PopupWinRewardPreview popupWinRewardPreview;
        private readonly Wallet wallet;

        public DailyRewardsService(Wallet wallet)
        {
            this.wallet = wallet;
        }

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
            //if (ClaimReward) OpenWin();
            return ClaimReward;
        }

        public void OpenWin(Transform container)
        {
            if (dailyRewardsPreview != null )
            {
                dailyRewardsPreview.gameObject.SetActive(!dailyRewardsPreview.gameObject.activeSelf);
                UpdateTime();
            }
            else
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<DailyRewardsPreview>("DailyRewardsPreview", container, op =>
                {
                    dailyRewardsPreview = op; 
                    dailyRewardsPreview.Init(this, config);
                    UpdateTime();
                }));
               
            }
           
        }
        public void OpenPopupWin(Transform container)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<PopupWinRewardPreview>("RewardPopup", container, op =>
            {
                popupWinRewardPreview = op;
                popupWinRewardPreview.Init(config.rewards[currentTarget], () => {
                    GetReward(); popupWinRewardPreview.gameObject.SetActive(false);
                    OpenWin(container); Addressables.ReleaseInstance(popupWinRewardPreview.gameObject);
                });
            }));
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
                    var nextClaim = dateTime.Value.AddHours(TimeCountDown);
                    var timeSpan = nextClaim - DateTime.UtcNow;
                    OnTimeSpan?.Invoke(timeSpan);
                }
                yield return null;
                if (ClaimReward) OnClaimReward?.Invoke(true);
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

            if(reward != null)
            {
                wallet.Add(reward.amount);
                dateTime = DateTime.UtcNow;
                currentTarget = (currentTarget + 1) % MaxTarget;
                UpdateTime();
                YandexGame.Instance.Save();
                OnReward?.Invoke(reward);
                OnClaimReward?.Invoke(false);
            }

            return reward;
        }
    }
}