using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;
using Assets.ParticleEffects.Scripts;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class EffectController : MonoBehaviour
    {

        private HoldEffect effect;

        public void Instatiate(Transform container = null)
        {
            if (effect == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<HoldEffect>("Bubble_Hold_P", container, op =>
                {
                    effect = op;
                    effect.gameObject.SetActive(false);
                }));
            }

        }

        public void SetPos(Vector3 worldPos)
        {
            if (effect != null)
            {
                if (effect.gameObject.activeSelf)
                    effect.ShowTrail(worldPos);
            }
        }

        public void HideVFX()
        {
            if (effect != null) effect.gameObject.SetActive(false);
        }

        public void ShowEffect(Vector3 pos, Vector3 worldPos)
        {
            if (effect != null)
            {
                effect.transform.position = pos;
                effect.gameObject.SetActive(true);
            }
        }
    }
}