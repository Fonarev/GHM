using Assets.GemHunterMatch.Scripts;

using System;
using System.Collections.Generic;

namespace Assets.YG.Scripts
{
    [Serializable]
    public class ProgressData
    {
        public int coins;
        public int topScore;
        public bool isSilence;
        public Dictionary<int,bool> levels = new();
        public Dictionary<int, Location> locations = new();
        public Dictionary<int, int> bonusGemItem = new();

        public void CreateDefaultData()
        {
            coins = 0;
            topScore = 0;
            levels.Add(1, false);
            locations[1]= new Location(){ number = 1,completedLevels = 1 };

            bonusGemItem[-1] = 5;
            bonusGemItem[-2] = 5;
            bonusGemItem[-3] = 5;
            bonusGemItem[-4] = 5;
            bonusGemItem[-5] = 5;

        }

        public int GetBonusGemAmount(int type)
        {
            int amount;
            if (bonusGemItem.TryGetValue(type, out amount))
            {
                return amount;
            }

            return amount;
        }
     
    }
}