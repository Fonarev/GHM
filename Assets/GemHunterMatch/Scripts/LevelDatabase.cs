using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts
{
    public class LevelDatabase 
    {
       private static Dictionary<int, LevelConfig> levelsMap;

       static public LevelConfig  GetLevel(int level)
       {
            LevelConfig levelConfig;

            if (levelsMap == null || !levelsMap.TryGetValue(level, out levelConfig))
                throw new NullReferenceException($"No level from the downloadable LevelDatabase {level}");

            return levelConfig;
       }

       static public IEnumerator Load()
       {
            if (levelsMap == null)
            {
                levelsMap = new();

                yield return Addressables.LoadAssetsAsync<LevelConfig>("level", op =>
                {
                    if (op != null)
                    {
                        if (!levelsMap.ContainsKey(op.level))
                            levelsMap.Add(op.level, op);
                    }
                });
            }
       }
    }
}