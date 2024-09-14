using Match3;
using System;

using UnityEditor;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class LevelConfig : ScriptableObject
    {
        [Serializable]
        public class GemGoal
        {
            public Gem Gem;
            public int Count;
        }

        public string LevelName = "Level";
        public int MaxMove;
        public int LowMoveTrigger = 10;
        public GemGoal[] Goals;

        [Header("Visuals")]
        public float BorderMargin = 0.3f;
        public SpriteRenderer Background;

    
    }
}