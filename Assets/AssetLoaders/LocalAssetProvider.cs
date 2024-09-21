using Assets.GameMains.Scripts.Expansion;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.AssetLoaders
{
    public class LocalAssetProvider
    {
        private Dictionary<Type, GameObject> loadingedAssets;
        LoaderAsset loaderAsset;

        public LocalAssetProvider()
        {
            loaderAsset = new();
        }

        public void Instatiate<T>(AssetReference reference, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<T>(reference, pos, rot, parent, callback));
        }

        public void Instatiate<T>(AssetReference reference, Transform parent = null, Action<T> callback = null)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<T>(reference, parent, callback));
        }

        public void Instatiate<T>(string assetName, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<T>(assetName, pos, rot, parent, callback));
        }

        public void Instatiate<T>(string assetName, Transform parent = null, Action<T> callback = null)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<T>(assetName, parent, callback));
        }

        //public T Instatiate<T>(AssetReference reference, Vector3 pos, Quaternion rot, Transform parent = null)
        //{
        //    T result = default;

        //    CoroutineHandler.StartRoutine(loaderAsset.InstantiateAsset<T>(reference, pos, rot, parent, (asset) =>
        //    {
        //        if (asset != null)
        //        {
        //            if (asset.TryGetComponent(out T component))
        //                result = component;
        //            else
        //                new NullReferenceException($"Non Asset from Addressables {typeof(T)}");

        //        }
        //        else
        //            Console.WriteLine("Failed to instantiate asset.");
        //    })
        //      );

        //    return result;
        //}
        //public T InstatiateAsset<T>(AssetReference reference)
        //{
        //    T result = default;

        //    CoroutineHandler.StartRoutine(loaderAsset.InstantiateAsset<T>(reference, op =>
        //    {
        //        if (op != null)
        //            result = op;
        //        else
        //            Console.WriteLine("Failed to instantiate Gem.");
        //    })
        //      );
        //    return result;
        //}

        //public void UnLoadingAsset(GameObject go) => loaderAsset.ReleaseInstance(go);

    }
}