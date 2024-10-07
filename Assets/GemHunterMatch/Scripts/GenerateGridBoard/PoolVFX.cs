using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class PoolVFX 
    {
        private Dictionary<VisualEffect, Queue<VFXInstance>> lookup = new();
        private List<VFXInstance> allVFX = new();
        private Dictionary<string, Queue<VFXInstance>> pool = new();
        private int startIndex;
        private Transform container;

        public PoolVFX(Transform container = null)
        {
            this.container = container;
        }

        public void Clean()
        {
            allVFX.Clear();
            lookup.Clear();

            startIndex = 0;
        }

        public void UpDate()
        {
            if (allVFX.Count == 0)
                return;

            var endIdx = startIndex + 16;
            while (endIdx >= allVFX.Count) endIdx -= allVFX.Count;

            while (startIndex != endIdx)
            {
                var vfx = allVFX[startIndex];

                if (vfx.Instance.gameObject.activeInHierarchy)
                {
                    var frameDiff = Time.frameCount - vfx.FrameCount;
                    //particle amount is updated every 60 frame, so we make sure we have enough frame since starting the vfx
                    //to check particle amount. if no particles left, we disable the vfx. This is because as most VFX use
                    //GPU event they never get to sleep. This would break if we had vfx that have moment with no particle,
                    //but non of our vfx are like this in this project so this is good enough for us.
                    if (frameDiff > 100 && vfx.Instance.aliveParticleCount == 0)
                    {
                        vfx.Instance.gameObject.SetActive(false);
                    }
                }

                startIndex++;
                while (startIndex >= allVFX.Count) startIndex -= allVFX.Count;
            }
        }

        public void AddNewInstance(VisualEffect prefab, int count)
        {
            if (lookup.ContainsKey(prefab))
                return;

            var queue = new Queue<VFXInstance>(count);

            for (int i = 0; i < count; ++i)
            {
                var instance = Object.Instantiate(prefab, container);
                instance.gameObject.SetActive(false);

                var vfxInstance = new VFXInstance()
                {
                    Instance = instance,
                    FrameCount = Time.frameCount
                };

                queue.Enqueue(vfxInstance);
                allVFX.Add(vfxInstance);
            }

            lookup.Add(prefab, queue);
        }

        public VisualEffect PlayInstance(VisualEffect prefab, Vector3 position)
        {
            var inst = GetInstance(prefab);

            if (inst == null)
                return null;

            inst.transform.position = position;
            inst.Stop();
            inst.Play();

            return inst;
        }

        public VisualEffect PlayInstance(string prefab, Vector3 position)
        {
            if (!pool.TryGetValue(prefab, out var vfxInstance))
            {
                var queue = new Queue<VFXInstance>(3);

                for (int i = 0; i < 3; ++i)
                {
                    CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<VisualEffect>(prefab, container, op =>
                    {
                        var instance = op;
                        instance.gameObject.SetActive(false);
                        var vfx = new VFXInstance()
                        {
                            Instance = instance,
                            FrameCount = Time.frameCount

                        };
                        queue.Enqueue(vfx);
                        allVFX.Add(vfx);
                        lookup.Add(instance, queue);
                        pool.Add(prefab, queue);
                    }));
                }

            }
            //if (inst == null)
            //    return null;

            //inst.transform.position = position;
            //inst.Stop();
            //inst.Play();
            return null;
            //return inst;
        }

        //This both activate the gameobject and update the starting framecount for that Instance so you need to call play the same frame!
        public VisualEffect GetInstance(VisualEffect prefab)
        {
            if (!lookup.TryGetValue(prefab, out var vfxInstance))
            {
                var queue = new Queue<VFXInstance>(3);

                for (int i = 0; i < 3; ++i)
                {
                    var instance = Object.Instantiate(prefab,container);
                    instance.gameObject.SetActive(false);

                    var vfx = new VFXInstance()
                    {
                        Instance = instance,
                        FrameCount = Time.frameCount
                    };

                    queue.Enqueue(vfx);
                    allVFX.Add(vfx);
                }
                lookup.Add(prefab, queue);

            }
            if (lookup.TryGetValue(prefab, out var vfxInstance1))
            {
                var inst = vfxInstance1.Dequeue();
                vfxInstance1.Enqueue(inst);

                inst.FrameCount = Time.frameCount;
                inst.Instance.gameObject.SetActive(true);
                
                return inst.Instance;
            }
            var vfxf = allVFX.Find(vfx => !prefab.gameObject.activeInHierarchy);
            vfxf.Instance.gameObject.SetActive(true);
            return null;
        }

       
    }
}