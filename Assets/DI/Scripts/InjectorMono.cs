using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.DI.Scripts
{
    public class InjectorMono : Injector
    {
        public InjectorMono(Dictionary<Type, Type> registratedsMap, Dictionary<Type, object> injectedsMap,
                                ContainerDI container = null) : base(registratedsMap, injectedsMap, container) { }

        public override void PerfomInject(Type type, object target = null)
        {
            InjectMethod(type, target);
        }

        private void InjectMethod(Type type, object target)
        {
            MethodInfo method = type.GetMethod("Initialize");

            if (method != null)
            {
                object[] args = GetAllParametersType(method);

                object obj = method.Invoke(target, args);

                injectedsMap[type] = target;
            }
            else
            {
                throw new Exception($"Method 'Initialize' not found in type {type.Name}");
            }
        }

        private object[] GetAllParametersType(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                    args[i] = GetObject(parameters[i].ParameterType);
            }

            return args;
        }
    }
}