using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using Match3;

using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class VFXController
    {
        private VisualSetting visualSetting;
       
        private VisualEffect gemHoldVFXInstance;
        private VisualEffect holdTrailInstance;

        public VFXController(VisualSetting visualSetting)
        {
            this.visualSetting = visualSetting;
        }

        public void Instatiate(Transform container = null)
        {
            if (gemHoldVFXInstance == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<VisualEffect>(visualSetting.GemHold, container, op =>
                {
                    gemHoldVFXInstance = op;
                    gemHoldVFXInstance.gameObject.SetActive(false);
                }));
            }

            if (holdTrailInstance == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<VisualEffect>(visualSetting.HoldTrail, container, op =>
                {
                    holdTrailInstance = op;
                    holdTrailInstance.gameObject.SetActive(false);
                }));
            }
        }

        public void SetPos(Vector3 worldPos)
        {
            if (holdTrailInstance != null)
            {
                if (holdTrailInstance.gameObject.activeSelf)
                    holdTrailInstance.transform.position = worldPos;
            }
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