using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;
using Assets.ParticleEffects.Scripts;

using Match3;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class EffectController : MonoBehaviour
    {

        private HoldEffect EffectInstance;

        public void Instatiate(Transform container = null)
        {
            if (EffectInstance == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<HoldEffect>("Bubble_Hold_P", container, op =>
                {
                    EffectInstance = op;
                    EffectInstance.gameObject.SetActive(false);
                }));
            }

        }

        public void SetPos(Vector3 worldPos)
        {
            if (EffectInstance != null)
            {
                if (EffectInstance.gameObject.activeSelf)
                    EffectInstance.ShowTrail(worldPos);
            }
        }

        public void HideVFX()
        {
            if (EffectInstance != null) EffectInstance.gameObject.SetActive(false);
        }

        public void ShowEffect(Vector3 pos, Vector3 worldPos)
        {
            if (EffectInstance != null)
            {
                EffectInstance.transform.position = pos;
                EffectInstance.gameObject.SetActive(true);
            }

            //if (holdTrailInstance != null)
            //{
            //    holdTrailInstance.transform.position = worldPos;
            //    holdTrailInstance.gameObject.SetActive(true);
            //}
        }
    }
}