using System;
using System.Reflection;

namespace Assets.Core
{
    internal class ResolversProvider
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly SettersProvider _settersProvider;
        private readonly MemberInfoProvider _memberInfoStorage;
        private readonly InstancesProvider _instancesStorage;
        private readonly FieldResolver _fieldResolver;

        public ResolversProvider(InstancesProvider instancesStorage)
        {
            _genericMethodsProvider = new GenericMethodsProvider();
            _settersProvider = new SettersProvider();
            _memberInfoStorage = new MemberInfoProvider(FLAGS);
            _instancesStorage = instancesStorage;
            _fieldResolver = new FieldResolver(_genericMethodsProvider, _memberInfoStorage, _settersProvider, _instancesStorage, FLAGS);
        }

        internal void Resolve(object consumer)
        {
            var consumerType = consumer.GetType();
            _fieldResolver.Resolve(consumer, consumerType);
        }
    }
}
