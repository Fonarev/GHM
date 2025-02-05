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
        public int endNumberLevel {get=> startLevel + 19;}
    }
}