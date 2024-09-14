using System;

namespace Assets.GemHunterMatch.Scripts
{
    [Serializable]
    public class Location 
    {
        public int number;
        public int maxLevels;
        public int completedLevels;
        public bool completed;
        public bool isLock;

    }
}