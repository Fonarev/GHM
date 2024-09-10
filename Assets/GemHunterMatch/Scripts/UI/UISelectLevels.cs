using Assets.GemHunterMatch.Scripts.Loaders;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLevels : MonoBehaviour
    {
        public int count;
        public RectTransform rootLevelEntry;
        public List<UILevelEntry> levels;
        private LocalAssetLoader loader;
        private void OnEnable()
        {
            UpDateVeiw();
        }
       
        private void OnDisable()
        {
           if(loader!= null) loader.UnLoadAll();
        }

        public async void Init(int location)
        {
            for (int i = 0; i < count; i++)
            {
                int number = location + i;
                var level = levels[i];
                if (level != null)
                {
                    level.Init(number + 1);
                }
                else
                {
                    loader = new LocalAssetLoader(true);
                    UILevelEntry newLevel = await loader.Load<UILevelEntry>("LevelEntry", rootLevelEntry);
                    levels.Add(newLevel);   
                    newLevel.Init(number + 1);
                }
            }
        }
        private void UpDateVeiw()
        {
            foreach (var level in levels) { level.UpDateView(); }
        }
    }
}