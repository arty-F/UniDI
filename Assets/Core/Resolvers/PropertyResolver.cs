using System;
using System.Reflection;

namespace Assets.Core.Resolvers
{
    internal class PropertyResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;

        private readonly object[] _tempResolveParams = new object[2];

        public PropertyResolver(
            GenericMethodsProvider genericMethodsProvider,
            MemberInfoProvider memberInfoProvider,
            SettersProvider settersProvider,
            InstancesProvider instancesProvider,
            BindingFlags flags)
        {
            _genericMethodsProvider = genericMethodsProvider;
            _memberInfoProvider = memberInfoProvider;
            _settersProvider = settersProvider;
            _instancesProvider = instancesProvider;
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveProperty), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            _tempResolveParams[0] = consumer;
            var injectedProperties = _memberInfoProvider.GetPropertyInfos(consumerType);
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
