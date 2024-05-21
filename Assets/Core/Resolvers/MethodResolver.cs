using Assets.Core.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core.Resolvers
{
    internal class MethodResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly ParameterTypesProvider _parameterTypesProvider;
        private readonly MethodInfo _baseGetParameterMethod;
        private readonly Dictionary<MethodInfo, object[]> _methodParametersMap = new();

        public MethodResolver(
            GenericMethodsProvider genericMethodsProvider,
            MemberInfoProvider memberInfoProvider,
            InstancesProvider instancesProvider,
            ParameterTypesProvider parameterTypesProvider,
            BindingFlags flags)
        {
            _genericMethodsProvider = genericMethodsProvider;
            _memberInfoProvider = memberInfoProvider;
            _instancesProvider = instancesProvider;
            _parameterTypesProvider = parameterTypesProvider;
            var type = GetType();
            _baseGetParameterMethod = type.GetMethod(nameof(GetParameterInstance), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            var injectedMethods = _memberInfoProvider.GetMethodInfos(consumerType);
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
