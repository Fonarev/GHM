using System;

namespace Assets.GemHunterMatch.Scripts
{
    [Serializable]
    public class Location 
    {
        public int number;
        public int maxLevels;
        public int startLevel;
        public int completedLevels;
        public int openLevels;
        public bool completed;
        public bool isLock;
        public bool isSelected;
        public int endindexLevel{ get => startLevel + maxLevels - 1; }
    }
}