using System.Collections.Generic;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    [CreateAssetMenu(fileName = "Rewards", menuName = "RewardsData/RewardsDataBase")]
    public class RewardsDataBase : ScriptableObject
    {
        public List<Reward> rewards;
    }
}