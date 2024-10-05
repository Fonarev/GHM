using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using UnityEngine;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform rootBackground;
        
        public void Initialize()
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("BG", rootBackground));
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset("VFX_Bubbles", rootBackground));
        }

    }
}