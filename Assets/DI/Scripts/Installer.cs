using System.Collections.Generic;

using UnityEngine;

namespace Assets.DI.Scripts
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> listingMono;

        protected ContainerDI container;

        public void Initialize(ContainerDI conteiner)
        {
            this.container = conteiner;
        }

        public virtual void Installize() { }

        public virtual void InstallizeMono()
        {
            foreach (var mono in listingMono)
            {
                container.Reg(mono);
            }
        }
    }
}