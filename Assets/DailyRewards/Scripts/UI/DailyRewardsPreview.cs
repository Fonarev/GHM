using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.DailyRewards.Scripts.UI
{
    public class DailyRewardsPreview : MonoBehaviour
    {
        [SerializeField] private List<RewardPreview> rewards;

       
       public void Init()
       {
            foreach (var reward in rewards)
            {
                
            }
        }
    }
}