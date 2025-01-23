using System;
using System.Collections;

using UnityEngine;

namespace Assets.GemHunterMatch.ShopStore.Scripts
{ [CreateAssetMenu(fileName ="ShopItems",menuName ="Shop/Items")]
    public class ShopItemData : ScriptableObject
    {
        public ShopItem[] items;
    }
}