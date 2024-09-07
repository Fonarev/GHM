using System;
using System.Collections.Generic;
using System.Reflection;

using Unity.VisualScripting.FullSerializer.Internal;

namespace Assets.DI.Scripts
{
    public class Injector 
    {
        private readonly Dictionary<Type, Type> registratedsMap;
        protected Dictionary<Type, object> injectedsMap;

        private ContainerDI container;

        public Injector(Dictionary<Type, Type> registratedsMap, Dictionary<Type, object> injectedsMap, ContainerDI container = null)
        {
            this.registratedsMap = registratedsMap;
            this.injectedsMap = injectedsMap;
            this.container = container;
        }

        public virtual void PerfomInject(Type type, object target = null)
        {
            InjectConstructor(type);
        }

        private void InjectConstructor(Type type)
        {
            ConstructorInfo constructor = GetConstructor(type);
            object[] args = GetAllParametersType(constructor);

            object obj = constructor.Invoke(args);

            injectedsMap[type] = obj;
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetDeclaredConstructors();
            ConstructorInfo constructor = constructors[0];

            if (constructor == null)
            {
                var con = type.GetConstructor(Type.EmptyTypes);
                return con;
            }

            return constructor;
        }

        private object[] GetAllParametersType(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] args = new object[parameters.Length];

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                    args[i] = GetObject(parameters[i].ParameterType);
            }

            return args;
        }

        protected object GetObject(Type type)
        {
            if (!injectedsMap.ContainsKey(type))
            {
                if (registratedsMap.ContainsKey(type))
                {
                    InjectConstructor(registratedsMap[type]);
                    return injectedsMap[type];
                }
                else
                {
                    if (container != null)
                        return ForeachContainer(container, type);
                    else
                        throw new NullReferenceException($"No type in Containers {type}");
                }
            }

            return injectedsMap[type];
        }

        private object ForeachContainer(ContainerDI container, Type type)
        {
            if (!container.injectedsMap.ContainsKey(type))
            {
                if (container.registratedsMap.ContainsKey(type))
                {
                    InjectConstructor(container.registratedsMap[type]);
                    return container.injectedsMap[type];
                }
                else
                {
                    throw new NullReferenceException($"No type in registratedsMap {type}");
                }
            }

            return container.injectedsMap[type];
        }
    }
}