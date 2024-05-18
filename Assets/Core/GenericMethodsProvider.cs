using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core
{
    internal class GenericMethodsProvider
    {
        private readonly MethodInfo _baseResolveFieldMethodInfo;
        private Dictionary<(Type, Type), MethodInfo> _resolveFieldsMethodsMap = new();

        internal GenericMethodsProvider(DelegatesResolver resolver, string resolveFiledMethodName, BindingFlags flags)
        {
            _baseResolveFieldMethodInfo = resolver.GetType().GetMethod(resolveFiledMethodName, flags);
        }

        internal MethodInfo GetResolveFields(Type consumer, Type injected)
        {
            MethodInfo genericMethod;
            if (!_resolveFieldsMethodsMap.TryGetValue((consumer, injected), out genericMethod))
            {
                genericMethod = _baseResolveFieldMethodInfo.MakeGenericMethod(new Type[] { consumer, injected });
                _resolveFieldsMethodsMap.Add((consumer, injected), genericMethod);
            }

            return genericMethod;
        }
    }
}
