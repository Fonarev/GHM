using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    class UIAnimationEntry
    {
        public UIPopupEntry UIElement;
        public Vector3 WorldPosition;
        public Vector3 StartPosition;
        public Vector3 StartToEnd;
        public float Time;

        public AnimationCurve Curve;

        //this is played when the animation reach its end position;
        public AudioClip EndClip;
    }
}