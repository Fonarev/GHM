using Assets.GemHunterMatch.Scripts;

using Match3;

using System;

namespace Assets.GemHunterMatch
{
    [Serializable]
    public class GemGoal
    {
        public Gem Gem;
        public int Count;
    }
    [Serializable]
    public class ObstacleGoal
    {
        public Obstacle Obstacle;
        public int Count;
    }
    [Serializable]
    public class UnderGemGoal
    {
        public UnderGem UnderGem;
        public int Count;
    }
}