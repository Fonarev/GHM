using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.ParticleEffects.Scripts
{
    public class PoolEffect 
    {
        private List<ParticleEffect> pool = new();
        private Transform container;
        private ParticleEffect selectedEffect;

        public PoolEffect(Transform container = null)
        {
            this.container = container;
        }

        public void Create(EffectType type, int size = 1)
        {
            for (int i = 0; i < size; i++)
            {
                CreatObject(type);
            }
        }
      
        public ParticleEffect PlayInstance(EffectType type, Vector3 position,bool isActiveByDefault = true)
        {
            if(IsFreeObject(type, isActiveByDefault))
            {
              selectedEffect.transform.position = position;
            }
            else
            {
                CreatObject(type,position, isActiveByDefault);
            }
           
            return selectedEffect;
        }

        private bool IsFreeObject(EffectType type, bool isActiveByDefault)
        {
            List<ParticleEffect> list = pool.FindAll(t => t.type == type);

            foreach (var obj in list)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(isActiveByDefault);
                    selectedEffect = obj;
                    return true;
                }
            }

            selectedEffect = null;

            return false;
        }

        private void CreatObject(EffectType type,Vector3 pos, bool isActiveByDefolt = false)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<ParticleEffect>(type.ToString(), container, op =>
            {
                op.transform.position = pos;
                op.gameObject.SetActive(isActiveByDefolt);
               
                pool.Add(op);
            }));
        }

        private ParticleEffect CreatObject(EffectType type, bool isActiveByDefolt = false)
        {
            ParticleEffect creatObject = null;

            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<ParticleEffect>(type.ToString(), container, op =>
            {
                creatObject = op;
                creatObject.gameObject.SetActive(isActiveByDefolt);
                pool.Add(creatObject);
            }));

            return creatObject;
        }
    }
}