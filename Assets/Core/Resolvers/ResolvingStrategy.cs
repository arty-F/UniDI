using Assets.Core.Resolvers;
using System.Reflection;

namespace Assets.Core
{
    internal class ResolvingStrategy
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly SettersProvider _settersProvider;
        private readonly MemberInfoProvider _memberInfoStorage;
        private readonly InstancesProvider _instancesStorage;
        private readonly FieldResolver _fieldResolver;
        private readonly PropertyResolver _propertyResolver;

        public ResolvingStrategy(InstancesProvider instancesStorage)
        {
            _genericMethodsProvider = new GenericMethodsProvider();
            _settersProvider = new SettersProvider();
            _memberInfoStorage = new MemberInfoProvider(FLAGS);
            _instancesStorage = instancesStorage;
            _fieldResolver = new FieldResolver(_genericMethodsProvider, _memberInfoStorage, _settersProvider, _instancesStorage, FLAGS);
            _propertyResolver = new PropertyResolver(_genericMethodsProvider, _memberInfoStorage, _settersProvider, _instancesStorage, FLAGS);
        }

        internal void Resolve(object consumer)
        {
            var consumerType = consumer.GetType();
            _fieldResolver.Resolve(consumer, consumerType);
            _propertyResolver.Resolve(consumer, consumerType);
        }
    }
}
