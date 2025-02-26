using System;

namespace Assets.GemHunterMatch.Scripts
{
    [Serializable]
    public class Location
    {
        public int number;
        public int startLevel;
        public int completedLevels;
        public bool completed;
        public bool isLock;
        public bool isSelected;

        public Location(int number, int startLevel, bool isLock = false, bool isSelected = false)
        {
            this.number = number;
            this.startLevel = startLevel;
            this.isLock = isLock;
            this.isSelected = isSelected;
        }

        public int endNumberLevel {get=> startLevel + 19;}
    }
}