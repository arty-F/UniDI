using UniDI.Providers;
using UniDI.Resolvers;
using UniDI.Utils;
using System.Reflection;

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

        public ResolvingStrategy(InstancesProvider instancesStorage)
        {
            _genericMethodsProvider = new GenericMethodsProvider();
            _settersProvider = new SettersProvider();
            _memberInfoProvider = new MemberInfoProvider(FLAGS);
            _instancesProvider = instancesStorage;
            _parameterTypesProvider = new ParameterTypesProvider();

            var providersDto = new ProvidersDto(_genericMethodsProvider, _settersProvider, _memberInfoProvider, _instancesProvider, _parameterTypesProvider);
            _fieldResolver = new FieldResolver(providersDto, FLAGS);
            _propertyResolver = new PropertyResolver(providersDto, FLAGS);
            _methodResolver = new MethodResolver(providersDto, FLAGS);
        }

        internal void Resolve(object consumer)
        {
            var consumerType = consumer.GetType();
            _fieldResolver.Resolve(consumer, consumerType);
            _propertyResolver.Resolve(consumer, consumerType);
            _methodResolver.Resolve(consumer, consumerType);
        }
    }
}
