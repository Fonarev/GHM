using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class SelectLevelsUI : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private List<LevelButtonSelectUI> levelButtons;

        public void Init(Location location)
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                LevelButtonSelectUI level = levelButtons[i];
                int numberLevel = location.startLevel + i;
                level.Init(location.number, numberLevel);
            }

            for (int i = 0; i < location.completedLevels; i++)
            {
                LevelButtonSelectUI level = levelButtons[i];
                level.Updater(false, true);
            }

            int openLastLevel = location.completedLevels;
            if (openLastLevel != levelButtons.Count && location.isLock)
            {
                LevelButtonSelectUI openLevel = levelButtons[openLastLevel];
                openLevel.Updater(false,false);
            }

        }
       
    }
}