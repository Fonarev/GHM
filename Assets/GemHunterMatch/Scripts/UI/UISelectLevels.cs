using Assets.GemHunterMatch.Scripts.Loaders;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLevels : MonoBehaviour
    {
        public RectTransform rootLevelEntry;
        public List<UILevelEntry> levels;

        private LocalAssetLoader loader;

        private void OnEnable()
        {
          
        }
       
        private void OnDisable()
        {
           if(loader!= null) loader.UnLoadAll();
        }

        public async void Init(int startCount,int amountLevel)
        {
            for (int i = 0; i < amountLevel; i++)
            {
                int number = startCount + i;
                var level = levels[i];
                if (level != null)
                {
                    level.Init(number);
                }
                else
                {
                    loader = new LocalAssetLoader(true);
                    UILevelEntry newLevel = await loader.Load<UILevelEntry>("LevelEntry", rootLevelEntry);
                    levels.Add(newLevel);   
                    newLevel.Init(number);
                }
            }
        }
        private void UpDateVeiw()
        {
            foreach (var level in levels) { level.UpDateView(); }
        }
    }
}