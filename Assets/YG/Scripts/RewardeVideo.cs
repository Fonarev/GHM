using System.Runtime.InteropServices;

using UnityEngine;

namespace Assets.YG.Scripts
{
    public class RewardeVideo : YandexBase, IAd
    {
        protected override string sourceMessage => "RewardeVideo";
        private enum RewardAdResult { None, Success, Error };
        private float timeOnOpenRewardedAds;
        private RewardAdResult rewardAdResult = RewardAdResult.None;
        private int lastRewardAdID;

        public RewardeVideo(YandexGame yandex, bool isMessage = true) : base(yandex, isMessage){ }
      
        [DllImport("__Internal")] private static extern void RewardedShow(int id);

        public void Show(int id = 0)
        {
#if !UNITY_EDITOR
            if (yandex.nowAdsShow) RewardedShow(id);
#else
            Message("RewardedAdShow");
#endif
        }

        public void Open()
        {
            yandex.NowVideoAd = true;
            timeOnOpenRewardedAds = Time.realtimeSinceStartup;
        }

        public void Close(string wasShown)
        {
            yandex.NowVideoAd = false;

            if (rewardAdResult == RewardAdResult.Success)
            {
                yandex.RewardedVideo(lastRewardAdID);
            }
            else if (rewardAdResult == RewardAdResult.Error)
            {
                Error();
            }

            rewardAdResult = RewardAdResult.None;
        }

        public void Error()
        {
            yandex.NowVideoAd = false;
            Message("Error VideoAd.");
        }

        public void RewardVideo(int id)
        {
            rewardAdResult = RewardAdResult.None;

            if (Time.realtimeSinceStartup > timeOnOpenRewardedAds + 0.5f)
            {
                rewardAdResult = RewardAdResult.Success;
                lastRewardAdID = id;
            }
            else
            {
                rewardAdResult = RewardAdResult.Error;
                Error();
            }
        }

    }
}