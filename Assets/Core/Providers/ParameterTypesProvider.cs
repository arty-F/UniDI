using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core.Providers
{
    internal class ParameterTypesProvider
    {
        private readonly Dictionary<MethodInfo, Type[]> _parameterTypeMap = new();

        internal Type[] GetParameterTypes(MethodInfo methodInfo)
        {
            if (!_parameterTypeMap.TryGetValue(methodInfo, out Type[] injectedParameterTypes))
            {
                injectedParameterTypes = GetInjectedParameterTypes(methodInfo);
                _parameterTypeMap.Add(methodInfo, injectedParameterTypes);
            }
            return injectedParameterTypes;
        }

        private Type[] GetInjectedParameterTypes(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var result = new Type[parameters.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = parameters[i].ParameterType;
            }
            return result;
        }
    }
}
