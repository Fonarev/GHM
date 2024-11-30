using UnityEngine;

namespace Assets.ParticleEffects.Scripts
{
    public class HoldEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        public void OnDisable()
        {
            if(particle != null )
               particle.transform.position = Vector3.zero;
        }

        public void ShowTrail(Vector3 pos)
        {
            particle.transform.position = pos;
        }
    }
}