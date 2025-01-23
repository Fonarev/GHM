using Match3;

using System;

using UnityEngine;

namespace Assets.GemHunterMatch.ShopStore.Scripts
{
    [Serializable]
    public class ShopItem 
    {
        public BonusGem bonusGem;
        public Sprite itemIcon;
        public string itemName;
        public int price;
    }
}