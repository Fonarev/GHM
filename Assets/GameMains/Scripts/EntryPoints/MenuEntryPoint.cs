using Assets.GameMains.Scripts.AudiosSources;

using System.Collections;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class MenuEntryPoint : MonoBehaviour
    {

       public void Initialize(AudioManager audio)
       {
         audio.Play();
       }
    }
}