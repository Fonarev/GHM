using System.Collections;

using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts
{
    public class GamePlayVisualSettings : ScriptableObject
    {
        public float inactivityBeforeHint = 8.0f;
        public VisualEffect gemHoldPrefab;
        public VisualEffect holdTrailPrefab;
        public VisualEffect CoinVFX;
        public VisualEffect WinEffect;
        public VisualEffect LoseEffect;

        public float FallSpeed = 10.0f;
        public AnimationCurve FallAccelerationCurve;
        public AnimationCurve BounceCurve;
        public AnimationCurve SquishCurve;

        public AnimationCurve MatchFlyCurve;
        public AnimationCurve CoinFlyCurve;

        public GameObject BonusModePrefab;

        public GameObject HintPrefab;

    }
}