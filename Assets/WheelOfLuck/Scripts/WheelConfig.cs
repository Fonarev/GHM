using System.Collections.Generic;

using UnityEngine;

namespace Assets.WheelOfLuck.Scripts
{
    [CreateAssetMenu(fileName = "Rewards", menuName = "RewardsData/RewardsWheel")]
    public class WheelConfig : ScriptableObject
    {
        public int AmountCell;
        public float rotationSpeed;
        public float maxSpeedRotateTime;
      
        public float accelerationTime;
        public int numberSpins;
        public List<Reward> cells;
    }
}