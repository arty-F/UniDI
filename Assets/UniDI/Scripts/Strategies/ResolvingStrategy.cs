using System;
using System.Reflection;
using UniDI.Providers;
using UniDI.Resolvers;
using UniDI.Settings;
using UniDI.Utils;

namespace UniDI.Strategies
{
    internal class ResolvingStrategy
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly SettersProvider _settersProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly ParameterTypesProvider _parameterTypesProvider;
        private readonly FieldResolver _fieldResolver;
        private readonly PropertyResolver _propertyResolver;
        private readonly MethodResolver _methodResolver;

        internal ResolvingStrategy(InstancesProvider instancesStorage, UniDISettings settings)
        {
            _genericMethodsProvider = new GenericMethodsProvider(settings);
            _settersProvider = new SettersProvider(settings);
            _memberInfoProvider = new MemberInfoProvider(settings, FLAGS);
            _instancesProvider = instancesStorage;
            _parameterTypesProvider = new ParameterTypesProvider(settings);

            var providersDto = new ProvidersDto(_genericMethodsProvider, _settersProvider, _memberInfoProvider, _instancesProvider, _parameterTypesProvider);
            _fieldResolver = new FieldResolver(providersDto, FLAGS);
            _propertyResolver = new PropertyResolver(providersDto, FLAGS);
            _methodResolver = new MethodResolver(settings, providersDto, FLAGS);
        }

        internal void Resolve(object consumer, int? id = null)
        {
            var consumerType = consumer.GetType();
            _fieldResolver.Resolve(consumer, consumerType, id);
            _propertyResolver.Resolve(consumer, consumerType, id);
            _methodResolver.Resolve(consumer, consumerType, id);
        }

        internal void ClearGlobalCache()
        {
            _methodResolver.ClearGlobalCache();
        }

        internal void ClearLocalCache()
        {
            _methodResolver.ClearLocalCache();
        }

        internal void ClearLocalCache(int id)
        {
            _methodResolver.ClearLocalCache(id);
        }

        internal void ClearCacheForParameterType(Type type, int? id = null)
        {
            _methodResolver.ClearCacheForParameterType(type, id);
        }
    }
}
