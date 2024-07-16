using UniDI.Providers;
using UniDI.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniDI.Settings;

namespace UniDI.Resolvers
{
    internal class MethodResolver
    {
        private readonly UniDISettings _settings;
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly ParameterTypesProvider _parameterTypesProvider;
        private readonly MethodInfo _baseGetParameterMethod;
        private readonly Dictionary<MethodInfo, object[]> _methodParametersMap;
        private readonly Dictionary<int, Dictionary<MethodInfo, object[]>> _methodIdParametersMap;
        private readonly object[] _tempResolveParams = new object[1];
        private readonly List<MethodInfo> _tempReleasingMapKeys = new();

        internal MethodResolver(UniDISettings settings, ProvidersDto providersDto, BindingFlags flags)
        {
            _settings = settings;
            _methodParametersMap = new(settings.InjectedMethods);
            _methodIdParametersMap = new(settings.LocalScopes);
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _parameterTypesProvider= providersDto.ParameterTypesProvider;
            _baseGetParameterMethod = GetType().GetMethod(nameof(GetParameterInstance), flags);
        }

        internal void Resolve(object consumer, Type consumerType, int? id = null)
        {
            var injectedMethods = _memberInfoProvider.GetMethodInfos(consumerType);
            if (injectedMethods.Length == 0)
            {
                return;
            }

            foreach (var injectedMethod in injectedMethods)
            {
                ResolveMethod(consumer, injectedMethod, id);
            }
        }

        internal void ClearGlobalCache()
        {
            _methodParametersMap.Clear();
        }

        internal void ClearLocalCache()
        {
            _methodIdParametersMap.Clear();
        }

        internal void ClearLocalCache(int id)
        {
            _methodIdParametersMap.Remove(id);
        }

        internal void ClearCacheForParameterType(Type type, int? id = null)
        {
            var map = id == null ? _methodParametersMap : _methodIdParametersMap[id.Value];
            foreach (var key in map.Keys)
            {
                foreach (var instance in map[key])
                {
                    if (instance.GetType() == type)
                    {
                        _tempReleasingMapKeys.Add(key);
                        break;
                    }
                }
            }

            foreach (var key in _tempReleasingMapKeys)
            {
                map.Remove(key);
            }
            _tempReleasingMapKeys.Clear();
        }

        private void ResolveMethod(object consumer, MethodInfo methodInfo, int? id = null)
        {
            if (id != null && !_methodIdParametersMap.ContainsKey(id.Value))
            {
                _methodIdParametersMap.Add(id.Value, new Dictionary<MethodInfo, object[]>(_settings.InjectedLocalMethods));
            }

            var map = id == null ? _methodParametersMap : _methodIdParametersMap[id.Value];

            if (!map.TryGetValue(methodInfo, out object[] methodParameters))
            {
                var injectedParameterTypes = _parameterTypesProvider.GetParameterTypes(methodInfo);
                methodParameters = new object[injectedParameterTypes.Length];
                _tempResolveParams[0] = id;

                for (int i = 0; i < methodParameters.Length; i++)
                {
                    var getInstance = _genericMethodsProvider.GetParameterInstanceMethod(_baseGetParameterMethod, injectedParameterTypes[i]);
                    methodParameters[i] = getInstance.Invoke(this, _tempResolveParams);
                }
                map.Add(methodInfo, methodParameters);
            }
            methodInfo.Invoke(consumer, methodParameters);
        }

        private I GetParameterInstance<I>(int? id)
        {
            if (id == null)
            {
                return _instancesProvider.GetInstance<I>(typeof(I));
            }
            else
            {
                return _instancesProvider.GetInstance<I>(typeof(I), id.Value);
            }
        }
    }
}
