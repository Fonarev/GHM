using System.Collections;

using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [field: SerializeField] public EffectType type;

    public bool OnlyDeactivate = true;
    public ParticleSystem Ps => GetComponent<ParticleSystem>();
  
    private void OnEnable()
    {
        StartCoroutine("CheckIfAlive");
    }
 
    private IEnumerator CheckIfAlive()
    {
        while (true && Ps != null)
        {
            yield return new WaitForSeconds(0.05f);

            if (!Ps.IsAlive(true))
            {
                if (OnlyDeactivate)
                {
                    this.gameObject.SetActive(false);
                }

                else
                    GameObject.Destroy(this.gameObject);
                break;
            }
        }

    }
}
