using MonoInjector.Providers;
using MonoInjector.Utils;
using System;
using System.Reflection;

namespace MonoInjector.Resolvers
{
    internal class PropertyResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;

        private readonly object[] _tempResolveParams = new object[2];
        
        public PropertyResolver(ProvidersDto providersDto, BindingFlags flags)
        {
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _settersProvider = providersDto.SettersProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveProperty), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            var injectedProperties = _memberInfoProvider.GetPropertyInfos(consumerType);
            if (injectedProperties.Length == 0)
            {
                return;
            }

            _tempResolveParams[0] = consumer;
            foreach (var injectedProperty in injectedProperties)
            {
                var resolvePropertyMethod = _genericMethodsProvider.GetResolvePropertyMethod(_baseResolveMethod, consumerType, injectedProperty);
                _tempResolveParams[1] = injectedProperty;
                resolvePropertyMethod.Invoke(this, _tempResolveParams);
            }
        }

        private void ResolveProperty<C, I>(C consumer, PropertyInfo prpertyInfo)
        {
            var setter = _settersProvider.GetPropertySetter<C, I>(prpertyInfo);
            setter(consumer, _instancesProvider.GetInstance<I>(typeof(I)));
        }
    }
}
