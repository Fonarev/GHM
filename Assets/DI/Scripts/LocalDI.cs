using System.Collections;

using UnityEngine;

namespace Assets.DI.Scripts
{
    public class LocalDI : AbstractDI
    {
        private GlobalDI globalDI;

        public bool IsAllInitiallize { get; private set; }
        public ContainerDI Container { get; private set; }

        private void Start()
        {
            FindGlobalPacker();

            StartCoroutine(InitRoutine());
        }

        private IEnumerator InitRoutine()
        {
            while (!globalDI.IsAllInitiallize)
                yield return globalDI.Initialize();

            IsAllInitiallize = Initialize();
        }

        private bool Initialize()
        {
            Container = new(globalDI.Container);

            foreach (var installer in installers)
            {
                installer.Initialize(Container);
                installer.Installize();
                installer.InstallizeMono();
            }

            Message($"Installize {this}");

            return true;
        }

        private void FindGlobalPacker()
        {
            globalDI = FindObjectOfType<GlobalDI>();

            if (globalDI == null) globalDI = CreateGlobalPart();
        }

        private GlobalDI CreateGlobalPart()
        {
            GlobalDI pref = Resources.Load<GlobalDI>("GlobalDI");

            if (pref == null)
            {
                var newGlobalPacker = Instantiate(new GameObject("GlobalDI"));
                var globalPacker = newGlobalPacker.AddComponent<GlobalDI>();
                globalPacker.Initialize();

                return globalPacker;
            }

            GlobalDI obj = Instantiate(pref);

            return globalDI = pref;
        }

    }
}