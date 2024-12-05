using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    [CreateAssetMenu(fileName = "Level", menuName = "LevelConfig",order = 1)]
    public class LevelConfig : ScriptableObject
    {
        public int level;

        [Header("Settings Conditions")]
        public int MaxMove;
        public int LowMoveTrigger = 5;
        [field:SerializeField] public GemGoal[] GemGoals{ get; private set; }
        [field: SerializeField] public ObstacleGoal[] ObstaclesGoals { get; private set; }
        [field: SerializeField] public UnderGemGoal[] UnderGemGoals { get; private set; }

        [Header("Visuals")]
        public float BorderMargin = 0.3f;
        public SpriteRenderer Background;
        public string gridBoardReference;

        public Tutorial tutorial;

    }
}