using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.Loaders
{
    public class LocalAssetLoader 
    {
        private GameObject cach;
        private Dictionary<Type, GameObject> cachsMap = new();
        private List<GameObject> cachs = new();
        private Type currentedType;
        private bool isMessage;

        public LocalAssetLoader(bool isMessage = false)
        {
            this.isMessage = isMessage;
        }

        public async Task<T> Load<T>(string assetId,Transform transform = null)
        {
            var handle = Addressables.InstantiateAsync(assetId,transform);
            cach = await handle.Task;
            if(cach.TryGetComponent(out T loaded)== false)
            {
                throw new NullReferenceException($"Null type {typeof(T)} in Asset Addresables");
            }
            cachs.Add(cach);
            Message($"Instance {cach.name}");
            return loaded;
        }

        public void UnLoad(Type type)
        {
            if (cachsMap.ContainsKey(type))
            {
                cachsMap[type].SetActive(false);
                Addressables.ReleaseInstance(cachsMap[type]);
                Message($"Release {cachsMap[type].name}");
                cachsMap.Remove(type);
            }
        }
        public void UnLoadAll()
        {
            foreach (var go in cachs)
            {
                Addressables.ReleaseInstance(go);
                Message($"Release {go.name}");
            }
            cachs.Clear();
        }
     public void Message(string  message)
     {
            Debug.Log(message);
     }
    }
}