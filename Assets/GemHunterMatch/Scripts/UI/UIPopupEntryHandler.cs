using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIPopupEntryHandler : MonoBehaviour
    {
        [Header("VisualSettings")]
        [Range(0.0f, 3.0f)] public float timeAnimation;
        public UIPopupEntry visualGem;
        public RectTransform containerEntry;
        public AnimationCurve MatchFlyCurve;
        public AnimationCurve CoinFlyCurve;
       
        private List<UIAnimationEntry> currentGemAnimations = new();

        private Camera cameraMain => Camera.main;

        private void Update()
        {
            if (currentGemAnimations.Count <= 0) return;

            var matchCurve = MatchFlyCurve;

            for (int i = 0; i < currentGemAnimations.Count; ++i)
            {
                var anim = currentGemAnimations[i];

                anim.Time += Time.deltaTime;

                Vector3 panelVector = Vector3.zero;

                if (anim.Curve != null)
                {
                    var startToEnd = (Vector3.up * 20) - anim.WorldPosition;
                    Vector3 perpendicular;
                    var angle = Vector3.SignedAngle(Vector3.up, startToEnd, Vector3.forward);
                    if (angle < 0)
                        perpendicular = (Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.left).normalized;
                    else
                        perpendicular = (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right).normalized;

                    float angleAmount = Mathf.Clamp01(Mathf.Abs(angle) / 10.0f);

                    float amount = anim.Curve.Evaluate(anim.Time) * angleAmount;
                    perpendicular *= amount;

                    var worldPos = anim.WorldPosition + perpendicular;
                    //var panelPos = (Vector3)RuntimePanelUtils.CameraTransformWorldToPanel(visualGem.panel, worldPos,

                    //panelVector = panelPos - anim.StartPosition;
                }

                //var newPos = Vector2.Lerp(anim.StartPosition, anim.EndPosition, anim.Time);
                var newPos = anim.StartPosition + anim.StartToEnd * matchCurve.Evaluate(anim.Time) + panelVector;

                if (anim.Time >= timeAnimation)
                {
                    Destroy(anim.UIElement.gameObject);
                    currentGemAnimations.RemoveAt(i);
                    i--;

                    //if (anim.EndClip != null)
                    //GameManager.Instance.PlaySFX(anim.EndClip);
                }
                else
                {
                    anim.UIElement.transform.position = newPos;
                }
            }
        }
        
        public void Show(Sprite sprite, Vector3 position,Vector3 target)
        {
            var pos = cameraMain.WorldToScreenPoint(position);

            var elem = Instantiate(visualGem, pos, Quaternion.identity, containerEntry);

            elem.Set(sprite, pos);

            currentGemAnimations.Add(new UIAnimationEntry()
            {
                Time = 0.0f,
                WorldPosition = position,
                StartPosition = pos,
                StartToEnd = target - pos,
                UIElement = elem,
                Curve = MatchFlyCurve
            }); 
        }
    }
}