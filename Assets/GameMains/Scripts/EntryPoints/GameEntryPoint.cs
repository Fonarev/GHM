using Assets.GemHunterMatch.Scripts;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GameMains.Scripts.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
      
        public AssetReference reference;
        public AssetReference gridAsset;
        public GridBoard gridBoard;
        public void Initialize()
        {
            //gridBoard.Initialize(this);
            Load(gridAsset);
            //Load(reference);
        }
        private async void Load(AssetReference reference)
        {
            var handle = await reference.InstantiateAsync().Task;

            var board =  handle.GetComponent<GridBoard>();
            board.Initialize(this);
        }
       
    }
}