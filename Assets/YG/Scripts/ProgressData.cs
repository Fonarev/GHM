using Assets.GemHunterMatch.Scripts;

using System;
using System.Collections.Generic;

namespace Assets.YG.Scripts
{
    [Serializable]
    public class ProgressData
    {
        public int coins;
        public int Score;
        public int topScore;
        public int order;

        public bool music;
        public bool effectAudio;

        public Dictionary<int, Location> locations = new();
        public Dictionary<int, int> bonusGemItem = new();

        public DateTime? dataTime;
        public int currentTarget;
        public DateTime? dataTimeWheel;

        public void Reset()
        {
            coins = 0;
            topScore = 0;
            Score = 0;
            locations.Clear();
            CreateDefaultData();
        }

        public void CreateDefaultData()
        {
            music = true;
            effectAudio = true;

            locations[1] = new Location(1, 1, true, true);

            locations[2] = new Location(2, 21, false);

            locations[3] = new Location(3, 41, false);

            locations[4] = new Location(4, 61);
            locations[5] = new Location(5, 81);
            locations[6] = new Location(6, 101);

        }

        public int GetBonusGemAmount(int type)
        {
            int amount;
            if (bonusGemItem.TryGetValue(type, out amount))
            {
                return amount;
            }
            
            return bonusGemItem[type] = 0;
        }

        public void AddBonusGem(int type,int value = 1)
        {
            if (bonusGemItem.TryGetValue(type, out var amount))
            {
                bonusGemItem[type] += value;
            }
            else
            {
                bonusGemItem[type] = value;
            }
            YandexGame.Instance.Save();
        }

    }
}