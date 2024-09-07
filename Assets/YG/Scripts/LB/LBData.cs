using System;


namespace Assets.YG.Scripts.LB
{
    [Serializable]
    public class LBData
    {
        public string technoName;
        public string entries;
        public bool isDefault;
        public bool isInvertSortOrder;
        public int decimalOffset;
        public string type;
        public LBPlayerData[] players;
        public LBThisPlayerData thisPlayer;
    }
}