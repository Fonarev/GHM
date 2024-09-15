using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class VFXController
    {
        private VisualEffect gemHoldPrefab;
        private VisualEffect holdTrailPrefab;
        private VisualEffect gemHoldVFXInstance;
        private VisualEffect holdTrailInstance;

        public VFXController(VisualEffect gemHoldPrefab, VisualEffect holdTrailPrefab)
        {
            this.gemHoldPrefab = gemHoldPrefab;
            this.holdTrailPrefab = holdTrailPrefab;
        }

        public void Instatiate(Transform parent = null)
        {
            if (gemHoldPrefab != null)
            {
                gemHoldVFXInstance = Object.Instantiate(gemHoldPrefab, parent);
                gemHoldVFXInstance.gameObject.SetActive(false);
            }

            if (holdTrailPrefab != null)
            {
                holdTrailInstance = Object.Instantiate(holdTrailPrefab,parent);
                holdTrailInstance.gameObject.SetActive(false);
            }
        }

        public void SetPos(Vector3 worldPos)
        {
            if (holdTrailInstance.gameObject.activeSelf)
                holdTrailInstance.transform.position = worldPos;
        }

        public void HideVFX()
        {
            if (gemHoldVFXInstance != null) gemHoldVFXInstance.gameObject.SetActive(false);
            if (holdTrailInstance != null) holdTrailInstance.gameObject.SetActive(false);
        }

        public void ShowVFX(Vector3 pos,Vector3 worldPos)
        {
            if (gemHoldVFXInstance != null)
            {
                gemHoldVFXInstance.transform.position = pos;
                gemHoldVFXInstance.gameObject.SetActive(true);
            }

            if (holdTrailInstance != null)
            {
                holdTrailInstance.transform.position = worldPos;
                holdTrailInstance.gameObject.SetActive(true);
            }
        }
    }
}