using UniDI.Providers;
using UniDI.Utils;
using System;
using System.Reflection;

namespace UniDI.Resolvers
{
    internal class FieldResolver
    {
        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly SettersProvider _settersProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly MethodInfo _baseResolveMethod;
        private readonly object[] _tempResolveParams3 = new object[3];

        internal FieldResolver(ProvidersDto providersDto, BindingFlags flags)
        {
            _genericMethodsProvider = providersDto.GenericMethodsProvider;
            _memberInfoProvider = providersDto.MemberInfoProvider;
            _settersProvider = providersDto.SettersProvider;
            _instancesProvider = providersDto.InstancesProvider;
            _baseResolveMethod = GetType().GetMethod(nameof(ResolveField), flags);
        }

        internal void Resolve(object consumer, Type consumerType, int? id = null)
        {
            var injectedFields = _memberInfoProvider.GetFieldInfos(consumerType);
            if (injectedFields.Length == 0)
            {
                return;
            }

            _tempResolveParams3[0] = consumer;
            _tempResolveParams3[2] = id;
            foreach (var injectedField in injectedFields)
            {
                var resolveFieldMethod = _genericMethodsProvider.GetResolveFieldMethod(_baseResolveMethod, consumerType, injectedField);
                _tempResolveParams3[1] = injectedField;
                resolveFieldMethod.Invoke(this, _tempResolveParams3);
            }
        }

        private void ResolveField<C, I>(C consumer, FieldInfo fieldInfo, int? id)
        {
            var setter = _settersProvider.GetFieldSetter<C, I>(fieldInfo);
            var instance = id == null 
                ? _instancesProvider.GetInstance<I>(typeof(I)) 
                : _instancesProvider.GetInstance<I>(typeof(I), id.Value);
            setter(consumer, instance);
        }
    }
}
