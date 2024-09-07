using System.Collections;

using UnityEngine;

namespace Assets.DI.Scripts
{
    public class GlobalDI : AbstractDI
    {
        public bool IsAllInitiallize { get; private set; }
        public ContainerDI Container { get; private set; }

        public bool Initialize()
        {
            Container = new();

            foreach (var installer in installers)
            {
                installer.Initialize(Container);
                installer.Installize();
                installer.InstallizeMono();
            }

            IsAllInitiallize = true;

            DontDestroyOnLoad(this.gameObject);

            Message($"Installize {this}");

            return IsAllInitiallize;
        }
        public IEnumerator InitializeRoutine()
        {
            Container = new();
            IsAllInitiallize = false;

            while (IsAllInitiallize)
            {
                foreach (var installer in installers)
                {
                    installer.Initialize(Container);
                    installer.Installize();
                    installer.InstallizeMono();
                }

                IsAllInitiallize = true;
                yield return IsAllInitiallize;
            }

            DontDestroyOnLoad(this.gameObject);
            Message($"Installize {this}");
        }

    }
}