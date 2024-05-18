using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Assets.Core
{
    internal class DelegatesResolver
    {
        private const int TEMP_TYPES_LIST_SIZE = 10;
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private Dictionary<Type, object> _injectedInstances = new();

        private Dictionary<Type, Type[]> _consumerInjectedFields = new();

        private List<Type> _tempTypes = new(TEMP_TYPES_LIST_SIZE);
        private object[] _tempConsumerArray = new object[1];

        private GenericMethodsProvider _genericMethodsProvider;

        public DelegatesResolver()
        {
            _genericMethodsProvider = new GenericMethodsProvider(this, nameof(ResolveField), FLAGS);
        }

        internal void Resolve(object consumer)
        {
            _tempConsumerArray[0] = consumer;
            ResolveFields(consumer);
        }

        private void ResolveFields(object consumer)
        {
            var consumerType = consumer.GetType();
            Type[] injectedTypes;
            if (!_consumerInjectedFields.TryGetValue(consumerType, out injectedTypes))
            {
                injectedTypes = CreateInjectedFieldsMap(consumerType);
                _consumerInjectedFields.Add(consumerType, injectedTypes);
            }

            foreach (var injectedType in injectedTypes)
            {
                var resolveFieldMethod = _genericMethodsProvider.GetResolveFields(consumerType, injectedType);
                resolveFieldMethod.Invoke(this, _tempConsumerArray);
            }
        }

        private void ResolveField<C, I>(C consumer)
        {
            var fields = consumer.GetType().GetFields(FLAGS);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<Inject>() != null)
                {
                    var setter = CreateSetter<C, I>(fieldInfo);
                    setter(consumer, (I)_injectedInstances[typeof(I)]);
                }
            }
        }

        private Type[] CreateInjectedFieldsMap(Type type)
        {
            var fields = type.GetFields(FLAGS);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<Inject>() != null)
                {
                    _tempTypes.Add(fieldInfo.FieldType);
                }
            }

            var result = _tempTypes.ToArray();
            _tempTypes.Clear();
            return result;
        }

        internal void Inject<I>(I injected)
        {
            var type = typeof(I);
            if (_injectedInstances.ContainsKey(type))
            {
                Debug.Log($"Type {type.Name} are reinjected.");
                _injectedInstances[type] = injected;
            }
            else
            {
                _injectedInstances.Add(typeof(I), injected);
            }
        }

        private Action<C, I> CreateSetter<C, I>(MethodInfo method)
        {
            return (Action<C, I>)Delegate.CreateDelegate(typeof(Action<C, I>), null, method);
        }

        private Action<C, I> CreateSetter<C, I>(FieldInfo field)
        {
            var methodName = field.ReflectedType.FullName + ".set_" + field.Name;
            var setterMethod = new DynamicMethod(methodName, null, new Type[2] { typeof(C), typeof(I) }, true);
            var gen = setterMethod.GetILGenerator();
            if (field.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stfld, field);
            }
            gen.Emit(OpCodes.Ret);
            return (Action<C, I>)setterMethod.CreateDelegate(typeof(Action<C, I>));
        }

        private Action<C, I> CreateSetter<C, I>(PropertyInfo property) =>
            (Action<C, I>)Delegate.CreateDelegate(typeof(Action<C, I>), null, property.GetSetMethod(true));
    }
}
