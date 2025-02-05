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
        public int order;

        public bool isSilence;
        public bool music;
        public bool effectAudio;

        //public Dictionary<int,LevelData> levels = new();
        public Dictionary<int, Location> locations = new();
        public int lastSelectedLevel;
        public Dictionary<int, int> bonusGemItem = new();

        public int Score;
        public DateTime? dataTime;
        public int currentTarget;
        public DateTime? dataTimeWheel;

        public void Reset()
        {
            coins = 0;
            topScore = 0;
            //levels.Clear();
            lastSelectedLevel = 1;
            //levels.Add(1, new LevelData() { levelConfig = 1, isOpened = true });
            locations.Clear();
            locations[1] = new Location()
            {
                number = 1,
                startLevel = 1,
                isLock = true,
                isSelected = true
            };
        }
        public void CreateDefaultData()
        {
            coins = 0;
            topScore = 0;

            music = true;
            effectAudio = true;

            lastSelectedLevel = 1;
            //int count = 42;

            //for (int i = 0; i < count; i++)
            //{
            //    levels.Add(i, new LevelData() { levelConfig = i, isOpened = true });
            //}

            //levelButtons.Add(1, new LevelData() { levelConfig = 1, isOpened = true });

            locations[1] = new Location()
            {
                number = 1,
                startLevel = 1,
                isLock = true,
                isSelected = true
            };
            locations[2] = new Location()
            {
                number = 2,
                startLevel = 21,
            };
            locations[3] = new Location()
            {
                number = 3,
                startLevel = 41,
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

        public void AddBonusGem(int type,int value = 1)
        {
            if (bonusGemItem.TryGetValue(type, out var amount))
            {
                amount += value;
                bonusGemItem[type] += 1;
            }
            else
            {
                bonusGemItem[type] = 1;
            }
            YandexGame.Instance.Save();
        }

    }
}