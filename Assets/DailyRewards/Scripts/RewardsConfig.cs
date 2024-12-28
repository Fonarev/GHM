using System.Collections.Generic;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    [CreateAssetMenu(fileName = "Rewards", menuName = "RewardsData/RewardsDataBase")]
    public class RewardsConfig : ScriptableObject
    {
        public float timeCountDown = 24;
        public float timeReset = 48;
       
        public List<Reward> rewards;
    }
}