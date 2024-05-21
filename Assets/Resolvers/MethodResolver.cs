using Assets.Core.Providers;
using Assets.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core.Resolvers
{
    internal class MethodResolver
    {
        private ResolvingType _resolvingType;
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly ParameterTypesProvider _parameterTypesProvider;
        private readonly MethodInfo _baseGetParameterMethod;
        private readonly Dictionary<MethodInfo, object[]> _methodParametersMap = new();

        public MethodResolver(ResolvingType resolvingType, ProvidersDto providersDto, BindingFlags flags)
        {
            _resolvingType = resolvingType;
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _parameterTypesProvider= providersDto.ParameterTypesProvider;
            _baseGetParameterMethod = GetType().GetMethod(nameof(GetParameterInstance), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            var injectedMethods = _memberInfoProvider.GetMethodInfos(consumerType);
            if (injectedMethods.Length == 0)
            {
                return;
            }

            foreach (var injectedMethod in injectedMethods)
            {
                ResolveMethod(consumer, injectedMethod);
            }
        }

        private void ResolveMethod(object consumer, MethodInfo methodInfo)
        {
            if (!_methodParametersMap.TryGetValue(methodInfo, out object[] methodParameters))
            {
                var injectedParameterTypes = _parameterTypesProvider.GetParameterTypes(methodInfo);
                methodParameters = new object[injectedParameterTypes.Length];

                for (int i = 0; i < methodParameters.Length; i++)
                {
                    var getInstance = _genericMethodsProvider.GetParameterInstanceMethod(_baseGetParameterMethod, injectedParameterTypes[i]);
                    methodParameters[i] = getInstance.Invoke(this, null);
                }
                _methodParametersMap.Add(methodInfo, methodParameters);
            }
            methodInfo.Invoke(consumer, methodParameters);
        }

        private I GetParameterInstance<I>()
        {
            return _instancesProvider.GetInstance<I>(typeof(I));
        }
    }
}
