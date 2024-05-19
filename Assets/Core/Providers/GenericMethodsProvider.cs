using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core
{
    internal class GenericMethodsProvider
    {
        private readonly BindingFlags _flags;
        private Dictionary<Tuple<string, Type, Type>, MethodInfo> _resolveFieldsMethodsMap = new();

        internal GenericMethodsProvider(BindingFlags flags)
        {
            _flags = flags;
        }

        internal MethodInfo GetResolveFieldMethod(IResolver resolver, string resolveMethodName, Type consumer, FieldInfo injectedField)
        {
            MethodInfo genericMethod;
            Tuple<string, Type, Type> key = Tuple.Create(resolveMethodName, consumer, injectedField.FieldType);
            if (!_resolveFieldsMethodsMap.TryGetValue(key, out genericMethod))
            {
                genericMethod = resolver.GetType()
                    .GetMethod(resolveMethodName, _flags)
                    .MakeGenericMethod(new Type[] { consumer, injectedField.FieldType });
                _resolveFieldsMethodsMap.Add(key, genericMethod);
            }

            return genericMethod;
        }
    }
}
