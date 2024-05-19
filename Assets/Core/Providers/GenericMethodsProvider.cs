using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core
{
    internal class GenericMethodsProvider
    {
        private Dictionary<Tuple<MethodInfo, Type, Type>, MethodInfo> _resolveFieldsMethodsMap = new();

        internal MethodInfo GetResolveFieldMethod(MethodInfo baseResolveMethod, Type consumer, FieldInfo injectedField)
        {
            MethodInfo genericMethod;
            Tuple<MethodInfo, Type, Type> key = Tuple.Create(baseResolveMethod, consumer, injectedField.FieldType);
            if (!_resolveFieldsMethodsMap.TryGetValue(key, out genericMethod))
            {
                genericMethod = baseResolveMethod.MakeGenericMethod(new Type[] { consumer, injectedField.FieldType });
                _resolveFieldsMethodsMap.Add(key, genericMethod);
            }

            return genericMethod;
        }
    }
}
