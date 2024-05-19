using System;
using System.Reflection;

namespace Assets.Core
{
    internal class FieldResolver : IResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoStorage;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesStorage;
        private readonly string _resolveMethodName;

        private object[] _tempResolveParams = new object[2];

        public FieldResolver(
            GenericMethodsProvider genericMethodsProvider, 
            MemberInfoProvider memberInfoStorage, 
            SettersProvider settersProvider, 
            InstancesProvider instancesProvider)
        {
            _genericMethodsProvider = genericMethodsProvider;
            _memberInfoStorage = memberInfoStorage;
            _settersProvider = settersProvider;
            _instancesStorage = instancesProvider;
            _resolveMethodName = nameof(ResolveField);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            _tempResolveParams[0] = consumer;
            var injectedFields = _memberInfoStorage.GetFieldInfos(consumerType);
            foreach (var injectedField in injectedFields)
            {
                var resolveFieldMethod = _genericMethodsProvider.GetResolveFieldMethod(this, _resolveMethodName, consumerType, injectedField);
                _tempResolveParams[1] = injectedField;
                resolveFieldMethod.Invoke(this, _tempResolveParams);
            }
        }

        private void ResolveField<C, I>(C consumer, FieldInfo fieldInfo)
        {
            var setter = _settersProvider.GetFieldSetter<C, I>(fieldInfo);
            setter(consumer, _instancesStorage.GetInstance<I>(typeof(I)));
        }
    }
}
