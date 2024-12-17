using System;

namespace Assets.DailyRewards.Scripts
{
    [Serializable]
    public class Reward 
    {
        public RewardType type;
        public int amount;
    }
}