using Assets.YG.Scripts;

using System;
using System.Collections.Generic;

namespace Assets.GemHunterMatch
{
    public class Inventory 
    {  
        public event Action<int, int> OnChangedBonus;

        public Dictionary<int, int> bonusGems = new();

        public void Init()
        {
            bonusGems = YandexGame.Instance.progressData.bonusGemItem;
        }

        public void AddBonus(int type,int amount = 1)
        {
            bonusGems[type] = amount;
        }

        public void SpanseBonus(int type, int amount = 1)
        {
            if (bonusGems.TryGetValue(type, out int count))
            {
                count -= amount;
                OnChangedBonus.Invoke(type, count);
            }
        }

        public bool CheckBonus(int type, int amount = 1)
        { 
            if (bonusGems.TryGetValue(type, out int count))
            {
                return count >= amount? true : false;
            }

            return false;
        }
    }
}