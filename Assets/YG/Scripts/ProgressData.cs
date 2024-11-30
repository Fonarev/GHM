using Assets.GemHunterMatch.Scripts;

using Match3;

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
        public Dictionary<int,LevelData> levels = new();
        public Dictionary<int, Location> locations = new();
        public Dictionary<int, int> bonusGemItem = new();
        public int Score;

        public void CreateDefaultData()
        {
            coins = 0;
            topScore = 0;

            levels.Add(1, new LevelData() { level = 1, isOpened = true });


            locations[1] = new Location()
            {
                number = 1,
                openLevels = 1,
                startLevel = 1,
                maxLevels = 20,
                isLock = true,
                isSelected = true
            };

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