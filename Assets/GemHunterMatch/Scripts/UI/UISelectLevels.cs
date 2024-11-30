using Assets.AssetLoaders;
using Assets.GemHunterMatch.Scripts.Loaders;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLevels : MonoBehaviour
    {
        public RectTransform rootLevelEntry;
        public List<UILevelEntry> levels;

        public void Init(int startCount,int amountLevel)
        {
            for (int i = 0; i < amountLevel; i++)
            {
                int number = startCount + i;
                var level = levels[number-1];

                if (level != null)
                {
                    level.Init(number);
                }
                else
                {
                    StartCoroutine(LoaderAsset.InstantiateAsset<UILevelEntry>("LevelEntry", rootLevelEntry, op =>
                    {
                        var newLevel = op;
                        levels.Add(newLevel);
                        newLevel.Init(number);
                    }));
                   
                }
            }
        }
       
    }
}