using Assets.Core.Utils;
using System;
using System.Reflection;

namespace Assets.Core
{
    internal class FieldResolver
    {
        private readonly ResolvingType _resolvingType;
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;

        private readonly object[] _tempResolveParams = new object[2];

        public FieldResolver(ResolvingType resolvingType, ProvidersDto providersDto, BindingFlags flags)
        {
            _resolvingType = resolvingType;
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _settersProvider = providersDto.SettersProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveField), flags);
        }

        public void Resolve(object consumer, Type consumerType)
        {
            var injectedFields = _memberInfoProvider.GetFieldInfos(consumerType);
            if (injectedFields.Length == 0)
            {
                return;
            }

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
