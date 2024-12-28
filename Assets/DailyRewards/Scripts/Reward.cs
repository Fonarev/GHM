using System;

using UnityEngine;

namespace Assets.DailyRewards.Scripts
{
    [Serializable]
    public class Reward 
    {
        public RewardType type;
        public Sprite sprite;
        public int amount;
    }
}