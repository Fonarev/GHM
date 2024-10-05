using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.AssetLoaders
{
    public class LoaderAsset 
    {
        private static Dictionary<GameObject, AsyncOperationHandle<GameObject>> instatiateAssets = new();
        private static Dictionary<object, AsyncOperationHandle> loadingAssets = new();

        private IEnumerator Load<T>(AssetReference assetReference, Action<T> callback = null)
        {
            if (!loadingAssets.ContainsKey(typeof(T)))
            {
                AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();

                yield return handle;

                var result = handle.Result;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (callback != null)
                        callback.Invoke(result);

                    loadingAssets[typeof(T)] = handle;
                }
                else
                {
                    Debug.LogError($"Failed to load asset: {assetReference}");
                }
            }
        }

        public static IEnumerator Load<T>(string name , Action<T> callback = null)
        {
            if (!loadingAssets.ContainsKey(typeof(T)))
            {
                AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);

                yield return handle;

                var result = handle.Result;
              
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (callback != null)
                        callback.Invoke(result);

                    loadingAssets[result] = handle;
                }
                else
                {
                    Debug.LogError($"Failed to load asset: {name}");
                }
            }
          
        }

        public static IEnumerator LoadList<T>(string assetName, Action<T> callback = null)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(assetName, op => 
            {
                if (op != null)
                    callback.Invoke(op);

            });

            yield return handle;
          
        }

        public static IEnumerator InstantiateAsset(string assetName, Transform parent = null, Action<GameObject> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (callback != null)
                {
                    callback.Invoke(asset);
                }

                instatiateAssets[asset] = handle;
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(AssetReference reference, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(reference, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (callback != null)
                {
                    if (!asset.TryGetComponent(out T loadinged))
                        throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                    callback.Invoke(loadinged);
                }

                instatiateAssets[asset] = handle;
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {reference}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(AssetReference reference, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(reference, pos, rot, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (callback != null)
                {
                    if (!asset.TryGetComponent(out T loadinged))
                        throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                    callback.Invoke(loadinged);
                }

                instatiateAssets[asset] = handle;
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {reference}");
            }
        }
        
        public static IEnumerator InstantiateAsset<T>(string assetName, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (callback != null)
                {
                    if (!asset.TryGetComponent(out T loadinged))
                        throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                    callback.Invoke(loadinged);
                }

                instatiateAssets[asset] = handle;
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
            }
        }

        public static IEnumerator InstantiateAsset<T>(string assetName, Vector3 pos, Quaternion rot, Transform parent = null, Action<T> callback = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName, pos, rot, parent);

            yield return handle;

            GameObject asset = handle.Result;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (callback != null)
                {
                    if (!asset.TryGetComponent(out T loadinged))
                        throw new NullReferenceException($"Non Component from Asset {typeof(T)}");

                    callback.Invoke(loadinged);
                }

                instatiateAssets[asset] = handle;
            }
            else
            {
                throw new NullReferenceException($"Failed to load Asset: {assetName}");
            }
        }

        public void ReleaseInstance(GameObject go)
        {
            if (!instatiateAssets.TryGetValue(go, out AsyncOperationHandle<GameObject> handle))
            {
                throw new NullReferenceException($"Non Instatiate Asset {go}");
            }

            go.SetActive(false);
            Addressables.ReleaseInstance(handle);
            instatiateAssets.Remove(go);
        }

        public static void UnLoaading(object type )
        {
            if (loadingAssets.TryGetValue(type, out AsyncOperationHandle handle))
            {
                Addressables.Release(handle);
                loadingAssets.Remove(type);
            }
            else
            {
                throw new NullReferenceException($"Non loadinged {type}");
            }
        }
    }
}