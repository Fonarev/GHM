using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts
{
    [CreateAssetMenu(fileName = "Level", menuName = "LevelConfig",order = 1)]
    public class LevelConfig : ScriptableObject
    {
        public int level;

        [Header("Settings Conditions")]
        public int MaxMove;
        public int LowMoveTrigger = 5;
        [field:SerializeField] public GemGoal[] Goals{ get; private set; }

        [Header("Visuals")]
        public float BorderMargin = 0.3f;
        public SpriteRenderer Background;
        public AssetReferenceT<GridBoard> gridBoardReference;
    }
}