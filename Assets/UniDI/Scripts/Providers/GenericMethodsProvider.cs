using System;
using System.Collections.Generic;
using System.Reflection;

namespace UniDI.Providers
{
    internal class GenericMethodsProvider
    {
        private readonly Dictionary<Tuple<MethodInfo, Type, Type>, MethodInfo> _twoTParameterMap = new();
        private readonly Dictionary<Tuple<MethodInfo, Type>, MethodInfo> _oneTParameterMap = new();

        private readonly Type[] _tempTwoParameters = new Type[2];
        private readonly Type[] _tempOneParameters = new Type[1];

        internal MethodInfo GetResolveFieldMethod(MethodInfo baseResolveMethod, Type consumer, FieldInfo injectedField)
        {
            return GetResolveMethod(baseResolveMethod, consumer, injectedField.FieldType);
        }

        internal MethodInfo GetResolvePropertyMethod(MethodInfo baseResolveMethod, Type consumer, PropertyInfo injectedProperty)
        {
            return GetResolveMethod(baseResolveMethod, consumer, injectedProperty.PropertyType);
        }

        internal MethodInfo GetParameterInstanceMethod(MethodInfo baseResolveMethod, Type parameterType)
        {
            return GetResolveMethod(baseResolveMethod, parameterType);
        }

        private MethodInfo GetResolveMethod(MethodInfo baseResolveMethod, Type consumer, Type injected)
        {
            Tuple<MethodInfo, Type, Type> key = Tuple.Create(baseResolveMethod, consumer, injected);
            if (!_twoTParameterMap.TryGetValue(key, out MethodInfo genericMethod))
            {
                _tempTwoParameters[0] = consumer;
                _tempTwoParameters[1] = injected;
                genericMethod = baseResolveMethod.MakeGenericMethod(_tempTwoParameters);
                _twoTParameterMap.Add(key, genericMethod);
            }
            return genericMethod;
        }

        private MethodInfo GetResolveMethod(MethodInfo baseResolveMethod, Type injected)
        {
            Tuple<MethodInfo, Type> key = Tuple.Create(baseResolveMethod, injected);
            if (!_oneTParameterMap.TryGetValue(key, out MethodInfo genericMethod))
            {
                _tempOneParameters[0] = injected;
                genericMethod = baseResolveMethod.MakeGenericMethod(_tempOneParameters);
                _oneTParameterMap.Add(key, genericMethod);
            }
            return genericMethod;
        }
    }
}
