using UniDI.Providers;
using UniDI.Utils;
using System;
using System.Reflection;

namespace UniDI.Resolvers
{
    internal class PropertyResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;
        private readonly object[] _tempResolveParams = new object[3];

        internal PropertyResolver(ProvidersDto providersDto, BindingFlags flags)
        {
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _settersProvider = providersDto.SettersProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveProperty), flags);
        }

        internal void Resolve(object consumer, Type consumerType, int? id = null)
        {
            var injectedProperties = _memberInfoProvider.GetPropertyInfos(consumerType);
            if (injectedProperties.Length == 0)
            {
                return;
            }

            _tempResolveParams[0] = consumer;
            _tempResolveParams[2] = id;
            foreach (var injectedProperty in injectedProperties)
            {
                var resolvePropertyMethod = _genericMethodsProvider.GetResolvePropertyMethod(_baseResolveMethod, consumerType, injectedProperty);
                _tempResolveParams[1] = injectedProperty;
                resolvePropertyMethod.Invoke(this, _tempResolveParams);
            }
        }

        private void ResolveProperty<C, I>(C consumer, PropertyInfo prpertyInfo, int? id)
        {
            var setter = _settersProvider.GetPropertySetter<C, I>(prpertyInfo);
            var instance = id == null
                ? _instancesProvider.GetInstance<I>(typeof(I))
                : _instancesProvider.GetInstance<I>(typeof(I), id.Value);
            setter(consumer, instance);
        }
    }
}
