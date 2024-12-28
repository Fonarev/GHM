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
        internal DateTime? dataTime;
        internal int currentTarget;

        public void CreateDefaultData()
        {
            coins = 0;
            topScore = 0;
            int count = 42;

            for (int i = 0; i < count; i++)
            {
                levels.Add(i, new LevelData() { level = i, isOpened = true });
            }

            //levels.Add(1, new LevelData() { level = 1, isOpened = true });

            locations[1] = new Location()
            {
                number = 1,
                openLevels = 20,
                startLevel = 1,
                maxLevels = 20,
                isLock = true,
                isSelected = true
            };
            locations[2] = new Location()
            {
                number = 2,
                openLevels = 20,
                startLevel = 21,
                maxLevels = 20,
                isLock = true,
                isSelected = true
            };
            locations[3] = new Location()
            {
                number = 3,
                openLevels = 20,
                startLevel = 41,
                maxLevels = 20,
                isLock = true,
                isSelected = true
            };
            bonusGemItem[-1] = 10;
            bonusGemItem[-2] = 10;
            bonusGemItem[-3] = 10;
            bonusGemItem[-4] = 10;
            bonusGemItem[-5] = 10;

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