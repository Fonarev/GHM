using Assets.DailyRewards.Scripts.UI;
using Assets.YG.Scripts;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    public class DailyRewardsService : MonoBehaviour
    {
        private readonly List<Reward> rewards;
        private DailyRewardsPreview prefabWin;
        public readonly bool claimReward;
        private int claimDead = 48;
        private int claimCounDown = 24;
        private maxTarget = 7;
        private int currentTarget
        {
            get => YandexGame.Instance.progressData.currentTarget;
            set => YandexGame.Instance.progressData.currentTarget; = value;
        }
       
        public DateTime? dateTime
        {
            get => YandexGame.Instance.progressData.dataTime;
            set => YandexGame.Instance.progressData.dataTime = value;
        }
        public void Initialize()
        {
            StartCoroutine(State());
        }

        public void SetNextTarget()
        {
            currentTarget = (currentTarget + 1) % maxTarget;
        }

        public Reward GetRevard()
        {
             dateTime = DateTime.UtcNow;
             return rewards[currentTarget];
        }

        public IEnumerator State()
        {
            while (true)
            {
                UpdateState();
                yield return new WaitForSeconds(1);
            }
        }

        private void UpdateState()
        {
            claimReward = true;
            if (dateTime.HasValue)
            {
                var span = DateTime.UtcNow - dateTime.Value;
                if (span.TotalHours > claimDead)
                {
                    dateTime = null;
                    currentStraik = 0;
                }
                else
                {
                    if (span.TotalHours < claimCounDown)
                    {
                        claimReward = false;
                    }
                }
            }
        }

        public TimeSpan GetCurrentCountDownTime()
        {
           var nextClaim = dateTime.Value.AddHours(claimCounDown);

           return nextClaim - DateTime.UtcNow;
        }
       
    }
}