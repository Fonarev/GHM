using Assets.DailyRewards.Scripts;

using System;
using System.Collections;

using UnityEngine;

namespace Assets.WheelOfLuck.Scripts
{
    [Serializable]
    public class Reward
    {
        public RewardType type;
        public Sprite sprite;
        public int amount;
        [Range(0, 1)] public float wight;
    }
}