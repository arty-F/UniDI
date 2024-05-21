using System;
using System.Reflection;

namespace Assets.Core
{
    internal class FieldResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;

        private readonly object[] _tempResolveParams = new object[2];

        public FieldResolver(
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
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveField), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            var injectedFields = _memberInfoProvider.GetFieldInfos(consumerType);
            _tempResolveParams[0] = consumer;
            foreach (var injectedField in injectedFields)
            {
                var resolveFieldMethod = _genericMethodsProvider.GetResolveFieldMethod(_baseResolveMethod, consumerType, injectedField);
                _tempResolveParams[1] = injectedField;
                resolveFieldMethod.Invoke(this, _tempResolveParams);
            }
        }

        private void ResolveField<C, I>(C consumer, FieldInfo fieldInfo)
        {
            var setter = _settersProvider.GetFieldSetter<C, I>(fieldInfo);
            setter(consumer, _instancesProvider.GetInstance<I>(typeof(I)));
        }
    }
}
