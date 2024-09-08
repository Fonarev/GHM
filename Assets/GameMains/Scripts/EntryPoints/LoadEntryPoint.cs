
using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class LoadEntryPoint : MonoBehaviour
    {

       public void Initalize()
       {
            StartCoroutine(Load());
       }
       private IEnumerator Load()
       {
            yield return null;
       }
    }
}