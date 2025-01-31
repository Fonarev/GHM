using Assets.AssetLoaders;

using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.WheelOfLuck.Scripts
{
    public class WheelOfLuckService
    {
        public event Action<Reward> OnReward;
        public event Action<bool> OnClaimReward;
        public event Action<TimeSpan> OnTimeSpan;

        public bool ClaimReward
        {
            get => claimReward;
            private set => claimReward = value;
        }

        public DateTime? dateTime
        {
            get => YandexGame.Instance.progressData.dataTimeWheel;
            set => YandexGame.Instance.progressData.dataTimeWheel = value;
        }

        private bool claimReward;

        private float TimeReset = 0.1f;
        private float TimeCountDown = 0.11f;
        private Wheel wheel;
        private PopupReward popupReward;
        private WheelConfig config;
        private Transform container;
        private Wallet wallet;

        public WheelOfLuckService(Wallet wallet)
        {
            this.wallet = wallet;
        }

        public void LoadData()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.Load<WheelConfig>("WheelConfig", op => config = op));
        }
        public void TryState()
        {
            HandlerCoroutine.StartRoutine(Updater());
        }

        public void OpenWheel(Transform container)
        {
            this.container = container;

            if (wheel == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<Wheel>("WheelView", container, op =>
                {
                    wheel = op;
                    wheel.Init(this, config);
                }));
            }
            else{
                wheel.gameObject.SetActive(true);
            }
        }
        public void OpenPopupWin(int index)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<PopupReward>("PopupRewardWheel", container, op =>
            {
                popupReward = op;
                popupReward.Init(config.cells[index], () =>
                {
                    AddReward(config.cells[index]);
                    popupReward.gameObject.SetActive(false);
                    Addressables.ReleaseInstance(popupReward.gameObject);
                });
            }));
        }
      
        private IEnumerator Updater()
        {
            while (true)
            {
                ClaimReward = UpdateState();

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
        private bool UpdateState()
        {

            if (dateTime.HasValue)
            {
                TimeSpan timeSpan = DateTime.UtcNow - dateTime.Value;

                if (timeSpan.TotalHours > TimeReset)
                {
                    dateTime = null;
                }

                if (timeSpan.TotalHours < TimeCountDown)
                {
                    return false;
                }
            }

            return true;
        }
        private void AddReward(Reward reward)
        {
            wallet.Add(reward.amount);
        }
        public void SetNewDateTime()
        {
            dateTime = DateTime.UtcNow;
            YandexGame.Instance.Save();
            TryState();
            OnClaimReward?.Invoke(false);
        }

    }
}