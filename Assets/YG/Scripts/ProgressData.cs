using System.Collections.Generic;

namespace Assets.YG.Scripts
{
    public class ProgressData
    {
        public int coint;
        public int topScore;
        public bool isSilence;
        public Dictionary<string,int> partItems = new();

        public void CreateDefaultData()
        {
            coint = 0;
            topScore = 0;
        }
    }
}