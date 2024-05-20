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
            return GetResolveMethod(baseResolveMethod, consumer, injectedField.FieldType);
        }

        internal MethodInfo GetResolvePropertyMethod(MethodInfo baseResolveMethod, Type consumer, PropertyInfo injectedProperty)
        {
            return GetResolveMethod(baseResolveMethod, consumer, injectedProperty.PropertyType);
        }

        private MethodInfo GetResolveMethod(MethodInfo baseResolveMethod, Type consumer, Type injected)
        {
            MethodInfo genericMethod;
            Tuple<MethodInfo, Type, Type> key = Tuple.Create(baseResolveMethod, consumer, injected);
            if (!_resolveFieldsMethodsMap.TryGetValue(key, out genericMethod))
            {
                genericMethod = baseResolveMethod.MakeGenericMethod(new Type[] { consumer, injected });
                _resolveFieldsMethodsMap.Add(key, genericMethod);
            }

            return genericMethod;
        }
    }
}
