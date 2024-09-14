using Assets.GemHunterMatch.Scripts;

using System.Collections.Generic;

namespace Assets.YG.Scripts
{
    public class ProgressData
    {
        public int coint;
        public int topScore;
        public bool isSilence;
        public Dictionary<int,bool> levels = new();
        public Dictionary<int, Location> locations = new();

        public void CreateDefaultData()
        {
            coint = 0;
            topScore = 0;
            levels.Add(1, false);
            locations[1]= new Location(){ number = 1,completedLevels = 1 };
        }
    }
}