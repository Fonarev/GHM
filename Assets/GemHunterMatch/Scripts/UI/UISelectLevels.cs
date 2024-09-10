using Assets.YG.Scripts;

using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLevels : MonoBehaviour
    {
        public int location;
        public int count;
        public List<UILevelEntry> levels;
        public  void Init()
        {
            StartCoroutine(LoadLevelEntry());
        }
        private IEnumerator LoadLevelEntry()
        {
            for (int i = 1; i < count; i++)
            {
                int level = location + i;
                var handle = Addressables.InstantiateAsync("LevelEntry");
               
                var ui = handle.Result;
                if (ui.TryGetComponent(out UILevelEntry lvl)== false)
                {
                    throw new Exception($"This Type already exists!!!{ui}");
                }

                lvl.Init(level, TryLevel(level));
                levels.Add(lvl);

                yield return lvl;
            }
        }

        private bool TryLevel(int level)
        {
            if (YandexGame.Instance.progressData.levels.ContainsKey(level))
            {
                return false;
            }
            return true;
        }
    }
}