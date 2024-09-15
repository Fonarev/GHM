using System;
using System.Collections;

using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class PoolService : MonoBehaviour
    {
        public static PoolService instance;

        internal void AddNewInstance(VisualEffect useEffect, int v)
        {
            throw new NotImplementedException();
        }

        internal VisualEffect GetInstance(VisualEffect useEffect)
        {
            throw new NotImplementedException();
        }

        internal void PlayInstanceAt(VisualEffect effectPrefab, Vector3 position)
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            instance = this;
        }
    }
}