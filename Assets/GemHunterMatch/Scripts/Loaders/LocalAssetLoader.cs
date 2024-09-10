using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.Loaders
{
    public class LocalAssetLoader 
    {
        private GameObject cach;

        public async Task<T> Load<T>(string assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            cach = await handle.Task;
            if(cach.TryGetComponent(out T loaded)== false)
            {
                throw new NullReferenceException($"Null type {typeof(T)} in Asset Addresables");
            }
            return loaded;
        }

        public void UnLoad()
        {
            if (cach != null)
            {
                Addressables.ReleaseInstance(cach);
                cach = null;
            }

        }

    }
}