using System;
using System.Collections.Generic;

namespace Assets.DI.Scripts
{
    public class ContainerDI 
    {
        public Dictionary<Type, Type> registratedsMap = new();
        public Dictionary<Type, object> injectedsMap = new();

        private Injector injector;
        private InjectorMono injectorMono;
        private Type type;

        public ContainerDI(ContainerDI container = null)
        {
            injector = new Injector(registratedsMap, injectedsMap, container);
            injectorMono = new InjectorMono(registratedsMap, injectedsMap, container);
        }

        public ContainerDI Reg<T>() where T : class
        {
            type = typeof(T);

            if (!registratedsMap.ContainsKey(type)) registratedsMap[type] = type;

            else throw new Exception($"This Type already exists!!!{type}");

            return this;
        }

        public ContainerDI To<T>() where T : class
        {
            var _type = typeof(T);

            if (!registratedsMap.ContainsKey(_type)) registratedsMap[type] = _type;

            else throw new Exception($"This type Object already exists!!!{type}");

            return this;
        }

        public ContainerDI Perform()
        {
            injector.PerfomInject(type);

            return this;
        }

        public void Reg(object obj)
        {
            type = obj.GetType();

            if (!injectedsMap.ContainsKey(type)) injectorMono.PerfomInject(type, obj);

            else throw new Exception($"This type Object already exists!!!{type}");
        }   

    }
}