using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
       
        
     
        public void Initialize()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG"));
           
        }

        

    }
}